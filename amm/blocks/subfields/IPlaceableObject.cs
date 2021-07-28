namespace AMMEdit.amm.blocks.subfields
{
    public interface IPlaceableObject
    {
        int ObjectIndex { get; }

        byte[] ToBytes();
        void SetObjectIndex(int index);
        string[] ToFormattedDescription(OLAYObject obj);
    }
}