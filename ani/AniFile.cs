using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AMMEdit.ani
{
    public partial class AniFile
    {
        private readonly int m_num_sprites;
        private List<Color> m_palette;
        private List<SpriteData> m_sprites;

        public AniFile(BinaryReader reader)
        {
            m_palette = new List<Color>(256);

            for (int c = 0; c < 256; c++)
            {
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();
                byte a = reader.ReadByte();

                m_palette.Add(
                    Color.FromArgb(255, r, g, b)
               );
            }

            m_palette[0] = Color.FromArgb(255, 255, 255, 255);
            m_palette[10] = Color.FromArgb(0, 0, 0, 0);

            m_num_sprites = reader.ReadInt32();

            // read sprite data
            Sprites = new List<SpriteData>(m_num_sprites);

            for (int i = 0; i < m_num_sprites; i++)
            {
                Sprites.Add(new SpriteData(i + 1, reader, m_palette));
            }
        }

        public List<SpriteData> Sprites { get => m_sprites; private set => m_sprites = value; }

        public class SpriteData
        {
            private readonly ushort m_width;
            private readonly ushort m_height;
            private readonly ushort m_a;
            private readonly ushort m_b;
            private readonly short s1;
            private readonly short s2;

            private readonly int m_number;

            private readonly SpriteImageData m_sprite_image_data;

            public Point A { get => new Point(m_a, m_b); }

            public Point B { get => new Point(s1, s2); }

            public String Name { get => "Sprite " + m_number; }

            public Bitmap Image { get => m_sprite_image_data.Sprite_image; }

            public SpriteData(int number, BinaryReader reader, List<Color> colorPalette)
            {
                m_number = number;
                m_width = reader.ReadUInt16();
                m_height = reader.ReadUInt16();
                m_a = reader.ReadUInt16();
                m_b = reader.ReadUInt16();
                s1 = reader.ReadInt16();
                s2 = reader.ReadInt16();

                m_sprite_image_data = new SpriteImageData(reader, colorPalette);
            }

            public class SpriteImageData
            {
                private readonly uint m_frame_size;
                private readonly ushort m_width;
                private readonly ushort m_height;
                private readonly Bitmap m_sprite_image;
                private readonly byte[] m_decoded_bitmap_data;

                public SpriteImageData(BinaryReader reader, List<Color> colorPalette)
                {
                    // read both sprite and shadow, if present
                    m_frame_size = reader.ReadUInt32();

                    if (m_frame_size > 0)
                    {
                        m_width = reader.ReadUInt16();
                        m_height = reader.ReadUInt16();

                        // skip line offset table
                        reader.BaseStream.Position += (m_height * 2);

                        m_decoded_bitmap_data = new byte[m_width * m_height];

                        // sprite
                        for (int h = 0; h < m_height; h++)
                        {
                            // AM2+ uses something closer to "modern" RLE via use of opcodes
                            int idx = 0;
                            while (idx < m_width)
                            {
                                // pad runs
                                var p = reader.ReadByte();
                                for (int pad = 0; pad < p; pad++)
                                {
                                    var bmpPtr = h * m_width + idx + pad;
                                    m_decoded_bitmap_data[bmpPtr] = 10; // transparent color in palette
                                }
                                idx += p;

                                // expand color indexes
                                p = reader.ReadByte();
                                for (int exp = 0; exp < p; exp++)
                                {
                                    var bmpPtr = h * m_width + idx + exp;
                                    m_decoded_bitmap_data[bmpPtr] = reader.ReadByte();
                                }
                                idx += p;
                            }
                        }

                        m_sprite_image = new Bitmap(Convert.ToInt32(m_width), Convert.ToInt32(m_height));
                        for (int x = 0; x < m_width; x++)
                        {
                            for (int y = 0; y < Convert.ToInt32(m_height); y++)
                            {
                                int pos = x + (y * m_width);
                                m_sprite_image.SetPixel(x, y, colorPalette[m_decoded_bitmap_data[pos]]);
                            }
                        }

                        // m_sprite_image.MakeTransparent(colorPalette[0]);
                        m_sprite_image.RotateFlip(RotateFlipType.RotateNoneFlipXY);

                        // shadow
                        UInt32 shadowDataSize = reader.ReadUInt32();
                        if (shadowDataSize > 0)
                        {
                            ushort _width = reader.ReadUInt16();
                            ushort height = reader.ReadUInt16();
                            reader.BaseStream.Position += (height * 2); // skip the RLE line offset table
                            reader.BaseStream.Position += (shadowDataSize - 4 - (height * 2)); // skip the shadow data for now
                        }
                    }
                }

                public Bitmap Sprite_image => m_sprite_image;
            }
        }
    }
}
