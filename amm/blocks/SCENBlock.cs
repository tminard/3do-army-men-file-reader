using AMMEdit.amm.blocks.subfields;
using System;
using System.Buffers.Binary;
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

            reconstructScenarios();
        }

        public string DisplayFieldName { get; }

        public string FieldID { get; }

        public byte[] toBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[4];

            BinaryPrimitives.WriteInt32LittleEndian(buff, getContentByteSize());

            BinaryPrimitives.WriteInt32LittleEndian(buff, m_scenarios.Count);
            content.AddRange(buff.Slice(0, 4).ToArray());

            m_scenarios.ForEach(s => content.AddRange(s.toBytes()));

            content.Add(0x0); // eof marker

            return content.ToArray();
        }

        public string[] toFormattedPreview()
        {
            List<string> lines = new List<string> {
                string.Format("Size of loaded:\t{0}", m_blockLength),
                string.Format("Size of current:\t{0}", getContentByteSize()),
                string.Format("Number scenarios:\t{0}", m_numScenarios)
            };

            lines.Add("== Scenarios defined ==");
            m_scenarios.ToList().ForEach(sc =>
            {
                lines.Add("[");
                foreach (var line in sc.toFormattedPreview())
                {
                    lines.Add(string.Format("\t{0}", line));
                }
                lines.Add("]");
            });

            return lines.ToArray();
        }

        private int getContentByteSize()
        {
            int numBytes = m_scenarios.Sum(s => s.toBytes().Length);
            numBytes += 4; // include scenario count
            numBytes += 1; // eof byte - not always present...

            return numBytes;
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
