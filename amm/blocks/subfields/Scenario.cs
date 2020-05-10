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
        private readonly string m_nameCstring;

        private readonly List<Fraction> m_fractions;

        public enum FractionOrder : int
        {
            Green = 0,
            Tan = 1,
            Blue = 2,
            Grey = 3
        }

        public Scenario(BinaryReader r)
        {
            m_nameCstring = new string(r.ReadChars(16));

            // order is Green > Tan > Blue > Grey. Min 1, max 4 present in file.
            // read until EOF, or until EOB marker is read where a fraction was expected.
            m_fractions = new List<Fraction>(4);

            for (int f = 0; f < 4; f++)
            {
                // read fraction and unit block
                this.m_fractions.Add(new Fraction((FractionOrder)f, r));
            }

        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();

            content.AddRange(ASCIIEncoding.ASCII.GetBytes(m_nameCstring));

            m_fractions.ForEach(f => content.AddRange(f.toBytes()));

            return content.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            return new string[] {
                string.Format("Scenario Name:\t{0}", m_nameCstring.Replace("\0", "")),
                string.Format("Number of fractions defined:\t{0}", m_fractions.Count)
            };
        }
    }
}
