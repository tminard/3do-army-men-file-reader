using System;

namespace AMMEdit.amm.blocks.subfields
{
    public class MapFlag
    {
        public MapFlag(byte flag)
        {
            Flag = flag;
        }

        public byte Flag { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Convert.ToString(Flag, 2).PadLeft(8, '0'), Convert.ToUInt32(Flag));
        }
    }
}
