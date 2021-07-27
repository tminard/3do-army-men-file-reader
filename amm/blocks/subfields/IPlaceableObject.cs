namespace AMMEdit.amm.blocks.subfields
{
    public interface IPlaceableObject
    {
        int ObjectIndex { get; set; }

        byte[] ToBytes();
        string[] ToFormattedDescription(OLAYObject obj);
    }
}