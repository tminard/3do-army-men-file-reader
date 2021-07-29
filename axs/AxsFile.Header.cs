using System;
using System.IO;

namespace AMMEdit.axs
{
    partial class AxsFile
    {
        private class Header
        {
            private byte m_unknown_field_1;
            private UInt32 m_unknown_field_2;
            private UInt32 m_num_sequences;
            private ushort m_num_sequences_short;
            private byte[] m_mask; // 2-bytes - not entirely sure what this is. Always FF FF.
            private ushort m_unknown_field_3;
            private ushort m_name_length;
            private byte[] m_name;

            public Header(BinaryReader reader)
            {
                m_unknown_field_1 = reader.ReadByte();
                m_unknown_field_2 = reader.ReadUInt32();
                m_num_sequences = reader.ReadUInt32();
                m_num_sequences_short = reader.ReadUInt16();
                m_mask = reader.ReadBytes(2);
                m_unknown_field_3 = reader.ReadUInt16();
                m_name_length = reader.ReadUInt16();
                m_name = reader.ReadBytes(m_name_length);
            }

            public UInt32 GetNumberOfSequences()
            {
                return m_num_sequences;
            }
        }
    }
}
