using AMMEdit.amm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit
{
    public partial class Editor : Form
    {
        private MapFile currentMap;
        private List<byte> dataBytes;

        public Editor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openAMMFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentAMMFile.Text = openAMMFileDialog.FileName;

                // TODO: load async. It is fast enough for now.
                currentMap = new MapFileLoader(openAMMFileDialog.FileName).Read();

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
                button2.Enabled = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;

            IGenericFieldBlock block = (IGenericFieldBlock)lb.SelectedItem;

            rawBinaryOutput.Lines = block.ToFormattedPreview();

            buttonEditProps.Enabled = block.CanEditProperties;
        }

        private void button2_Click(object sender, EventArgs e)
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

        }
    }
}
