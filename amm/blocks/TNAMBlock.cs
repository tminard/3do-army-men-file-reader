using AMMEdit.PropertyEditors;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.amm.blocks
{
    public class TNAMBlock : IGenericFieldBlock
    {
        public string DisplayFieldName => "Textures";

        public string FieldID => Guid.NewGuid().ToString();

        public bool CanEditProperties => true;

        public string TextureFileName { get; private set; }

        public Bitmap TextureImagesheet { get; private set; }

        public int TileRunLength { get; private set; }

        public int TileSquareSize { get; private set; }

        public int NumTiles { get; private set; }

        public int NumTilesPerRow { get; private set; }

        public TNAMBlock(BinaryReader r, string parentDirectory)
        {
            var cstringLen = r.ReadInt32();
            var cstring = r.ReadChars(cstringLen);

            TextureFileName = new string(cstring).Replace("\0", "");

            LoadTextureFile(parentDirectory + "\\" + TextureFileName);
        }

        public void ShowPropertyEditor(IWin32Window current)
        {
            TextureMap tm = new TextureMap(this, null);

            tm.Show(current);
        }

        public byte[] ToBytes()
        {
            var cstring = TextureFileName + "\0";

            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[4];

            content.AddRange(new byte[]
            {
                0x54, 0x4E, 0x41, 0x4D // TNAM
            });

            BinaryPrimitives.WriteInt32LittleEndian(buff, cstring.Length);
            content.AddRange(buff.Slice(0, 4).ToArray());
            content.AddRange(ASCIIEncoding.ASCII.GetBytes(cstring.ToArray()));

            return content.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            return new string[]
            {
                string.Format("File:\t{0}", TextureFileName),
                string.Format("Tiles:\t{0}", NumTiles),
                string.Format("Tile Size:\t{0}x{0}", TileSquareSize),
                string.Format("Tile Run Width:\t{0}", TileRunLength),
                string.Format("Raw Tile Slots:\t{0}", NumTiles),
                string.Format("Raw Tiles Per Scan Line:\t{0}", NumTilesPerRow)
            };
        }

        public Bitmap GetTileImage(UInt16 index)
        {
            int indx = Convert.ToInt32(index);
            int x = (index % NumTilesPerRow)*TileSquareSize;
            int y = (index / NumTilesPerRow)*TileSquareSize;
            Rectangle bounds = new Rectangle(new Point(x, y), new Size(TileSquareSize, TileSquareSize));

            return TextureImagesheet.Clone(bounds, TextureImagesheet.PixelFormat);
        }

        private void LoadTextureFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new ArgumentException(string.Format("Please load from the location that contains {0}; not found at {1}", this.TextureFileName, filename));
            }

            /**
             * Texture file is pretty simple: a short header followed by a bitmap
             **/
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    r.BaseStream.Seek(4, SeekOrigin.Begin);
                    var totalLength = r.ReadInt32();
                    r.BaseStream.Seek(24, SeekOrigin.Current); // skip version and header stuff

                    var tileRunLength = r.ReadInt32(); // this appears to be the number of tiles in a single span/category. Not important for the game but perhaps meaningful for the editor
                    var tileWidth = r.ReadInt32(); // OR this may be the height of each span/category
                    var tileHeight = r.ReadInt32(); // OR this may be the square size of a single tile
                    var numTileSlots = r.ReadInt32(); // raw image slots
                    var numTileSlotsPerRow = r.ReadInt32(); // raw image slots per row in the bitmap

                    TileRunLength = tileRunLength;
                    TileSquareSize = tileHeight;
                    NumTiles = numTileSlots;
                    NumTilesPerRow = numTileSlotsPerRow;

                    r.BaseStream.Seek(12, SeekOrigin.Current); // skip over DIB header stuff
                    byte[] imageBytes = r.ReadBytes(totalLength);

                    MemoryStream ms = new MemoryStream(imageBytes);
                    ms.Seek(0, SeekOrigin.Begin);

                    TextureImagesheet = new Bitmap(ms);
                    TextureImagesheet.MakeTransparent(TextureImagesheet.Palette.Entries[0]);
                }
            }
        }
    }
}
