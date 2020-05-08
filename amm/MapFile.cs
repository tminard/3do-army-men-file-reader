using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    class MapFile
    {
        private List<IGenericFieldBlock> m_fields;

        //private Dictionary<FractionBlock, List<UnitBlock>> m_armies; // ordered list of fractions and their units. Order: Green, Tan, Blue, Grey

        public MapFile(List<IGenericFieldBlock> fields)
        {
            this.m_fields = fields;

            // TODO: header fields
            // TODO: pull out armies?
        }

        public List<IGenericFieldBlock> GetGenericFields()
        {
            return this.m_fields;
        }
    }
}
