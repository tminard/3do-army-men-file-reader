using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields
{
    /**
     * Appears to contain metadata for <see cref="OLAYObject"/>.
     */
    public class PlaceableObject : IPlaceableObject
    {
        private Dictionary<string, Int32> m_fields; // field name, and byte length
        private Dictionary<string, List<byte>> m_values;
        private readonly string m_name; // nil terminated when written. Two objects cannot share the same name, UNLESS the name is empty

        public int ObjectIndex
        {
            get
            {
                return GetFieldValue("INDX");
            }
        }

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
            }
            else
            {
                m_name = "";
            }
        }

        public byte[] ToBytes()
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

        public string[] ToFormattedDescription(OLAYObject obj)
        {
            List<string> lines = new List<string> {
                string.Format("Script Name:\t{0}", GetFormattedName())
            };

            lines.Add(string.Empty);
            lines.AddRange(obj.ToFormattedPreview());
            lines.Add(string.Empty);

            m_fields.ToList().ForEach(kv =>
            {
                lines.Add(string.Format("{0}:\t{1}", kv.Key, GetFieldValue(kv.Key)));
            });

            return lines.ToArray();
        }

        private int GetFieldValue(string key)
        {
            byte[] data = m_values[key].ToArray();

            if (data.Length == 1)
            {
                return Convert.ToInt32(data[0]);
            }
            else
            {
                return BinaryPrimitives.ReadInt32LittleEndian(m_values[key].ToArray());
            }
        }

        private string GetFormattedName()
        {
            char[] cstring = m_name.ToArray();

            if (cstring.Length > 0)
            {
                char[] str = new char[m_name.Length - 1];
                Array.Copy(cstring, str, m_name.Length - 1);

                return new string(str);
            }
            else
            {
                return "(unnamed)";
            }
        }
    }
}
