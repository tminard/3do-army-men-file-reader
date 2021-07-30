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
            return Convert.ToString(Flag, 2).PadLeft(8, '0');
        }
    }
}
