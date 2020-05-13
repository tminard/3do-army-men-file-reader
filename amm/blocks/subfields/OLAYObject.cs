using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields
{
    public class OLAYObject
    {
        private Int32 m_itemCategory;
        private Int32 m_itemSubType;
        private Int32 m_itemPosX;
        private Int32 m_itemPosY;

        public OLAYObject(BinaryReader r)
        {
            m_itemCategory = r.ReadInt32();
            m_itemSubType = r.ReadInt32();
            m_itemPosX = r.ReadInt32();
            m_itemPosY = r.ReadInt32();
        }

        public string[] ToFormattedPreview()
        {
            return new string[]
            {
                string.Format("Category:\t{0}", m_itemCategory),
                string.Format("Subtype:\t{0}", m_itemSubType),
                string.Format("Position X:\t{0}", m_itemPosX),
                string.Format("Position Y:\t{0}", m_itemPosY),
                string.Format("Position X (tile):\t{0}", m_itemPosX >> 4), // sectors are 6 x 6
                string.Format("Position Y (tile):\t{0}", m_itemPosY >> 4),
            };
        }

        public byte[] ToBytes()
        {
            Span<byte> buff = stackalloc byte[16];

            BinaryPrimitives.WriteInt32LittleEndian(buff, m_itemCategory);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(4), m_itemSubType);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(8), m_itemPosX);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(12), m_itemPosY);

            return buff.ToArray();
        }
    }
}
