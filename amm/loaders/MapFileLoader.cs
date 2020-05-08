using AMMEdit.amm.blocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    class MapFileLoader
    {
        private char[] MAGIC = new char[] { 'F', 'O', 'R', 'M' }; // FORM

        private string infile;

        public MapFileLoader(string infile)
        {
            this.infile = infile ?? throw new ArgumentNullException(nameof(infile));
        }

        public MapFile read()
        {
            List<IGenericFieldBlock> fields = new List<IGenericFieldBlock>();

            using (FileStream fs = new FileStream(this.infile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    // construct an object using the file spec
                    char[] fileID = r.ReadChars(4);
                    if (string.Compare(new string(fileID), new string(MAGIC)) != 0) {
                        throw new ArgumentException("Not a valid Army Men 1 map file");
                    }

                    // checknum
                    UInt32 fileContentSizeBytes = r.ReadUInt32(); // not including magic id and file size
                    _ = r.ReadChars(4); // "MAP " identifier

                    // I dont know that this is correct way to read the version
                    int major = 6;
                    int minor = 0;

                    while (r.BaseStream.Position != r.BaseStream.Length)
                    {
                        char[] fieldID = r.ReadChars(4);

                        if (r.BaseStream.Position == r.BaseStream.Length) // some files appear to have an extra nil at the end
                        {
                            break;
                        }

                        // there seems to be a bug in the OATT field length int
                        if (new string(fieldID) == "OATT")
                        {
                            bool longerObjectHeader = major > 5;
                            fields.Add(new OATTBlock(r, longerObjectHeader));
                        } else if (new string(fieldID) == "VERS")
                        {
                            _ = r.ReadInt32(); // length of version field. Always 8.
                            // this field always occurs either first or after CNUM. CNUM doesnt always appear.
                            major = r.ReadInt32();
                            minor = r.ReadInt32();
                        }
                        else
                        {
                            Int32 size = r.ReadInt32(); ;
                            byte[] content = r.ReadBytes(size);

                            fields.Add(new GenericFieldBlock(fieldID, size, content));
                        }
                    }
                }
            }

            return new MapFile(fields);
        }
    }
}
