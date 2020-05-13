namespace AMMEdit.amm.blocks.subfields
{
    interface IPlaceableObject
    {
        int ObjectIndex { get; }

        byte[] ToBytes();
        string[] ToFormattedDescription(OLAYObject obj);
    }
}