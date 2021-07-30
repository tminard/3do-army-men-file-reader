using AMMEdit.amm.blocks.subfields;
using AMMEdit.PropertyEditors.dialogs;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors.tools
{
    public partial class FlagPainter : UserControl
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Properties
        {
            public enum PaintMode
            {
                OR,
                AND,
                NOOP
            }

            [Category("Flags"), Description("Flag map value"), DisplayName("Flag")]
            [Editor(typeof(FlagEditor.TypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
            public MapFlag MapFlag { get; set; }

            public PaintMode SelectedPaintMode { get; set; }
        }

        public Properties EditorProperties { get; set; }

        public FlagPainter()
        {
            InitializeComponent();
            EditorProperties = new Properties
            {
                MapFlag = new MapFlag(0b0),
                SelectedPaintMode = Properties.PaintMode.NOOP
            };

            propertyGrid1.SelectedObject = EditorProperties;
        }

        private void FlagPainter_Load(object sender, EventArgs e)
        {
        }
    }
}
