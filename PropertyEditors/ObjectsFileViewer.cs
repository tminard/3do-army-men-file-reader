using AMMEdit.objects;
using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors
{
    public partial class ObjectsFileViewer : Form
    {
        public ObjectsFileViewer(DatFile dataFile)
        {
            InitializeComponent();
            DataFile = dataFile;
        }

        public DatFile DataFile { get; }

        public AMObject SelectedObject { get; }

        private void ObjectsFileViewer_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = DataFile.Objects;
            listBox1.DisplayMember = "LabelText";
            propertyGrid1.SelectedObject = SelectedObject;
            exportSelectedToolStripMenuItem.Enabled = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ((ListBox)sender).SelectedItem;
            spriteViewBox.Image = ((AMObject)((ListBox)sender).SelectedItem).SpriteImage;
        }

        private void exportSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    AMObject selectedObject = (AMObject)listBox1.SelectedItem;
                    selectedObject.SpriteImage.Save(saveFileDialog1.FileName);

                    string objectPropertiesAsJson = new JavaScriptSerializer().Serialize(selectedObject);
                    File.WriteAllText(saveFileDialog1.FileName + ".json", objectPropertiesAsJson);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
