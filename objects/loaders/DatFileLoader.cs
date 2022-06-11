using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AMMEdit.objects.loaders
{
    class DatFileLoader
    {
        private const uint FILE_ID_AM2 = 2173830758;
        private readonly string infile;
        private readonly ProgressBar ProgressBarControl;

        public DatFileLoader(string infile, ProgressBar progressControl = null)
        {
            this.infile = infile ?? throw new ArgumentNullException(nameof(infile));
            this.ProgressBarControl = progressControl;
        }

        private delegate void updateListener();

        private delegate void resetListener(int numberOfSprites);

        private void UpdateProgressListener()
        {
            if (ProgressBarControl != null)
            {
                ProgressBarControl.PerformStep();
            }
        }

        private void ResetProgressListener(int numberOfSprites)
        {
            if (ProgressBarControl != null)
            {
                ProgressBarControl.Maximum = numberOfSprites;
                ProgressBarControl.Step = 1;
            }
        }

        public DatFile Read()
        {
            List<AMObject> amObjectsList = new List<AMObject>();

            using (FileStream fs = new FileStream(this.infile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    UInt32 checkSum = br.ReadUInt32();
                    byte[] paletteData = br.ReadBytes(256 * 4); // BGRr in 4 bytes
                    UInt32 numSprites = br.ReadUInt32();

                    if (ProgressBarControl != null)
                    {
                        ProgressBarControl.Invoke(new resetListener(this.ResetProgressListener), Convert.ToInt32(numSprites));
                    }

                    // create the palette
                    List<Color> colorPalette = new List<Color>();
                    for (int c = 0; c < 256; c++)
                    {
                        byte b = paletteData[c * 4];
                        byte g = paletteData[(c * 4) + 1];
                        byte r = paletteData[(c * 4) + 2];
                        // from paletteData, each element is an int packing a BGRa color
                        colorPalette.Add(Color.FromArgb(r, g, b));
                    }

                    long lookupPosition = br.BaseStream.Position;

                    // iterate over each sprite in the table and jump to content to read
                    for (long i = 0; i < numSprites; i++)
                    {
                        if (ProgressBarControl != null)
                        {
                            ProgressBarControl.Invoke(new updateListener(this.UpdateProgressListener));
                        }
                        // jump back
                        br.BaseStream.Position = lookupPosition;
                        UInt32 encodedCategoryKey = br.ReadUInt32();
                        UInt32 dataOffset = br.ReadUInt32();
                        lookupPosition = br.BaseStream.Position;

                        UInt32 decodedTypeKey = (encodedCategoryKey & 0x7F8000) >> 15;
                        UInt32 decodedInstance = (encodedCategoryKey & 0x7FE0) >> 5;

                        if (checkSum == FILE_ID_AM2)
                        {
                            // AM 2 +
                            UInt32 am2Type = (encodedCategoryKey & 0b00000001111110000000000000000000) >> 19;
                            decodedInstance = (encodedCategoryKey & 0b000001111111111110000000) >> 7;

                            switch (am2Type)
                            {
                                case 0b010101:
                                    decodedTypeKey = 1;
                                    break;
                                case 0b010110:
                                    decodedTypeKey = 2;
                                    break;
                                case 0b010111:
                                    decodedTypeKey = 3;
                                    break;
                                case 0b011001:
                                    decodedTypeKey = 5;
                                    break;
                                case 0b011010:
                                    decodedTypeKey = 6;
                                    break;
                                case 0b011101:
                                    decodedTypeKey = 9;
                                    break;
                                case 0b011111:
                                    decodedTypeKey = 11;
                                    break;
                                case 0b011011:
                                    decodedTypeKey = 7;
                                    break;
                                case 0b100101:
                                    decodedTypeKey = 17;
                                    break;
                                case 0b101011:
                                    decodedTypeKey = 23;
                                    break;
                                case 0b101101:
                                    decodedTypeKey = 25;
                                    break;
                                case 0b010100:
                                    decodedTypeKey = 0;
                                    break;
                                default:
                                    break;
                            }
                        }

                        // jump to data location
                        br.BaseStream.Position = dataOffset + 4; // skip first field which is just the encoded key again
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
                        else if (rleEncodingMode == 0 && rleEncodingModeSecondPass == 32)
                        {
                            // skip cannot read this - use `?` image in renderer
                            Debug.WriteLine("Could not load T" + decodedTypeKey + " i" + decodedInstance + ": Unsupported RLE mode.");

                            continue;
                        }
                        else if (rleEncodingMode == 0)
                        {
                            // these tend to be UI images
                            widthPadded = width + (4 - width % 4) % 4;
                            decodedBitmapData = new byte[widthPadded * height];
                            decodedBitmapData = br.ReadBytes(Convert.ToInt32(widthPadded * height));
                        }
                        else
                        {
                            // skip cannot read this - use `?` image in renderer
                            // TODO: ^
                            Debug.WriteLine("Could not load T" + decodedTypeKey + " i" + decodedInstance + ": Unrecognized RLE mode.");
                            continue;
                        }

                        // create image
                        Bitmap spriteBitmap = new Bitmap(Convert.ToInt32(widthPadded), Convert.ToInt32(height), PixelFormat.Format32bppArgb);
                        
                        /*for (int x = 0; x < Convert.ToInt32(widthPadded); x++)
                        {
                            for (int y = 0; y < Convert.ToInt32(height); y++)
                            {
                                int pos = x + (y * Convert.ToInt32(widthPadded));
                                // This is very slow.
                                spriteBitmap.SetPixel(x, y, colorPalette[decodedBitmapData[pos]]);
                            }
                        }*/
                        // Let's try lockbits instead
                        // TODO: refactor this logic out
                        Rectangle rect = new Rectangle(0, 0, Convert.ToInt32(widthPadded), Convert.ToInt32(height));
                        BitmapData bmpData = spriteBitmap.LockBits(rect, ImageLockMode.ReadWrite, spriteBitmap.PixelFormat);
                        IntPtr ptr = bmpData.Scan0;
                        int bytes = Math.Abs(bmpData.Stride) * spriteBitmap.Height;
                        byte[] rgbValues = new byte[bytes];
                        Marshal.Copy(ptr, rgbValues, 0, bytes);
                        for (int x = 0; x < Convert.ToInt32(widthPadded); x++)
                          {
                              for (int y = 0; y < Convert.ToInt32(height); y++)
                              {
                                int pos = x + (y * Convert.ToInt32(widthPadded));
                                int byteOffset = Math.Abs((bmpData.Stride / spriteBitmap.Width) * x) + (y * bmpData.Stride);

                                Color c = colorPalette[decodedBitmapData[pos]];
                                rgbValues[byteOffset] = c.B;
                                rgbValues[byteOffset + 1] = c.G;
                                rgbValues[byteOffset + 2] = c.R;
                                rgbValues[byteOffset + 3] = (byte)((c == colorPalette[0]) ? 0 : 255);
                            }
                        }
                        Marshal.Copy(rgbValues, 0, ptr, bytes);
                        spriteBitmap.UnlockBits(bmpData);

                        if (decodedTypeKey == 44 || decodedTypeKey == 45 || decodedTypeKey == 46 || decodedTypeKey == 47) // UI Element
                        {
                            spriteBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }

                        // Compute sequence
                        int existingInstanceCount = amObjectsList
                            .Where(obj => obj.TypeKey == decodedTypeKey && obj.InstanceKey == decodedInstance)
                            .Count();

                        // Add
                        amObjectsList.Add(
                            new AMObject(encodedCategoryKey, decodedTypeKey, decodedInstance, (Bitmap)spriteBitmap.Clone(), existingInstanceCount)
                        );
                        spriteBitmap.Dispose();
                    }

                    return new DatFile(amObjectsList);
                }
            }
        }
    }
}
