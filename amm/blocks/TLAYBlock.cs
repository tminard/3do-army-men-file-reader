using AMMEdit.PropertyEditors;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AMMEdit.amm.blocks
{
    public class TLAYBlock : IGenericFieldBlock
    {
        public Int32 LayerNumber { get; private set; }

        public Int32 Width { get; private set; }

        public Int32 Height { get; private set; }

        public Int32 UnknownField1 { get; private set; }

        public Int32 UnknownField2 { get; private set; }

        private List<byte> RawTileTextureIDs { get; set; }

        private byte[] RawTextureIDs;
        
        private TNAMBlock TextureNameBlock { get; set; }

        public TLAYBlock(BinaryReader r, TNAMBlock textureBlock)
        {
            TextureNameBlock = textureBlock;

            _ = r.ReadInt32();
            LayerNumber = r.ReadInt32();
            Width = r.ReadInt32();
            Height = r.ReadInt32();
            UnknownField1 = r.ReadInt32();
            UnknownField2 = r.ReadInt32();

            RawTileTextureIDs = new List<byte>(r.ReadBytes(Width * Height * sizeof(UInt16)));
            RawTextureIDs = RawTileTextureIDs.ToArray();
        }

        public string DisplayFieldName => string.Format("Texture Layer {0}", LayerNumber);

        public string FieldID => Guid.NewGuid().ToString();

        public bool CanEditProperties => TextureNameBlock.DidLoadTexture;

        public void ShowPropertyEditor(IWin32Window current)
        {
            if (TextureNameBlock.DidLoadTexture)
            {
                TextureMap tm = new TextureMap(TextureNameBlock, this);

                tm.Show(current);
            }
        }

        public UInt16 GetTextureIDAtLocation(int x, int y)
        {
            // convert from XY to linear sequence
            int seqX = x * 2; // each segment is represented by 2 bytes (UInt16)
            int seqY = y * Width * 2;
            int seq = seqY + seqX;

            if (seq + 2 > RawTileTextureIDs.Count)
            {
                Debug.WriteLine("Tried to access texture at x: " + x + ", y: " + y + " but was greater than max textures. Skipping");
                return 0;
            }

            Span<byte> ranAccess = new Span<byte>(RawTextureIDs);
            UInt16 id = BinaryPrimitives.ReadUInt16LittleEndian(ranAccess.Slice(seq, 2));

            return id;
        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[128];

            content.AddRange(new byte[]
            {
                0x54, 0x4C, 0x41, 0x59 // TLAY
            });

            BinaryPrimitives.WriteInt32LittleEndian(buff, RawTileTextureIDs.Count + 20); // textures plus below header details
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(4), LayerNumber);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(8), Width);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(12), Height);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(16), UnknownField1);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(20), UnknownField2);

            content.AddRange(buff.Slice(0, 24).ToArray()); // header + size signature
            content.AddRange(RawTileTextureIDs);

            return content.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            return new string[]
            {
                "Section ID:\tTLAY",
                string.Format("Layer Number:\t{0}", LayerNumber),
                string.Format("Size:\t{0}x{1}", Width, Height),
                string.Format("Unknown Field 1:\t{0}", UnknownField1),
                string.Format("Unknown Field 2:\t{0}", UnknownField2),
                string.Empty,
                "This section contains the texture map for the above layer.\nIn Army Men, maps can have multiple texture layers.\nEach layer is render one over the other in sequence starting with 0.\nEach tile is a UInt16 that maps to a tile ID in the ATT file."
            };
        }
    }
}
