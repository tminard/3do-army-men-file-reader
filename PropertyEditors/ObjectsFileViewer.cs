using AMMEdit.objects;
using System;
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
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ((ListBox)sender).SelectedItem;
            spriteViewBox.Image = ((AMObject)((ListBox)sender).SelectedItem).SpriteImage;
        }
    }
}
