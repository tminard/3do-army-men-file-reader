using AMMEdit.amm.blocks.subfields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks
{
    /*
     * A Scenario represents a single mission setup.
     * A map may contain multiple scenario setups.
     * 
     * Each scenario provides configurations for each of the 4 fractions: green, tan, grey and blue.
     * Each fraction contains the layout of all units, their names, and locations.
     * 
     * You can add multiple scenarios to a map file. Multiplayer maps contain only one scenario.
     */
    class SCENBlock : IGenericFieldBlock
    {
        private Int32 m_blockLength;
        private byte[] m_origData;
        private Int32 m_numScenarios;

        private List<Scenario> m_scenarios;

        public SCENBlock(BinaryReader r)
        {
            DisplayFieldName = "Scenario Setup (SCEN)";
            FieldID = Guid.NewGuid().ToString();

            m_blockLength = r.ReadInt32();

            m_origData = r.ReadBytes(m_blockLength);

            // Each scenario contains fraction blocks
            // each fraction block contains unit blocks
            reconstructScenarios();
        }

        public string DisplayFieldName { get; }

        public string FieldID { get; }

        public byte[] toBytes()
        {
            // TODO: reconstuct from underlying data
            return m_origData;
        }

        public string[] toFormattedPreview()
        {
            return new string[] {
                string.Format("Size of block:\t{0}", m_blockLength),
                string.Format("Number scenarios:\t{0}", m_numScenarios)
            };
        }

        private void reconstructScenarios()
        {
            using (MemoryStream ms = new MemoryStream(m_origData))
            {
                using (BinaryReader r = new BinaryReader(ms))
                {
                    m_numScenarios = r.ReadInt32();

                    m_scenarios = new List<Scenario>(m_numScenarios);

                    for (int i = 0; i < m_numScenarios; i++)
                    {
                        m_scenarios.Add(new Scenario(r));
                    }
                }
            }
        }
    }
}
