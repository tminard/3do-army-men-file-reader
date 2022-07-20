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

        private delegate void updateListener(int numBytesRead);

        private delegate void resetListener(int totalBytes);

        private void UpdateProgressListener(int numBytesRead)
        {
            if (ProgressBarControl != null)
            {
                ProgressBarControl.Value = numBytesRead;
            }
        }

        private void ResetProgressListener(int totalBytes)
        {
            if (ProgressBarControl != null)
            {
                ProgressBarControl.Maximum = totalBytes;
                ProgressBarControl.Value = 0;
            }
        }

        private void reportReadProgress(Stream fs)
        {
            if (ProgressBarControl != null)
            {
                ProgressBarControl.Invoke(new updateListener(this.UpdateProgressListener), Convert.ToInt32(fs.Position));
            }
        }

        public DatFile Read()
        {
            List<AMObject> amObjectsList = new List<AMObject>();
            List<DatFileEntry> DatFileEntries = new List<DatFileEntry>();

            using (FileStream fs = new FileStream(this.infile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    UInt32 checkSum = br.ReadUInt32();
                    byte[] paletteData = br.ReadBytes(256 * 4); // BGRr in 4 bytes
                    UInt32 numSprites = br.ReadUInt32();

                    if (ProgressBarControl != null)
                    {
                        ProgressBarControl.Invoke(new resetListener(this.ResetProgressListener), Convert.ToInt32(fs.Length));
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

                        // Compute sequence
                        int existingInstanceCount = amObjectsList
                            .Where(obj => obj.TypeKey == decodedTypeKey && obj.InstanceKey == decodedInstance)
                            .Count();

                        // Decode and load
                        br.BaseStream.Position = dataOffset + 4; // skip first field which is just the encoded key again
                        DatFileEntry objectEntry = new DatFileEntry(colorPalette, decodedTypeKey, decodedInstance, br, reportReadProgress);
                        DatFileEntries.Add(objectEntry);

                        // Add
                        if (objectEntry.Sprite != null)
                        {
                            amObjectsList.Add(
                                new AMObject(encodedCategoryKey, decodedTypeKey, decodedInstance,
                                    objectEntry.Sprite, // Note we are passing the reference
                                    existingInstanceCount
                                )
                            );
                        }

                        reportReadProgress(fs);
                    }
                    reportReadProgress(fs);

                    return new DatFile(amObjectsList);
                }
            }
        }
    }
}
