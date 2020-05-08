using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields
{
    class Scenario
    {
        private const byte EOB_MARKER = 0x8; // used by AM to indicate end of fractions for given scenario
        private string m_nameCstring;
        private Int32 m_numFractions;

        private List<FractionBlock> m_fractions;

        public Scenario(BinaryReader r)
        {
            m_nameCstring = new string(r.ReadChars(16));

            // order is Green > Tan > Blue > Grey. Min 1, max 4 present in file.
            // read until EOF, or until EOB marker is read where a fraction was expected.
            byte[] fractionData = r.ReadBytes(84);
        }
    }
}
