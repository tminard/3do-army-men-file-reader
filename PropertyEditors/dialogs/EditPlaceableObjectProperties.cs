using AMMEdit.amm.blocks.subfields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors.dialogs
{
    public partial class EditPlaceableObjectProperties : Form
    {
        public PlaceableObject PlaceableObject { get; }

        public EditPlaceableObjectProperties(PlaceableObject placeable)
        {
            PlaceableObject = new PlaceableObject(placeable);

            InitializeComponent();
        }

        private void EditPlaceableObjectProperties_Load(object sender, EventArgs e)
        {
            propertyGridValues.SelectedObject = PlaceableObject;
        }
    }
}
