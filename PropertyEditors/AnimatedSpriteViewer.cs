using AMMEdit.axs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        private bool m_playing = false;

        private Bitmap renderedPreview = new Bitmap(256, 256);

        public AxsFile AxsFile { get; }
        public AxsFile.Animation Selected_animation { get => m_selected_animation; private set => m_selected_animation = value; }

        private AxsFile.Animation m_selected_animation;

        private void AnimatedSpriteViewer_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = AxsFile.Animations;
            listBox1.DisplayMember = "Name";
            timer1.Interval = 66; // 15 FPS
            timer1.Stop();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selected_animation = (AxsFile.Animation)((ListBox)sender).SelectedItem;
            propertyGrid1.SelectedObject = Selected_animation;
            trackBar1.Minimum = 0;
            trackBar1.Value = 0;
            trackBar1.Maximum = Selected_animation.Frames.Count - 1;

            DrawAnimationFrame();
        }

        private void DrawAnimationFrame()
        {
            
            int frameIndex = trackBar1.Value;
            Bitmap previewImage = (Bitmap)Selected_animation.Images[trackBar1.Value].Clone();

            previewImage.RotateFlip(RotateFlipType.RotateNoneFlipXY); // return to original orientation

            BufferedGraphicsContext currentContext;
            BufferedGraphics buffer;
            currentContext = BufferedGraphicsManager.Current;

            buffer = currentContext.Allocate(Graphics.FromImage(renderedPreview), new Rectangle(0, 0, 256, 256));
            buffer.Graphics.Clear(Color.AntiqueWhite);

            // start with a default centered image
            AxsFile.FrameData centerPointData = Selected_animation.AnimationData.Center_reference_data[frameIndex];
            AxsFile.FrameData offsetPointData = Selected_animation.AnimationData.Offset_data[frameIndex];
            Point centerPoint = new Point(128, 128);

            if (centerPointData != null)
            {
                centerPoint.X = 128 - Convert.ToInt32(centerPointData.X);
                centerPoint.Y = 128 - Convert.ToInt32(centerPointData.Y);
            }

            // draw sprite
            buffer.Graphics.DrawImage(previewImage, new Rectangle(
                centerPoint,
                new Size(previewImage.Width, previewImage.Height)
            ), 0, 0, previewImage.Width, previewImage.Height, GraphicsUnit.Pixel);

            // draw annotations
            SolidBrush solidBrush = new SolidBrush(Color.Red);

            // this seems to be the anchor pixel for the image
            if (Selected_animation.ShowOffsetPoint == true)
            {
                solidBrush.Color = Color.Blue;
                buffer.Graphics.FillEllipse(solidBrush, previewImage.Width - Convert.ToInt32(offsetPointData.X) + 128, previewImage.Height - Convert.ToInt32(offsetPointData.Y) + 128, 4, 4);
            }

            if (Selected_animation.ShowCenterPoint == true)
            {
                if (centerPointData.X < 255 && centerPointData.Y < 255)
                {
                    Point p = new Point(
                        Convert.ToInt32(centerPointData.X) + 128,
                        previewImage.Height - Convert.ToInt32(centerPointData.Y) + 128
                    );

                    p = centerPoint;

                    solidBrush.Color = Color.Red;
                    buffer.Graphics.FillEllipse(solidBrush, p.X, p.Y, 2, 2);
                }
            }

            int otherIndx = 0;
            Selected_animation.AnimationData.Other_frame_data.ForEach(other => {
                if (other.Count > frameIndex)
                {
                    AxsFile.FrameData otherData = other[frameIndex];
                    if (otherData.X > 256 || otherData.Y > 256)
                    {
                        return;
                    }

                    solidBrush.Color = Color.FromArgb(255, otherIndx, otherIndx, otherIndx);
                    buffer.Graphics.FillEllipse(solidBrush, Convert.ToInt32(otherData.X) + 128, previewImage.Height - Convert.ToInt32(otherData.Y) + 128, 4, 4);
                }
                otherIndx += 64;
            });

            // done

            buffer.Render();
            buffer.Dispose();

            renderedPreview.RotateFlip(RotateFlipType.RotateNoneFlipXY);

            spriteViewBox.Image = (Bitmap)renderedPreview.Clone();

            spriteViewBox.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            DrawAnimationFrame();
        }

        private void exportSpritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowse = new FolderBrowserDialog();

            if (folderBrowse.ShowDialog() == DialogResult.OK)
            {
                string outFolder = System.IO.Path.Combine(folderBrowse.SelectedPath, "SpriteSheet");
                System.IO.Directory.CreateDirectory(outFolder);
                for (int f = 0; f < AxsFile.Frame_images.Count; f++)
                {
                    string outFile = System.IO.Path.Combine(outFolder, "sprite_" + f + ".png");
                    AxsFile.Frame_images[f].Sprite_image.Save(outFile, System.Drawing.Imaging.ImageFormat.Png);
                }
                
                for (int a = 0; a < AxsFile.Animations.Count; a++)
                {
                    AxsFile.Animation ani = AxsFile.Animations[a];

                    outFolder = System.IO.Path.Combine(folderBrowse.SelectedPath, a + " " + ani.Name.Trim());
                    System.IO.Directory.CreateDirectory(outFolder);

                    for (int i = 0; i < ani.Images.Count; i++)
                    {
                        int frameIndex = Convert.ToInt32(ani.AnimationData.Frame_indices[i]);

                        string outFile = System.IO.Path.Combine(outFolder, "frame_" + i + "_indx_" + frameIndex + ".png");
                        try
                        {
                            ani.Images[i].Save(outFile, System.Drawing.Imaging.ImageFormat.Png);
                        } catch (System.Runtime.InteropServices.ExternalException ex)
                        {
                            Debug.WriteLine("Failed to save " + outFile + ": " + ex.Message);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_playing = !m_playing;

            if (m_playing)
            {
                timer1.Start();
            } else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int nextValue = trackBar1.Value + 1;

            if (nextValue > trackBar1.Maximum)
            {
                nextValue = trackBar1.Minimum;
            }

            trackBar1.Value = nextValue;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            DrawAnimationFrame();
        }

        private void AnimatedSpriteViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
