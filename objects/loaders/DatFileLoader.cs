using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.objects.loaders
{
    class DatFileLoader
    {

        private string infile;

        private ProgressBar ProgressBarControl;

        public DatFileLoader(string infile, ProgressBar progressControl = null)
        {
            this.infile = infile ?? throw new ArgumentNullException(nameof(infile));
            this.ProgressBarControl = progressControl;
        }

        private delegate void updateListener();

        private void UpdateProgressListener()
        {
            if (ProgressBarControl != null)
            {
                ProgressBarControl.Value += 1;
            }
        }

        public DatFile Read()
        {
            //FileInfo fileSizeInfo = new FileInfo(infile);
            //long size = fileSizeInfo.Length;

            /* long incrementSize = (size / 100);
            long currentSize = 0;
            long lastReportedPosition = 0;
            */

            List<AMObject> amObjectsList = new List<AMObject>();

            using (FileStream fs = new FileStream(this.infile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    char[] checkSum = r.ReadChars(4);
                    byte[] paletteData = r.ReadBytes(256 * 4); // BGRr in 4 bytes
                    UInt32 numSprites = r.ReadUInt32();

                    // create the palette
                    List<Color> colorPalette = new List<Color>();
                    for (int c = 0; c < 256; c++)
                    {
                        byte bC = paletteData[c * 4];
                        byte gC = paletteData[(c * 4) + 1];
                        byte rC = paletteData[(c * 4) + 2];
                        byte unC = paletteData[(c * 4) + 3];
                        // from paletteData, each element is an int packing a BGRa color
                        colorPalette.Add(Color.FromArgb(rC, gC, bC));
                    }

                    /*if (r.BaseStream.Position - lastReportedPosition >= incrementSize)
                    {
                        lastReportedPosition = r.BaseStream.Position;
                        if (ProgressBarControl != null)
                        {
                            ProgressBarControl.Invoke(new updateListener(this.UpdateProgressListener));
                        }
                    }*/

                    long lookupPosition = r.BaseStream.Position;

                    // iterate over each sprite in the table and jump to content to read
                    for (long i = 0; i < numSprites; i++)
                    {
                        // jump back
                        r.BaseStream.Position = lookupPosition;
                        UInt32 encodedCategoryKey = r.ReadUInt32();
                        UInt32 dataOffset = r.ReadUInt32();
                        lookupPosition = r.BaseStream.Position;

                        UInt32 decodedTypeKey = (encodedCategoryKey & 0x7F8000) >> 15;
                        UInt32 decodedInstance = (encodedCategoryKey & 0x7FE0) >> 5;

                        // jump to data location
                        r.BaseStream.Position = dataOffset + 4; // skip first field which is just the encoded key again
                        var width = r.ReadUInt32();
                        var height = r.ReadUInt32();
                        var rleFlag = r.ReadUInt32();
                        r.BaseStream.Position += (4 * 3); // skip unknown fields

                        var rleEncodingMode = rleFlag & 0b1111;
                        var rleEncodingModeSecondPass = rleFlag & 0b11110000;

                        // read from known encodings
                        byte[] decodedBitmapData;
                        var widthPadded = width;

                        if (rleEncodingMode != 0 || rleEncodingModeSecondPass == 16)
                        {
                            var rleWidth = r.ReadInt16();
                            var rleHeight = r.ReadInt16();
                            widthPadded = Convert.ToUInt32(rleWidth + (4 - rleWidth % 4) % 4);

                            // ignore the line offset table
                            var lookupTableLength = (rleEncodingMode == 8 || rleEncodingMode == 0) ? height * 2 : height * 4;
                            r.BaseStream.Position += lookupTableLength;

                            // build the bitmap data - bitmap is indexed
                            decodedBitmapData = new byte[widthPadded * height];
                            var idx = 0;
                            for (int h = 0; h < height; h++)
                            {
                                idx = 0;
                                while (idx < width)
                                {
                                    // pad runs
                                    var p = r.ReadByte();
                                    for (int pad = 0; pad < p; pad++)
                                    {
                                        var bmpPtr = h * widthPadded + idx + pad;
                                        decodedBitmapData[bmpPtr] = 0; // transparent color in palette
                                    }
                                    idx += p;

                                    // expand color indexes
                                    p = r.ReadByte();
                                    for (int exp = 0; exp < p; exp++)
                                    {
                                        var bmpPtr = h * widthPadded + idx + exp;
                                        decodedBitmapData[bmpPtr] = r.ReadByte();
                                    }
                                    idx += p;
                                }
                            }
                        } else if (rleEncodingMode == 0 && rleEncodingModeSecondPass == 32)
                        {
                            // skip cannot read this - use `?` image in renderer
                            Debug.WriteLine("Could not load T" + decodedTypeKey + " i" + decodedInstance + ": Unsupported RLE mode.");

                            continue;
                        } else if (rleEncodingMode == 0)
                        {
                            // these tend to be UI images
                            widthPadded = width + (4 - width % 4) % 4;
                            decodedBitmapData = new byte[widthPadded * height];
                            decodedBitmapData = r.ReadBytes(Convert.ToInt32(widthPadded * height));
                        } else
                        {
                            // skip cannot read this - use `?` image in renderer
                            // TODO: ^
                            Debug.WriteLine("Could not load T" + decodedTypeKey + " i" + decodedInstance + ": Unrecognized RLE mode.");
                            continue;
                        }

                        if (decodedTypeKey == 17)
                        {
                            Debug.WriteLine("Found building of instance " + decodedInstance);
                        }

                        // create image
                        Bitmap spriteBitmap = new Bitmap(Convert.ToInt32(widthPadded), Convert.ToInt32(height));
                        for (int x = 0; x < Convert.ToInt32(widthPadded); x++)
                        {
                            for (int y = 0; y < Convert.ToInt32(height); y++)
                            {
                                int pos = x + (y * Convert.ToInt32(widthPadded));
                                spriteBitmap.SetPixel(x, y, colorPalette[decodedBitmapData[pos]]);
                            }
                        }

                        amObjectsList.Add(new AMObject(decodedTypeKey, decodedInstance, (Bitmap)spriteBitmap.Clone()));
                        spriteBitmap.Dispose();
                    }

                    return new DatFile(amObjectsList);
                }
            }
        }
    }
}
