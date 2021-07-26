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
        public Int32 m_itemCategory { get; private set; }
        public Int32 m_itemSubType { get; private set; }
        public Int32 m_itemPosX { get; private set; }
        public Int32 m_itemPosY { get; private set; }

        public OLAYObject(BinaryReader r)
        {
            m_itemCategory = r.ReadInt32();
            m_itemSubType = r.ReadInt32();
            m_itemPosX = r.ReadInt32();
            m_itemPosY = r.ReadInt32();
        }

        public OLAYObject(int itemCategory, int itemSubType, int itemPosX, int itemPosY)
        {
            m_itemCategory = itemCategory;
            m_itemSubType = itemSubType;
            m_itemPosX = itemPosX;
            m_itemPosY = itemPosY;
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
