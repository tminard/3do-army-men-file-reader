using System;
using System.Buffers.Binary;
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
        private string m_name; // nil terminated when written. Two objects cannot share the same name, UNLESS the name is empty
        private bool m_includeUnknownB;

        public PlaceableObject(int index, int unknownA, byte unknownB, string name, bool includeUnknownB)
        {
            m_index = index;
            m_unknownA = unknownA;
            m_unknownB = unknownB;
            m_name = name;
            m_includeUnknownB = includeUnknownB;
        }

        public byte[] toBytes()
        {
            int size = 8 + (m_includeUnknownB ? 1 : 0) + m_name.Length; // strings are already nil terminated
            List<byte> content = new List<byte>(size);
            Span<byte> buff = stackalloc byte[1024];

            // index
            BinaryPrimitives.WriteInt32LittleEndian(buff, Convert.ToInt32(m_index));
            content.AddRange(buff.Slice(0, 4).ToArray());

            // unknown
            BinaryPrimitives.WriteInt32LittleEndian(buff, Convert.ToInt32(m_unknownA));
            content.AddRange(buff.Slice(0, 4).ToArray());

            // optional byte
            if (m_includeUnknownB)
            {
                content.Add(m_unknownB);
            }

            // name length
            BinaryPrimitives.WriteInt32LittleEndian(buff, Convert.ToInt32(m_name.Length));
            content.AddRange(buff.Slice(0, 4).ToArray());

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
                string.Format("Name:\t{0}", getFormattedName()),
                string.Format("Index:\t{0}", m_index),
                string.Format("Unknown A:\t{0}", m_unknownA),
                string.Format("Unknown B:\t{0}", m_unknownB),
                string.Format("Include extra byte?:\t{0}", m_includeUnknownB)
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
