using AMMEdit.amm;
using AMMEdit.amm.blocks;
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

        public List<Tuple<OLAYBlock, OATTBlock>> ObjectLayerBlocks { get; private set; }

        public List<GenericFieldBlock> FlagBlocks { get; private set; }

        private DatFile DataFileReference;

        public SelectedTileDataSource SelectedTile { get; private set; }

        private Bitmap renderedMap = new Bitmap(256 * 16, 256 * 16);
        private Thread previewGenerationThread;
        private Bitmap tileSheet;
        private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private List<Tuple<AMObject, Point>> placedObjects = new List<Tuple<AMObject, Point>>();

        private List<Tuple<AMObject, Point>> deletedObjects = new List<Tuple<AMObject, Point>>();

        private enum EditorState
        {
            VIEW,
            PLACE_OBJECT,
            DELETE_OBJECT,
            VIEW_FLAG
        }

        private EditorState editorState = EditorState.VIEW;

        public TextureMap(TNAMBlock textureBlock, TLAYBlock mapBlock, TLAYBlock mapBlock2 = null, List<Tuple<OLAYBlock, OATTBlock>> objectLayers = null, DatFile dataFile = null, List<GenericFieldBlock> flagLayers = null)
        {
            TNameBlock = textureBlock;
            LayerBlock = mapBlock;
            LayerBlock2 = mapBlock2;
            SelectedTile = new SelectedTileDataSource();
            ObjectLayerBlocks = objectLayers;
            DataFileReference = dataFile;
            FlagBlocks = flagLayers;

            InitializeComponent();
        }

        private void TextureMap_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = TNameBlock.GetTileImage(1);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            comboBoxMode.DataSource = Enum.GetNames(typeof(EditorState));
            comboBoxMode.SelectedItem = editorState;

            numericUpDown1.Maximum = TNameBlock.NumTiles;

            propertyGrid1.SelectedObject = SelectedTile;

            if (DataFileReference != null)
            {
                listBox1.DataSource = DataFileReference.GetPlaceableObjects();
                listBox1.DisplayMember = "LabelText";
            }
            else
            {
                listBox1.Enabled = false;
                listBox1.Hide();
                comboBoxMode.Enabled = false;
                comboBoxMode.Hide();
            }

            if (LayerBlock == null)
            {
                label1.Visible = false;
                pictureBox1.Image = (Bitmap)TNameBlock.TextureImagesheet.Clone();
                textureMapPreviewBox.Image = pictureBox1.Image;
            }
            else
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

            // render objects from front to back, right to left.
            if (ObjectLayerBlocks != null && DataFileReference != null)
            {
                ObjectLayerBlocks.ForEach(objectLayer =>
                {
                    foreach (OLAYObject obj in objectLayer.Item1.GetRenderOrderedObjects(DataFileReference))
                    {
                        if (DataFileReference.ObjectsByCatAndInstance.ContainsKey(obj.m_itemCategory) == false)
                        {
                            Debug.WriteLine("Could not place object " + obj.m_itemCategory + " type " + obj.m_itemSubType + "(x:" + obj.m_itemPosX + ",y:" + obj.m_itemPosY + "): Object not defined in loaded DAT file.");

                            continue;
                        }
                        else if (DataFileReference.ObjectsByCatAndInstance[obj.m_itemCategory].ContainsKey(obj.m_itemSubType) == false)
                        {
                            Debug.WriteLine("Could not place object " + obj.m_itemCategory + " type " + obj.m_itemSubType + "(x:" + obj.m_itemPosX + ",y:" + obj.m_itemPosY + "): Object defined, but type not defined in loaded DAT file.");

                            continue;
                        }

                        AMObject aObj = DataFileReference.GetObject(obj.m_itemCategory, obj.m_itemSubType);

                        if (aObj != null && aObj.SpriteImage != null)
                        {
                            mapBuffer.Graphics.DrawImage(aObj.SpriteImage, new Rectangle(new Point(obj.m_itemPosX, obj.m_itemPosY), new Size(aObj.SpriteImage.Width, aObj.SpriteImage.Height)), 0, 0, aObj.SpriteImage.Width, aObj.SpriteImage.Height, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            Debug.WriteLine("Image not loaded for object " + obj.m_itemCategory + " type " + obj.m_itemSubType + "(x:" + obj.m_itemPosX + ",y:" + obj.m_itemPosY + ")");
                        }
                    }

                    /*for (int o = 0; o < objectLayer.Item1.m_numObjects; o++)
                    {
                        OLAYObject obj = objectLayer.Item1.GetObjectByIndex(o);

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
                    }*/
                });
            }

            mapBuffer.Render();
            mapBuffer.Dispose();

            new Task(() =>
            {
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

            if (editorState == EditorState.VIEW || editorState == EditorState.DELETE_OBJECT)
            {
                if (ObjectLayerBlocks != null &&
                    ObjectLayerBlocks.Count > 0 &&
                    DataFileReference != null)
                {
                    List<KeyValuePair<int, Tuple<OLAYObject, OATTBlock, OLAYBlock>>> found = new List<KeyValuePair<int, Tuple<OLAYObject, OATTBlock, OLAYBlock>>>();

                    // This is crap - I'm so sorry
                    // We need access to the OATT block entry AND the index
                    ObjectLayerBlocks.ForEach(
                        objLayer =>
                        {
                            found.AddRange(
                                objLayer.Item1.GetObjectsByLocation(e.X, e.Y, DataFileReference)
                                    .ConvertAll(
                                        objectsByLocationAndIndex =>
                                            new KeyValuePair<int, Tuple<OLAYObject, OATTBlock, OLAYBlock>>(
                                                objectsByLocationAndIndex.Key,
                                                new Tuple<OLAYObject, OATTBlock, OLAYBlock>(objectsByLocationAndIndex.Value, objLayer.Item2, objLayer.Item1)))
                                );
                        }
                    );

                    if (found.Count > 0)
                    {
                        List<Tuple<AMObject, OLAYObject, OATTBlock, PlaceableObject, OLAYBlock, int>> selectedObjects = found.ConvertAll(kv =>
                        {
                            return new Tuple<AMObject, OLAYObject, OATTBlock, PlaceableObject, OLAYBlock, int>(
                                DataFileReference.GetObject(kv.Value.Item1.m_itemCategory, kv.Value.Item1.m_itemSubType),
                                kv.Value.Item1,
                                kv.Value.Item2,
                                (PlaceableObject)kv.Value.Item2.GetPlaceableByObjectIndex(kv.Key),
                                kv.Value.Item3,
                                kv.Key
                                );
                        });

                        if (editorState == EditorState.VIEW)
                        {
                            SelectedTile.UpdateSelectedObjects(selectedObjects);
                            listBox1.DataSource = selectedObjects.ConvertAll(foundObjects => foundObjects.Item1);
                            listBox1.DisplayMember = "LabelText";
                        }
                        else if (editorState == EditorState.DELETE_OBJECT)
                        {
                            // Don't delete objects more than once
                            selectedObjects.RemoveAll(kv => deletedObjects.Contains(new Tuple<AMObject, Point>(kv.Item1, new Point(kv.Item2.m_itemPosX, kv.Item2.m_itemPosY))));

                            // If given multiple options, select the first
                            if (selectedObjects.Count > 0)
                            {
                                Tuple<AMObject, OLAYObject, OATTBlock, PlaceableObject, OLAYBlock, int> toDelete = selectedObjects[0];

                                // register delete object
                                deletedObjects.Add(new Tuple<AMObject, Point>(toDelete.Item1, new Point(toDelete.Item2.m_itemPosX, toDelete.Item2.m_itemPosY)));

                                // delete the object
                                toDelete.Item5.DeleteObject(toDelete.Item6);

                                if (toDelete.Item4 != null)
                                {
                                    toDelete.Item3.RemovePlaceable(toDelete.Item4);
                                }

                                toDelete.Item3.MarkObjectIndexDeleted(toDelete.Item6);

                            }
                        }
                    }
                    else
                    {
                        SelectedTile.UpdateSelectedObjects(new List<Tuple<AMObject, OLAYObject, OATTBlock, PlaceableObject, OLAYBlock, int>>());
                        listBox1.DataSource = null;
                    }
                }
            }

            // Update general tile selection
            if (LayerBlock != null)
            {
                ushort textureID = (ushort)(LayerBlock.GetTextureIDAtLocation(SelectedTile.Tile.X, SelectedTile.Tile.Y) + Convert.ToUInt16(1));
                SelectedTile.UpdateRawValue(textureID);
                numericUpDown1.Value = textureID;
            }
            else
            {
                // Set to the selected texture ID
                int textureID = SelectedTile.Tile.X + (SelectedTile.Tile.Y * TNameBlock.NumTilesPerRow) + 1;

                SelectedTile.UpdateRawValue(textureID);
                numericUpDown1.Value = textureID;
            }

            propertyGrid1.SelectedObject = SelectedTile;

            // handle flap map selection
            if (editorState == EditorState.VIEW_FLAG)
            {
                GenericFieldBlock selectedFlagBlock = (GenericFieldBlock)listBox2.SelectedItem;

                if (selectedFlagBlock != null && selectedFlagBlock.FlagMap != null)
                {
                    SelectedTile.UpdatePosition(new Point(e.X, e.Y), selectedFlagBlock.FlagMap);
                }
            }

            // handle object placement
            if (editorState == EditorState.PLACE_OBJECT)
            {
                AMObject selectedPrototype = (AMObject)listBox1.SelectedItem;
                if (selectedPrototype != null)
                {
                    placedObjects.Add(new Tuple<AMObject, Point>(selectedPrototype, new Point(e.X, e.Y)));

                    if (ObjectLayerBlocks != null && ObjectLayerBlocks.Count > 0)
                    {
                        // add to first object layer for now
                        OLAYObject placedObject = new OLAYObject(selectedPrototype.TypeKey, selectedPrototype.InstanceKey, e.X, e.Y);
                        ObjectLayerBlocks[0].Item1.AddObject(placedObject);
                    }
                }
            }
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

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowse = new FolderBrowserDialog();

            if (folderBrowse.ShowDialog() == DialogResult.OK)
            {
                string outFolder = System.IO.Path.Combine(folderBrowse.SelectedPath, "TileMap");
                System.IO.Directory.CreateDirectory(outFolder);
                for (ushort f = 0; f < TNameBlock.NumTiles; f++)
                {
                    string outFile = System.IO.Path.Combine(outFolder, "tile_" + f + ".png");
                    TNameBlock.GetTileImage(f).Save(outFile, ImageFormat.Png);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ((ListBox)sender).SelectedItem;
            AMObject selectedAMObject = ((AMObject)((ListBox)sender).SelectedItem);

            if (selectedAMObject != null)
            {
                pictureBox3.Image = selectedAMObject.SpriteImage;
            }
            else
            {
                pictureBox3.Image = null;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // draw objects placed during current session
            // performance will degrade the more objects are added in a given session.
            // TODO: consider culling the list to current window only.
            placedObjects.ForEach(placement =>
            {
                e.Graphics.DrawImage(placement.Item1.SpriteImage, placement.Item2);
            });

            // draw deleted object overlays similar to above
            deletedObjects.ForEach(deletion =>
            {
                Point origin = deletion.Item2;
                Size spriteSize = deletion.Item1.SpriteImage.Size;

                // Cross out the image
                Point[] points = {
                    origin,
                    new Point(origin.X + spriteSize.Width, origin.Y + spriteSize.Height),
                    new Point(origin.X + spriteSize.Width, origin.Y),
                    new Point(origin.X, origin.Y + spriteSize.Height)
                };
                e.Graphics.DrawLines(new Pen(Color.Red, 5), points);
            });

            // draw flag map
            if (editorState == EditorState.VIEW_FLAG)
            {
                GenericFieldBlock selectedFlagBlock = (GenericFieldBlock)listBox2.SelectedItem;

                if (selectedFlagBlock != null && selectedFlagBlock.FlagMap != null)
                {
                    // TODO: render only clipped version
                    e.Graphics.DrawImage(selectedFlagBlock.FlagMap.Overlay, new Point(0, 0));
                }
            }

            // draw selected object
            Point mousePos = pictureBox1.PointToClient(Cursor.Position);

            if (editorState == EditorState.PLACE_OBJECT)
            {
                AMObject selectedPrototype = (AMObject)listBox1.SelectedItem;
                if (selectedPrototype != null)
                {
                    e.Graphics.DrawImage(selectedPrototype.SpriteImage, mousePos);
                }
            }
            else
            {
                e.Graphics.DrawRectangle(
                    new Pen(
                        Color.FromArgb(128, Color.AliceBlue)
                        ), new Rectangle(mousePos, new Size(16, 16)));
            }
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorState = (EditorState)Enum.Parse(typeof(EditorState), (string)comboBoxMode.SelectedItem);

            if (editorState == EditorState.PLACE_OBJECT)
            {
                listBox1.DataSource = DataFileReference.GetPlaceableObjects();
                listBox1.DisplayMember = "LabelText";
                listBox1.Show();
                listBox2.DataSource = null;
                listBox2.Hide();
            }
            else if (editorState == EditorState.VIEW_FLAG)
            {
                listBox2.DataSource = FlagBlocks;
                listBox2.DisplayMember = "DisplayFieldName";
                listBox1.Hide();
                listBox1.DataSource = null;
                listBox2.Show();
            }
            else
            {
                listBox1.DataSource = null;
                listBox1.Show();
                listBox2.DataSource = null;
                listBox2.Hide();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // flag map selection change
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

        [Category("Objects"), Description("Number of selected objects"), DisplayName("Count")]
        public int SelectedObjectsCount { get; private set; }

        [Category("Properties"), Description("Objects placed on map can have custom properties like script name or number of bullets."), DisplayName("Custom properties")]
        [Editor(typeof(PlaceableObject.PlaceableObjectPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public PlaceableObject PlaceableObject { get; set; }

        [Category("Flags"), Description("Flag map value"), DisplayName("Flag")]
        public int Flag { get; private set; }

        [Browsable(false)]
        public OLAYObject SelectedOLAYObject { get; private set; }

        [Browsable(false)]
        public OATTBlock SelectedOATTBlock { get; private set; }

        [Browsable(false)]
        public AMObject SelectedAMObject { get; private set; }

        [Browsable(false)]
        public int SelectedObjectIndex { get; private set; }

        public void UpdatePosition(Point p, GenericFlagMap selectedFlagMap = null)
        {
            PositionX = p.X;
            PositionY = p.Y;
            Position = p;
            Tile = new Point((int)(p.X / 16.0), (int)(p.Y / 16.0));

            if (selectedFlagMap != null)
            {
                Flag = Convert.ToInt32(selectedFlagMap.GetFlagAtLocation(Tile.X, Tile.Y));
            }
        }

        public void UpdateSelectedObjects(List<Tuple<AMObject, OLAYObject, OATTBlock, PlaceableObject, OLAYBlock, int>> objects)
        {
            SelectedObjectsCount = objects.Count;
            if (objects.Count == 1)
            {
                SelectedObjectIndex = objects[0].Item6;
                SelectedAMObject = objects[0].Item1;
                SelectedOATTBlock = objects[0].Item3;
                SelectedOLAYObject = objects[0].Item2;
                PlaceableObject = objects[0].Item4;
            }
            else
            {
                SelectedObjectIndex = 0;
                SelectedAMObject = null;
                SelectedOATTBlock = null;
                SelectedOLAYObject = null;
                PlaceableObject = null;
            }
        }

        public void UpdateRawValue(Object v)
        {
            RawValue = v;
        }
    }
}
