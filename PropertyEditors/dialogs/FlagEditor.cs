using AMMEdit.amm.blocks.subfields;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors.dialogs
{
    public partial class FlagEditor : Form
    {

        public class TypeEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (provider != null && value != null)
                {
                    byte byteValue;
                    if (value is MapFlag @flag)
                    {
                        byteValue = @flag.Flag;
                    }
                    else if (value is byte @byte)
                    {
                        byteValue = @byte;
                    }
                    else
                    {
                        throw new Exception(String.Format("Object `{0}` is of unsupported type for flag editor", value.GetType()));
                    }

                    var editorForm = new FlagEditor(byteValue);

                    if (editorForm.ShowDialog() == DialogResult.OK)
                    {
                        // update the flagmap
                        if (value is MapFlag @mapFlag)
                        {
                            mapFlag.Flag = editorForm.UpdatedByte;
                        }
                        else if (value is byte)
                        {
                            value = editorForm.UpdatedByte;
                        }
                        else
                        {
                            throw new Exception(String.Format("Object `{0}` is of unsupported type for flag editor", value.GetType()));
                        }
                    }
                }

                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        public byte Byte { get; private set; }
        private BitArray BitArray { get; set; }

        public byte UpdatedByte
        {
            get
            {
                return Convert.ToByte(BitArray.Cast<bool>()
                    .Select((b, i) => Convert.ToUInt16(b) << i)
                    .Aggregate(0b0, (acc, f) => acc |= f));
            }
        }

        public string FormattedByte => String.Format("{0} ({1})", Convert.ToString(UpdatedByte, 2).PadLeft(8, '0'), Convert.ToUInt32(UpdatedByte));

        public FlagEditor(byte initialValue)
        {
            InitializeComponent();
            Byte = initialValue;
            BitArray = new BitArray(new Byte[] { Byte });
            label1.Text = FormattedByte;
        }

        private void FlagEditor_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();

            foreach (bool bitChecked in BitArray.Cast<bool>().Reverse())
            {
                checkedListBox1.Items.Add(bitChecked, bitChecked);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BitArray.Set(7 - e.Index, e.NewValue == CheckState.Checked);
            label1.Text = FormattedByte;
        }
    }
}
