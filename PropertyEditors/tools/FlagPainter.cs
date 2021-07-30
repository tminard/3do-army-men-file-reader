using AMMEdit.amm.blocks.subfields;
using AMMEdit.PropertyEditors.dialogs;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

            [Category("Tool"), Description("Size of the brush"), DisplayName("Brush size")]
            [Range(1, 5, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
            public int BrushSize { get; set; }

            public PaintMode SelectedPaintMode { get; set; }
        }

        public Properties EditorProperties { get; set; }

        public FlagPainter()
        {
            InitializeComponent();
            EditorProperties = new Properties
            {
                MapFlag = new MapFlag(0b0),
                SelectedPaintMode = Properties.PaintMode.NOOP,
                BrushSize = 1
            };

            propertyGrid1.SelectedObject = EditorProperties;
        }

        private void FlagPainter_Load(object sender, EventArgs e)
        {
        }
    }
}
