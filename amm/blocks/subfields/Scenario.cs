using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields
{
    public class Scenario : INotifyPropertyChanged
    {
        private readonly string m_nameCstring;

        public List<Fraction> m_fractions { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                name = value;

                // TODO: set the cstring

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
 

        public string FieldID { get; }

        public enum FractionOrder : int
        {
            Green = 0,
            Tan = 1,
            Blue = 2,
            Gray = 3
        }

        public Scenario(BinaryReader r)
        {
            m_nameCstring = new string(r.ReadChars(16));

            // order is Green > Tan > Blue > Gray. Min 1, max 4 present in file.
            // read until EOF, or until EOB marker is read where a fraction was expected.
            m_fractions = new List<Fraction>(4);

            for (int f = 0; f < 4; f++)
            {
                // read fraction and unit block
                this.m_fractions.Add(new Fraction((FractionOrder)f, r));
            }

            this.name = (m_nameCstring.Replace("\0", ""));
            FieldID = Guid.NewGuid().ToString();
        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();

            content.AddRange(ASCIIEncoding.ASCII.GetBytes(m_nameCstring));

            m_fractions.ForEach(f => content.AddRange(f.ToBytes()));

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
