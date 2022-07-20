using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AMMEdit.objects.loaders
{
    public class DatFileEntry
    {
        public delegate void ReportLoadedBytesProgressDelegate(Stream s);

        public AMObject ObjectEntry { get; private set; }

        public Bitmap Sprite { get; private set; }

        public DatFileEntry(List<Color> colorPalette, UInt32 decodedTypeKey, UInt32 decodedInstance, BinaryReader br, ReportLoadedBytesProgressDelegate ReportLoadedBytes)
        {
            // starts reading AFTER jump to data section
            var width = br.ReadUInt32();
            var height = br.ReadUInt32();
            var rleFlag = br.ReadUInt32();
            br.BaseStream.Position += (4 * 3); // skip unknown fields

            var rleEncodingMode = rleFlag & 0b1111;
            var rleEncodingModeSecondPass = rleFlag & 0b11110000;

            // read from known encodings
            byte[] decodedBitmapData;
            var widthPadded = width;

            if (rleEncodingMode != 0 || rleEncodingModeSecondPass == 16)
            {
                var rleWidth = br.ReadInt16();
                var rleHeight = br.ReadInt16();
                widthPadded = Convert.ToUInt32(rleWidth + (4 - rleWidth % 4) % 4);

                // ignore the line offset table
                var lookupTableLength = (rleEncodingMode == 8 || rleEncodingMode == 0) ? height * 2 : height * 4;
                br.BaseStream.Position += lookupTableLength;

                // build the bitmap data - bitmap is indexed
                decodedBitmapData = new byte[widthPadded * height];
                var idx = 0;
                for (int h = 0; h < height; h++)
                {
                    ReportLoadedBytes(br.BaseStream);
                    idx = 0;
                    while (idx < width)
                    {
                        // pad runs
                        var p = br.ReadByte();
                        for (int pad = 0; pad < p; pad++)
                        {
                            var bmpPtr = h * widthPadded + idx + pad;
                            decodedBitmapData[bmpPtr] = 0; // transparent color in palette
                        }
                        idx += p;

                        // expand color indexes
                        p = br.ReadByte();
                        for (int exp = 0; exp < p; exp++)
                        {
                            var bmpPtr = h * widthPadded + idx + exp;
                            decodedBitmapData[bmpPtr] = br.ReadByte();
                        }
                        idx += p;
                    }
                }
            }
            else if (rleEncodingMode == 0 && rleEncodingModeSecondPass != 32)
            {
                // these tend to be UI images
                widthPadded = width + (4 - width % 4) % 4;
                decodedBitmapData = new byte[widthPadded * height];
                decodedBitmapData = br.ReadBytes(Convert.ToInt32(widthPadded * height));
            }
            else
            {
                // TODO: skip cannot read this - use `?` image in renderer but keep data for export
                Debug.WriteLine("Could not load T" + decodedTypeKey + " i" + decodedInstance + ": Unrecognized RLE mode.");

                return;
            }

            // create image
            Bitmap spriteBitmap = new Bitmap(Convert.ToInt32(widthPadded), Convert.ToInt32(height), PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, spriteBitmap.Width, spriteBitmap.Height);
            BitmapData bmpData = spriteBitmap.LockBits(rect, ImageLockMode.ReadWrite, spriteBitmap.PixelFormat);
            ReportLoadedBytes(br.BaseStream);

            try
            {
                byte[] rgbValues = decodedBitmapData.SelectMany(index => new List<byte>{
                                colorPalette[index].B,
                                colorPalette[index].G,
                                colorPalette[index].R,
                                (byte)((index == 0) ? 0 : 255)
                            }).ToArray();

                int expectedByteLen = Math.Abs(bmpData.Stride) * spriteBitmap.Height;
                if (rgbValues.Length != expectedByteLen)
                {
                    throw new Exception("Byte length mismatch! Expected " + expectedByteLen + " but got " + rgbValues.Length);
                }
                IntPtr ptr = bmpData.Scan0;
                Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
            }
            finally
            {
                spriteBitmap.UnlockBits(bmpData);
            }

            if (decodedTypeKey == 44 || decodedTypeKey == 45 || decodedTypeKey == 46 || decodedTypeKey == 47) // UI Element
            {
                spriteBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            Sprite = (Bitmap)spriteBitmap.Clone();
            spriteBitmap.Dispose();
        }
    }
}
