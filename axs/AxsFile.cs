using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AMMEdit.axs
{
    public partial class AxsFile
    {
        private readonly Header m_header;
        private readonly List<AnimationSequence> m_animation_sequences = new List<AnimationSequence>();

        private readonly byte[] m_axs_block_header;
        private readonly UInt32 m_num_sprites;
        private readonly byte[] m_bmp_header;
        private List<FrameImageData> m_frame_images;
        private List<Color> m_palette;
        private List<Animation> m_animations = new List<Animation>();


        public AxsFile(BinaryReader reader)
        {
            m_header = new Header(reader);

            for (var i = 0; i < m_header.GetNumberOfSequences(); i++)
            {
                m_animation_sequences.Add(new AnimationSequence(reader));
            }

            m_axs_block_header = reader.ReadBytes(6);
            m_num_sprites = reader.ReadUInt32();
            m_bmp_header = reader.ReadBytes(54); // nearly valid but missing size fields
            m_palette = new List<Color>(256);

            for (int c = 0; c < 256; c++)
            {
                byte bC = reader.ReadByte();
                byte gC = reader.ReadByte();
                byte rC = reader.ReadByte();
                _ = reader.ReadByte();

                // each element is an int packing a BGRa color
                m_palette.Add(Color.FromArgb(rC, gC, bC));
            }

            m_frame_images = new List<FrameImageData>((int)m_num_sprites);
            for (int s = 0; s < m_num_sprites; s++)
            {
                m_frame_images.Add(new FrameImageData(reader, m_palette));
            }

            for (int a = 0; a < m_header.GetNumberOfSequences(); a++)
            {
                Animations.Add(new Animation(Animation_sequences[a], m_frame_images));
            }
        }

        public List<AnimationSequence> Animation_sequences => m_animation_sequences;

        public List<FrameImageData> Frame_images => m_frame_images;

        public List<Animation> Animations { get => m_animations; set => m_animations = value; }
    }
}
