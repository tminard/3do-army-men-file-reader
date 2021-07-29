using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AMMEdit.amm
{
    class MapFile
    {
        private List<IGenericFieldBlock> m_fields;

        public MapFile(List<IGenericFieldBlock> fields)
        {
            this.m_fields = fields;
        }

        public List<IGenericFieldBlock> GetGenericFields()
        {
            return this.m_fields;
        }

        public void SaveAs(string filename)
        {
            List<byte> orderedBytes = new List<byte>(); // TODO: make this a memory stream?

            m_fields.ForEach(f => orderedBytes.AddRange(f.ToBytes()));

            uint contentSize = Convert.ToUInt32(orderedBytes.Count());

            List<byte> content = new List<byte>();
            content.AddRange(new byte[] {
                0x46, 0x4F, 0x52, 0x4D
            });
            content.AddRange(UInt32ToBytes(contentSize));
            content.AddRange(new byte[] {
                0x4D, 0x41, 0x50, 0x20
            });
            content.AddRange(orderedBytes);

            File.WriteAllBytes(filename, content.ToArray());
        }

        private byte[] UInt32ToBytes(UInt32 val)
        {
            Span<byte> buff = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(buff, val);

            return buff.Slice(0, 4).ToArray();
        }
    }
}
