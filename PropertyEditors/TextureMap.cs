using AMMEdit.amm.blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors
{
    public partial class TextureMap : Form
    {
        public TNAMBlock TNameBlock { get; private set; }

        public TextureMap(TNAMBlock textureBlock)
        {
            TNameBlock = textureBlock;

            InitializeComponent();
        }

        private void TextureMap_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = (Bitmap)TNameBlock.TextureImagesheet.Clone();
        }
    }
}
