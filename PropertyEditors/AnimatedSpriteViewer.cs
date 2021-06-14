using AMMEdit.axs;
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
    public partial class AnimatedSpriteViewer : Form
    {
        public AnimatedSpriteViewer(AxsFile axsFile)
        {
            InitializeComponent();
            AxsFile = axsFile;
        }

        public AxsFile AxsFile { get; }

        private void AnimatedSpriteViewer_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = AxsFile.Animation_sequences;
            listBox1.DisplayMember = "AnimationName";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ((ListBox)sender).SelectedItem;
        }
    }
}
