using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.axs
{
    partial class AxsFile
    {
        private class SpriteHeader
        {
            private readonly UInt32 m_unk_0; // always seems to be "40"
            private readonly UInt32 m_width;
            private readonly UInt32 m_height;
            private readonly ushort m_planes;
            private readonly ushort m_bit_count;
            private readonly Int32 _;
            private readonly UInt32 m_data_size;
            private readonly UInt32 m_unk_1;
            private readonly UInt32 m_unk_2;
            private readonly UInt32 m_unk_3;
            private readonly UInt32 m_unk_4;

            public SpriteHeader(BinaryReader reader)
            {
                m_unk_0 = reader.ReadUInt32();
                m_width = reader.ReadUInt32();
                m_height = reader.ReadUInt32();
                m_planes = reader.ReadUInt16();
                m_bit_count = reader.ReadUInt16();
                _ = reader.ReadInt32();
                m_data_size = reader.ReadUInt32();
                m_unk_1 = reader.ReadUInt32();
                m_unk_2 = reader.ReadUInt32();
                m_unk_3 = reader.ReadUInt32();
                m_unk_4 = reader.ReadUInt32();
            }

            public uint Unk_0 => m_unk_0;

            public uint Width => m_width;

            public uint Height => m_height;

            public ushort Planes => m_planes;

            public ushort Bit_count => m_bit_count;

            public int _1 => _;

            public uint Data_size => m_data_size;

            public uint Unk_1 => m_unk_1;

            public uint Unk_2 => m_unk_2;

            public uint Unk_3 => m_unk_3;

            public uint Unk_4 => m_unk_4;
        }
    }
}
