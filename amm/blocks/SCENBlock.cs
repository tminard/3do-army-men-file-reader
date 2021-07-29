using AMMEdit.amm.blocks.subfields;
using AMMEdit.PropertyEditors;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AMMEdit.amm.blocks
{
    /*
     * A Scenario represents a single mission setup.
     * A map may contain multiple scenario setups.
     * 
     * Each scenario provides configurations for each of the 4 fractions: green, tan, Gray and blue.
     * Each fraction contains the layout of all units, their names, and locations.
     * 
     * You can add multiple scenarios to a map file. Multiplayer maps contain only one scenario.
     */
    public class SCENBlock : IGenericFieldBlock
    {
        private readonly Int32 m_blockLength;
        private readonly byte[] m_origData;
        private Int32 m_numScenarios;

        private List<Scenario> m_scenarios;

        public SCENBlock(BinaryReader r)
        {
            DisplayFieldName = "Scenario Setup (SCEN)";
            FieldID = Guid.NewGuid().ToString();

            m_blockLength = r.ReadInt32();

            m_origData = r.ReadBytes(m_blockLength);

            ReconstructScenarios();
        }

        public string DisplayFieldName { get; }

        public string FieldID { get; }

        public bool CanEditProperties => true;

        public void ShowPropertyEditor(IWin32Window current)
        {
            ScenarioEditor ed = new ScenarioEditor(m_scenarios);

            ed.ShowDialog(current); // TODO: capture results, and load the returned list
        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[4];

            content.AddRange(new byte[]
            {
                0x53, 0x43, 0x45, 0x4E // SCEN
            });

            BinaryPrimitives.WriteInt32LittleEndian(buff, GetContentByteSize());
            content.AddRange(buff.Slice(0, 4).ToArray());

            BinaryPrimitives.WriteInt32LittleEndian(buff, m_scenarios.Count);
            content.AddRange(buff.Slice(0, 4).ToArray());

            m_scenarios.ForEach(s => content.AddRange(s.ToBytes()));

            content.Add(0x0); // eof marker

            return content.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            List<string> lines = new List<string> {
                string.Format("Size of loaded:\t{0}", m_blockLength),
                string.Format("Size of current:\t{0}", GetContentByteSize()),
                string.Format("Number scenarios:\t{0}", m_numScenarios)
            };

            lines.Add("== Scenarios defined ==");
            m_scenarios.ToList().ForEach(sc =>
            {
                lines.Add("[");
                foreach (var line in sc.ToFormattedPreview())
                {
                    lines.Add(string.Format("\t{0}", line));
                }
                lines.Add("]");
            });

            return lines.ToArray();
        }

        private int GetContentByteSize()
        {
            int numBytes = m_scenarios.Sum(s => s.ToBytes().Length);
            numBytes += 4; // include scenario count
            numBytes += 1; // eof byte - not always present...

            return numBytes;
        }

        private void ReconstructScenarios()
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
