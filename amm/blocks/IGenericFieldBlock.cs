namespace AMMEdit.amm
{
    interface IGenericFieldBlock
    {
        string DisplayFieldName { get; }
        string FieldID { get; }

        byte[] toBytes();

        string[] toFormattedPreview();
    }
}