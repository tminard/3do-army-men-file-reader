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
                currentMap = new MapFileLoader(openAMMFileDialog.FileName).read();

                listBox1.DataSource = currentMap.GetGenericFields();
                listBox1.DisplayMember = "DisplayFieldName";
                listBox1.ValueMember = "FieldID";

                List<IGenericFieldBlock> fields = currentMap.GetGenericFields();
                dataBytes = new List<byte>();
                fields.ForEach(x =>
                {
                    dataBytes.AddRange(x.toBytes());
                });

                rawBinaryOutput.Lines = new string[] { "Select a block to preview contents" };
            }
        }

        private string getHexStringFromBytes(byte[] bytes)
        {
            var builder = new StringBuilder();
            
            foreach (var b in bytes)
            {
                builder.AppendFormat(" {0:x} ", b);
            }

            return builder.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;

            IGenericFieldBlock block = (IGenericFieldBlock)lb.SelectedItem;

            rawBinaryOutput.Lines = block.toFormattedPreview();
        }
    }
}
