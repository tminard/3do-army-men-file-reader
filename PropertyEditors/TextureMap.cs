using AMMEdit.amm.blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors
{
    public partial class TextureMap : Form
    {
        public TNAMBlock TNameBlock { get; private set; }
        public TLAYBlock LayerBlock { get; private set; }

        public SelectedTileDataSource SelectedTile { get; private set; }

        public TextureMap(TNAMBlock textureBlock, TLAYBlock mapBlock)
        {
            TNameBlock = textureBlock;
            LayerBlock = mapBlock;
            SelectedTile = new SelectedTileDataSource();

            InitializeComponent();
        }

        private void TextureMap_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = TNameBlock.GetTileImage(100);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            numericUpDown1.Maximum = TNameBlock.NumTiles;

            propertyGrid1.SelectedObject = SelectedTile;

            if (LayerBlock == null)
            {
                button1.Enabled = false;
                button1.Visible = false;

                pictureBox1.Image = (Bitmap)TNameBlock.TextureImagesheet.Clone();
            } else
            {
                panel1.Controls.Add(pictureBox1);
                panel1.AutoScroll = true;
                pictureBox1.BackColor = Color.Aqua;
                pictureBox1.Size = new Size(256 * 16, 256 * 16);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox2.Image = TNameBlock.GetTileImage(Convert.ToUInt16(numericUpDown1.Value));
        }

        /*
         * This is for my own testing
         * */
        private void button1_Click(object sender, EventArgs e)
        {
            DrawLayer();
        }

        private void DrawLayer()
        {
            pictureBox1.Update();

            BufferedGraphicsContext currentContext;
            BufferedGraphics mapBuffer;
            currentContext = BufferedGraphicsManager.Current;

            // TODO: Use graphcs hardware for this
            mapBuffer = currentContext.Allocate(pictureBox1.CreateGraphics(),
               pictureBox1.DisplayRectangle);

            int xBaseTile = panel1.HorizontalScroll.Value / 16;
            int yBaseTile = panel1.VerticalScroll.Value / 16;

            for (int x = xBaseTile; x < xBaseTile + 80; x++)
            {
                for (int y = yBaseTile; y < yBaseTile + 55; y++)
                {
                    if (x >= 256 || y >= 256) continue;

                    UInt16 imageNum = LayerBlock.GetTextureIDAtLocation(x, y);

                    int posX = x * 16;
                    int posY = y * 16;
                    int srcX = (imageNum % TNameBlock.NumTilesPerRow) * 16;
                    int srcY = (imageNum / TNameBlock.NumTilesPerRow) * 16;

                    mapBuffer.Graphics.DrawImage(TNameBlock.TextureImagesheet, new Rectangle(new Point(posX, posY), new Size(16, 16)), srcX, srcY, 16.0F, 16.0F, GraphicsUnit.Pixel);
                }
            }

            mapBuffer.Render();
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            /*if ((Math.Abs(panel1.HorizontalScroll.Value - lastPositionX) * 16 > 68) || (Math.Abs(panel1.VerticalScroll.Value - lastPositionY)*16 > 68))
            {
                DrawLayer();

                lastPositionX = panel1.HorizontalScroll.Value;
                lastPositionY = panel1.VerticalScroll.Value;
            }*/
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedTile.UpdatePosition(new Point(e.X, e.Y));

            propertyGrid1.SelectedObject = SelectedTile;
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

        public void UpdatePosition(Point p)
        {
            PositionX = p.X;
            PositionY = p.Y;
            Position = p;
        }
    }
}
