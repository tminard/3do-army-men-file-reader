﻿using AMMEdit.amm.blocks;
using AMMEdit.amm.blocks.subfields;
using AMMEdit.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors
{
    public partial class TextureMap : Form
    {
        public TNAMBlock TNameBlock { get; private set; }
        public TLAYBlock LayerBlock { get; private set; }
        public TLAYBlock LayerBlock2 { get; private set; }

        public List<OLAYBlock> ObjectLayerBlocks { get; private set; }

        private DatFile DataFileReference;

        public SelectedTileDataSource SelectedTile { get; private set; }

        private Bitmap renderedMap = new Bitmap(256 * 16, 256 * 16);
        private Thread previewGenerationThread;
        private Bitmap tileSheet;
        private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public TextureMap(TNAMBlock textureBlock, TLAYBlock mapBlock, TLAYBlock mapBlock2 = null, List<OLAYBlock> objectLayers = null, DatFile dataFile = null)
        {
            TNameBlock = textureBlock;
            LayerBlock = mapBlock;
            LayerBlock2 = mapBlock2;
            SelectedTile = new SelectedTileDataSource();
            ObjectLayerBlocks = objectLayers;
            DataFileReference = dataFile;

            InitializeComponent();
        }

        private void TextureMap_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = TNameBlock.GetTileImage(1);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            numericUpDown1.Maximum = TNameBlock.NumTiles;

            propertyGrid1.SelectedObject = SelectedTile;

            if (LayerBlock == null)
            {
                label1.Visible = false;
                pictureBox1.Image = (Bitmap)TNameBlock.TextureImagesheet.Clone();
                textureMapPreviewBox.Image = pictureBox1.Image;
            } else
            {
                panel1.Controls.Add(pictureBox1);
                panel1.AutoScroll = true;
                pictureBox1.BackColor = Color.Aqua;
                pictureBox1.Size = new Size(256 * 16, 256 * 16);

                tileSheet = (Bitmap)TNameBlock.TextureImagesheet.Clone();
                DrawMap();
            }
        }

        private void DrawMap()
        {
            if (previewGenerationThread != null)
            {
                previewGenerationThread.Abort();
                previewGenerationThread = null;
            }

            previewGenerationThread = new Thread(BuildMap);
            previewGenerationThread.Priority = ThreadPriority.AboveNormal;
            previewGenerationThread.IsBackground = true;

            previewGenerationThread.Start();
        }

        private void BuildMap()
        {
            // generate and display a preview of the current map
            if (LayerBlock == null)
            {
                return;
            }
            BufferedGraphicsContext currentContext;
            BufferedGraphics mapBuffer;
            currentContext = BufferedGraphicsManager.Current;

            mapBuffer = currentContext.Allocate(Graphics.FromImage(renderedMap), new Rectangle(0, 0, 256 * 16, 256 * 16));

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (x >= 256 || y >= 256) continue;

                    UInt16 imageNum = LayerBlock.GetTextureIDAtLocation(x, y);

                    int posX = x * 16;
                    int posY = y * 16;
                    int srcX = (imageNum % TNameBlock.NumTilesPerRow) * 16;
                    int srcY = (imageNum / TNameBlock.NumTilesPerRow) * 16;

                    mapBuffer.Graphics.DrawImage(tileSheet, new Rectangle(new Point(posX, posY), new Size(16, 16)), srcX, srcY, 16.0F, 16.0F, GraphicsUnit.Pixel);

                    if (LayerBlock2 != null)
                    {
                        imageNum = LayerBlock2.GetTextureIDAtLocation(x, y);
                        if (imageNum < TNameBlock.NumTiles)
                        {
                            srcX = (imageNum % TNameBlock.NumTilesPerRow) * 16;
                            srcY = (imageNum / TNameBlock.NumTilesPerRow) * 16;
                            mapBuffer.Graphics.DrawImage(tileSheet, new Rectangle(new Point(posX, posY), new Size(16, 16)), srcX, srcY, 16.0F, 16.0F, GraphicsUnit.Pixel);
                        }
                    }
                }
            }

            if (ObjectLayerBlocks != null && DataFileReference != null)
            {
                ObjectLayerBlocks.ForEach(objectLayer =>
                {
                    for (int o = 0; o < objectLayer.m_numObjects; o++)
                    {
                        OLAYObject obj = objectLayer.GetObjectByIndex(o);

                        if (DataFileReference.ObjectsByCatAndInstance.ContainsKey(obj.m_itemCategory) == false)
                        {
                            Debug.WriteLine("Could not place object " + obj.m_itemCategory + " type " + obj.m_itemSubType + "(x:" + obj.m_itemPosX + ",y:" + obj.m_itemPosY + "): Object not defined in loaded DAT file.");

                            continue;
                        } else if (DataFileReference.ObjectsByCatAndInstance[obj.m_itemCategory].ContainsKey(obj.m_itemSubType) == false)
                        {
                            Debug.WriteLine("Could not place object " + obj.m_itemCategory + " type " + obj.m_itemSubType + "(x:" + obj.m_itemPosX + ",y:" + obj.m_itemPosY + "): Object defined, but type not defined in loaded DAT file.");

                            continue;
                        }

                        AMObject aObj = DataFileReference.GetObject(obj.m_itemCategory, obj.m_itemSubType);

                        if (aObj != null && aObj.SpriteImage != null)
                        {
                            mapBuffer.Graphics.DrawImage(aObj.SpriteImage, new Rectangle(new Point(obj.m_itemPosX, obj.m_itemPosY), new Size(aObj.SpriteImage.Width, aObj.SpriteImage.Height)), 0, 0, aObj.SpriteImage.Width, aObj.SpriteImage.Height, GraphicsUnit.Pixel);
                        } else
                        {
                            Debug.WriteLine("Image not loaded for object " + obj.m_itemCategory + " type " + obj.m_itemSubType + "(x:" + obj.m_itemPosX + ",y:" + obj.m_itemPosY + ")");
                        }
                    }
                });
            }

            mapBuffer.Render();
            mapBuffer.Dispose();

            new Task(() => {
                textureMapPreviewBox.Image = renderedMap;
                label1.Visible = false;
                textureMapPreviewBox.Invalidate();
                pictureBox1.Image = (Bitmap)renderedMap.Clone();
                pictureBox1.Invalidate();
            }).Start(scheduler);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown1.Maximum)
            {
                return;
            }

            pictureBox2.Image = TNameBlock.GetTileImage(Convert.ToUInt16(numericUpDown1.Value - 1));
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedTile.UpdatePosition(new Point(e.X, e.Y));

            if (LayerBlock != null)
            {
                ushort textureID = (ushort)(LayerBlock.GetTextureIDAtLocation(SelectedTile.Tile.X, SelectedTile.Tile.Y) + Convert.ToUInt16(1));
                SelectedTile.UpdateRawValue(textureID);
                numericUpDown1.Value = textureID;
            } else
            {
                // Set to the selected texture ID
                int textureID = SelectedTile.Tile.X + (SelectedTile.Tile.Y * TNameBlock.NumTilesPerRow) + 1;

                SelectedTile.UpdateRawValue(textureID);
                numericUpDown1.Value = textureID;
            }

            propertyGrid1.SelectedObject = SelectedTile;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void TextureMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
            if (renderedMap != null) renderedMap.Dispose();
            if (tileSheet != null) tileSheet.Dispose();
        }

        private void TextureMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (previewGenerationThread != null && previewGenerationThread.IsAlive)
            {
                previewGenerationThread.Abort();
            }
        }
    }

    public class SelectedTileDataSource
    {
        [Browsable(false)]
        public int PositionX { get; private set; }

        [Browsable(false)]
        public int PositionY { get; private set; }

        [Category("Selected")]
        public Point Position { get; private set; }

        [Category("Selected")]
        public Point Tile { get; private set; }

        [Category("Selected")]
        public Object RawValue { get; private set; }

        public void UpdatePosition(Point p)
        {
            PositionX = p.X;
            PositionY = p.Y;
            Position = p;
            Tile = new Point((int)(p.X / 16.0), (int)(p.Y / 16.0));
        }

        public void UpdateRawValue(Object v)
        {
            RawValue = v;
        }
    }
}
