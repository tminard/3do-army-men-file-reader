using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AMMEdit.axs
{
    public partial class AxsFile
    {
        public class FrameImageData
        {
            private int m_width;
            private int m_height;

            private Bitmap m_sprite_image;
            private Bitmap m_shadow_image;

            private int m_section_id;
            private ushort m_planes;
            private ushort m_bitcount;
            private int m_data_size;

            private ushort m_compressed_width;
            private ushort m_compressed_height;

            private byte[] decodedBitmapData;

            public FrameImageData(Bitmap spriteImage)
            {
                Sprite_image = spriteImage;
            }

            public FrameImageData(BinaryReader r, List<Color> colorPalette)
            {
                Section_id = r.ReadInt32();
                Width = r.ReadInt32();
                Height = r.ReadInt32();
                Planes = r.ReadUInt16();
                Bitcount = r.ReadUInt16();

                r.ReadUInt32();

                Data_size = r.ReadInt32();

                r.BaseStream.Position += 4 * 4;

                Compressed_width = r.ReadUInt16();
                Compressed_height = r.ReadUInt16();

                int padded_width = Compressed_width + (4 - Compressed_width % 4) % 4;

                // ignore the line offset table
                r.BaseStream.Position += (Compressed_height * 2);

                // build the bitmap data - bitmap is indexed
                decodedBitmapData = new byte[padded_width * Compressed_height];

                // sprite
                for (int h = 0; h < Compressed_height; h++)
                {
                    int idx = 0;
                    while (idx < Compressed_width)
                    {
                        // pad runs
                        var p = r.ReadByte();
                        for (int pad = 0; pad < p; pad++)
                        {
                            var bmpPtr = h * padded_width + idx + pad;
                            decodedBitmapData[bmpPtr] = 0; // transparent color in palette
                        }
                        idx += p;

                        // expand color indexes
                        p = r.ReadByte();
                        for (int exp = 0; exp < p; exp++)
                        {
                            var bmpPtr = h * padded_width + idx + exp;
                            decodedBitmapData[bmpPtr] = r.ReadByte();
                        }
                        idx += p;
                    }
                }

                Sprite_image = new Bitmap(padded_width, Convert.ToInt32(Compressed_height));
                for (int x = 0; x < padded_width; x++)
                {
                    for (int y = 0; y < Convert.ToInt32(Compressed_height); y++)
                    {
                        int pos = x + (y * padded_width);
                        Sprite_image.SetPixel(x, y, colorPalette[decodedBitmapData[pos]]);
                    }
                }

                Sprite_image.MakeTransparent(colorPalette[0]);
                Sprite_image.RotateFlip(RotateFlipType.RotateNoneFlipXY);

                // shadow
                // TODO: add
                UInt32 shadowDataSize = r.ReadUInt32();
                if (shadowDataSize > 0)
                {
                    ushort _width = r.ReadUInt16();
                    ushort height = r.ReadUInt16();
                    r.BaseStream.Position += (height * 2); // skip the RLE line offset table
                    r.BaseStream.Position += (shadowDataSize - 4 - (height * 2)); // skip the shadow data for now
                }
            }

            public int Width { get => m_width; private set => m_width = value; }

            public int PaddedWidth { get => m_width + (4 - m_width % 4) % 4; }

            public int Height { get => m_height; private set => m_height = value; }
            public Bitmap Sprite_image { get => m_sprite_image; private set => m_sprite_image = value; }
            public Bitmap Shadow_image { get => m_shadow_image; private set => m_shadow_image = value; }
            public int Section_id { get => m_section_id; private set => m_section_id = value; }
            public ushort Planes { get => m_planes; private set => m_planes = value; }
            public ushort Bitcount { get => m_bitcount; private set => m_bitcount = value; }
            public int Data_size { get => m_data_size; private set => m_data_size = value; }
            public ushort Compressed_width { get => m_compressed_width; private set => m_compressed_width = value; }
            public ushort Compressed_height { get => m_compressed_height; private set => m_compressed_height = value; }
        }
    }
}
