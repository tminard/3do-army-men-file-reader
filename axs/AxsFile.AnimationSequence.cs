using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.axs
{
    public partial class AxsFile
    {
        public class AnimationSequence
        {
            private UInt32 m_unknown_field_1;
            private byte m_name_length;
            private byte[] m_name;
            private UInt32[] m_headers;
            private ushort m_num_frames;
            private UInt32[] m_frame_indices;

            private List<FrameData> m_offset_data;
            private List<FrameData> m_center_reference_data;
            private List<List<FrameData>> m_other_frame_data;

            private byte[] m_end_indicator; // 0180 in all cases except the last where it is not present

            public AnimationSequence(BinaryReader reader)
            {
                m_unknown_field_1 = reader.ReadUInt32();
                m_name_length = reader.ReadByte();
                Name = reader.ReadBytes(m_name_length);
                m_headers = new uint[7];
                for (var i = 0; i < 7; i++)
                {
                    m_headers[i] = reader.ReadUInt32();
                }
                m_num_frames = reader.ReadUInt16();

                Frame_indices = new uint[m_num_frames];
                for (var i = 0; i < m_num_frames; i++)
                {
                    Frame_indices[i] = reader.ReadUInt32();
                }

                Offset_data = new List<FrameData>(m_num_frames);
                for (var i = 0; i < m_num_frames; i++)
                {
                    Offset_data.Add(new FrameData(reader.ReadUInt32(), reader.ReadUInt32()));
                }

                Center_reference_data = new List<FrameData>(m_num_frames);
                for (var i = 0; i < m_num_frames; i++)
                {
                    Center_reference_data.Add(new FrameData(reader.ReadUInt32(), reader.ReadUInt32()));
                }

                Other_frame_data = new List<List<FrameData>>((int)m_headers[5]);
                for (var i = 0; i < m_headers[5]; i++)
                {
                    var other_data = new List<FrameData>(m_num_frames);
                    for (var j = 0; j < m_num_frames; j++)
                    {
                        other_data.Add(new FrameData(reader.ReadUInt32(), reader.ReadUInt32()));
                    }
                    Other_frame_data.Add(other_data);
                }

                m_end_indicator = reader.ReadBytes(2);
            }

            public ushort Num_frames { get => m_num_frames; set => m_num_frames = value; }
            public uint[] Frame_indices { get => m_frame_indices; set => m_frame_indices = value; }
            public byte[] Name { get => m_name; private set => m_name = value; }

            public string AnimationName { get => Encoding.Default.GetString(m_name); }
            public List<FrameData> Offset_data { get => m_offset_data; private set => m_offset_data = value; }
            public List<FrameData> Center_reference_data { get => m_center_reference_data; private set => m_center_reference_data = value; }
            public List<List<FrameData>> Other_frame_data { get => m_other_frame_data; private set => m_other_frame_data = value; }
        }
    }
}
