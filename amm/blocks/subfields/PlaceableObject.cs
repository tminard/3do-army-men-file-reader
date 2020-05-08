using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields
{
    class PlaceableObject
    {
        private Int32 m_index;
        private Int32 m_unknownA;
        private byte m_unknownB;
        private string m_name; // nil terminated when written

        public PlaceableObject(int index, int unknownA, byte unknownB, string name)
        {
            m_index = index;
            m_unknownA = unknownA;
            m_unknownB = unknownB;
            m_name = name;
        }
    }
}
