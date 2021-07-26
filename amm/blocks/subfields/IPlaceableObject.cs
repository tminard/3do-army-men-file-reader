namespace AMMEdit.amm.blocks.subfields
{
    public interface IPlaceableObject
    {
        int ObjectIndex { get; }

        byte[] ToBytes();
        string[] ToFormattedDescription(OLAYObject obj);
    }
}