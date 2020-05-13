﻿using AMMEdit.PropertyEditors;
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

        public int TileSquareSize { get; private set; }

        public int NumTiles { get; private set; }

        public TNAMBlock(BinaryReader r, string parentDirectory)
        {
            var cstringLen = r.ReadInt32();
            var cstring = r.ReadChars(cstringLen);

            TextureFileName = new string(cstring).Replace("\0", "");

            LoadTextureFile(parentDirectory + "\\" + TextureFileName);
        }

        public void ShowPropertyEditor(IWin32Window current)
        {
            TextureMap tm = new TextureMap(this);

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
                string.Format("Tile Size:\t{0}x{0}", TileSquareSize)
            };
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
                    r.BaseStream.Seek(28, SeekOrigin.Current); // skip version and header stuff

                    var tileWidth = r.ReadInt32();
                    var tileHeight = r.ReadInt32();
                    var numTileSlots = r.ReadInt32();
                    var numTileSlotsPerRow = r.ReadInt32();

                    TileSquareSize = tileHeight;
                    NumTiles = numTileSlots;

                    r.BaseStream.Seek(12, SeekOrigin.Current); // skip over DIB header stuff
                    byte[] imageBytes = r.ReadBytes(totalLength);

                    MemoryStream ms = new MemoryStream(imageBytes);
                    ms.Seek(0, SeekOrigin.Begin);

                    TextureImagesheet = new Bitmap(ms);
                }
            }
        }
    }
}
