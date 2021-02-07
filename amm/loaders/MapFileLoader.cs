using AMMEdit.amm.blocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Binary;

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

        public MapFile Read()
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

                        // there seems to be a bug in the OATT field length int, so we need to parse it bit by bit
                        string id = new string(fieldID);
                        Int32 size;

                        switch (id)
                        {
                            case "TLAY":
                                TNAMBlock tNAMBlock = (TNAMBlock)fields.FindLast(x => x.DisplayFieldName == "Textures");
                                fields.Add(new TLAYBlock(r, tNAMBlock));
                                break;
                            case "TNAM":
                                fields.Add(new TNAMBlock(r, Directory.GetParent(this.infile).FullName));
                                break;
                            case "OLAY":
                                fields.Add(new OLAYBlock(r));
                                break;
                            case "OATT":
                                OLAYBlock lastOLAY = (OLAYBlock)fields.FindLast(x => x.DisplayFieldName == OLAYBlock.FIELD_NAME);
                                fields.Add(new OATTBlock(r, lastOLAY));
                                break;
                            case "SCEN":
                                fields.Add(new SCENBlock(r));
                                break;
                            case "VERS":
                                size = r.ReadInt32(); // length of version field. Always 8.
                                                            // this field always occurs either first or after CNUM. CNUM doesnt always appear.
                                major = r.ReadInt32();
                                minor = r.ReadInt32();

                                Span<byte> content = stackalloc byte[size];

                                BinaryPrimitives.WriteInt32LittleEndian(content.Slice(0, 4), major);
                                BinaryPrimitives.WriteInt32LittleEndian(content.Slice(4, 4), minor);

                                fields.Add(new GenericFieldBlock(fieldID, size, content.ToArray()));
                                break;
                            default:
                                size = r.ReadInt32();
                                {
                                    byte[] c = r.ReadBytes(size);

                                    fields.Add(new GenericFieldBlock(fieldID, size, c));
                                }
                                break;
                        }
                    }
                }
            }

            fields.Add(
                new CombinedTextureLayersBlock(
                    fields
                        .OfType<TLAYBlock>()
                        .ToList()
                )
            );

            return new MapFile(fields);
        }
    }
}
