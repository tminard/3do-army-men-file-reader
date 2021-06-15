using AMMEdit.amm;
using AMMEdit.ani;
using AMMEdit.axs;
using AMMEdit.objects;
using AMMEdit.objects.loaders;
using AMMEdit.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit
{
    public partial class Editor : Form
    {
        private MapFile currentMap;
        private DatFile currentDataFile;
        private List<byte> dataBytes;
        private Thread ioThread;
        private TaskScheduler scheduler;

        public Editor()
        {
            InitializeComponent();
        }

        private void openAMMFile()
        {
            if (openAMMFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentAMMFile.Text = openAMMFileDialog.FileName;

                // TODO: load async. It is fast enough for now.
                currentMap = new MapFileLoader(openAMMFileDialog.FileName).Read(currentDataFile);

                listBox1.DataSource = currentMap.GetGenericFields();
                listBox1.DisplayMember = "DisplayFieldName";
                listBox1.ValueMember = "FieldID";

                List<IGenericFieldBlock> fields = currentMap.GetGenericFields();
                dataBytes = new List<byte>();
                fields.ForEach(x =>
                {
                    dataBytes.AddRange(x.ToBytes());
                });

                rawBinaryOutput.Lines = new string[] { "Select a block to preview contents" };
                saveAsToolStripMenuItem.Enabled = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;

            IGenericFieldBlock block = (IGenericFieldBlock)lb.SelectedItem;

            rawBinaryOutput.Lines = block.ToFormattedPreview();

            buttonEditProps.Enabled = block.CanEditProperties;
        }

        private void saveAMMAs()
        {
            if (saveAMMFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentMap.SaveAs(saveAMMFileDialog.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IGenericFieldBlock block = (IGenericFieldBlock)listBox1.SelectedItem;

            block.ShowPropertyEditor(this);
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (openDATFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (ioThread != null && ioThread.IsAlive)
                {
                    statusLabel.Text = "Terminating existing operation";
                    ioThread.Abort();
                    ioThread = null;
                }

                statusLabel.Text = "Loading `" + openDATFileDialog.FileName + "`";
                progressBar1.Value = 0;
                progressBar1.Visible = true;

                ioThread = new Thread(loadDataFile);
                ioThread.Priority = ThreadPriority.Highest;
                ioThread.IsBackground = true;
                ioThread.Start();

            } else
            {
                statusLabel.Text = "Idle.";
            }
        }

        private void loadDataFile()
        {
            currentDataFile = new DatFileLoader(openDATFileDialog.FileName, progressBar1).Read();

            new Task(() =>
            {
                statusLabel.Text = "Done";
                progressBar1.Value = 100;
                progressBar1.Visible = false;
            }).Start(scheduler);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openAMMFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAMMAs();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDataFile != null)
            {
                ObjectsFileViewer fv = new ObjectsFileViewer(currentDataFile);

                fv.Show(this);
            }
        }

        private void loadAM1AXSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openAXSFileDialog.ShowDialog() == DialogResult.OK)
            {
                AxsFile axsFile;

                using (FileStream fs = new FileStream(openAXSFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        axsFile = new AxsFile(r);
                    }
                }
                 
                if (axsFile != null)
                {
                    AnimatedSpriteViewer sv = new AnimatedSpriteViewer(axsFile);
                    sv.Show(this);
                }
            }
        }

        private void loadAM2ANIFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openANIDialog1.ShowDialog() == DialogResult.OK)
            {
                AniFile aniFile;

                using (FileStream fs = new FileStream(openANIDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        aniFile = new AniFile(r);
                    }
                }

                if (aniFile != null)
                {
                    AnimatedANISpriteView sv = new AnimatedANISpriteView(aniFile);
                    sv.Show(this);
                }
            }
        }
    }
}
