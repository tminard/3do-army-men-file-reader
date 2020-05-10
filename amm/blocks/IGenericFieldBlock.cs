using System.Windows.Forms;

namespace AMMEdit.amm
{
    interface IGenericFieldBlock
    {
        string DisplayFieldName { get; }
        string FieldID { get; }

        byte[] ToBytes();

        string[] ToFormattedPreview();

        bool CanEditProperties { get; }

        void ShowPropertyEditor(IWin32Window current);
    }
}