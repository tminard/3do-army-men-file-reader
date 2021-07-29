using AMMEdit.ani;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors
{
    public partial class AnimatedANISpriteView : Form
    {
        public AnimatedANISpriteView(AniFile aniFile)
        {
            InitializeComponent();
            AniFile = aniFile;
        }

        public AniFile AniFile { get; }

        public AniFile.SpriteData SelectedSpriteData { get; set; }

        private void AnimatedANISpriteView_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = AniFile;
            listBox1.DataSource = AniFile.Sprites;
            listBox1.DisplayMember = "Name";
            pictureBox1.BackColor = Color.AntiqueWhite;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = Math.Max(AniFile.Sprites.Count() - 1, 0);
            trackBar1.Value = 0;
            timer1.Stop();
            timer1.Interval = 66; // 15 fps
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();

            SelectedSpriteData = (AniFile.SpriteData)((ListBox)sender).SelectedItem;

            DrawSprite();
        }

        private void DrawSprite()
        {
            propertyGrid1.SelectedObject = SelectedSpriteData;
            pictureBox1.Image = SelectedSpriteData.Image;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowse = new FolderBrowserDialog();

            if (folderBrowse.ShowDialog() == DialogResult.OK)
            {
                string outFolder = System.IO.Path.Combine(folderBrowse.SelectedPath, "SpriteSheet");
                System.IO.Directory.CreateDirectory(outFolder);
                for (int f = 0; f < AniFile.Sprites.Count; f++)
                {
                    string outFile = System.IO.Path.Combine(outFolder, "sprite_" + f + ".png");
                    AniFile.Sprites[f].Image.Save(outFile, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void AnimatedANISpriteView_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int curSpriteIndex = trackBar1.Value;
            int nextIndex = curSpriteIndex + 1;

            if (nextIndex > trackBar1.Maximum)
            {
                nextIndex = trackBar1.Minimum;
            }

            trackBar1.Value = nextIndex;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int selectedSpriteIndex = ((TrackBar)sender).Value;

            SelectedSpriteData = AniFile.Sprites[selectedSpriteIndex];

            DrawSprite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
            }
        }
    }
}
