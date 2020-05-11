using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields
{
    class PlaceableObject
    {
        private Dictionary<string, Int32> m_fields; // field name, and byte length
        private Dictionary<string, List<byte>> m_values;
        private string m_name; // nil terminated when written. Two objects cannot share the same name, UNLESS the name is empty

        public PlaceableObject(Dictionary<string, Int32> fields, BinaryReader r)
        {
            m_fields = fields;
            m_values = new Dictionary<string, List<byte>>();

            m_fields.ToList().ForEach(kv =>
            {
                List<byte> d;
                switch (kv.Value)
                {
                    case 1:
                        d = new List<byte>(1)
                        {
                            r.ReadByte()
                        };

                        m_values.Add(kv.Key, d);
                        break;
                    default:
                        d = new List<byte>(kv.Value);
                        d.AddRange(r.ReadBytes(kv.Value));

                        m_values.Add(kv.Key, d);
                        break;
                }
            });

            int nameLen = BinaryPrimitives.ReadInt32LittleEndian(m_values["SCRI"].ToArray());
            if (nameLen > 0)
            {
                m_name = new string(r.ReadChars(nameLen));
            } else
            {
                m_name = "";
            }
        }

        public byte[] toBytes()
        {
            int contentSize = m_values.Sum(kv => kv.Value.Count);
            int size = contentSize + m_name.Length; // strings are already nil terminated
            List<byte> content = new List<byte>(size);

            m_values.ToList().ForEach(kv => content.AddRange(kv.Value));
            
            // nil terminated name (ASCII)
            if (m_name.Length > 0)
            {
                byte[] b = UTF8Encoding.UTF8.GetBytes(m_name);

                content.AddRange(b);
            }

            return content.ToArray();
        }

        public string[] toFormattedDescription()
        {
            return new string[] {
                string.Format("Name:\t{0}", getFormattedName())
                /*string.Format("Index:\t{0}", m_index),
                string.Format("Unknown A:\t{0}", m_unknownA),
                string.Format("Unknown B:\t{0}", m_unknownB),
                string.Format("Include extra byte?:\t{0}", m_includeUnknownB)*/
            };
        }

        private string getFormattedName()
        {
            char[] cstring = m_name.ToArray();

            if (cstring.Length > 0)
            {
                char[] str = new char[m_name.Length - 1];
                Array.Copy(cstring, str, m_name.Length - 1);

                return new string(str);
            } else
            {
                return "(empty)";
            }
        }
    }
}
