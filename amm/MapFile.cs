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

namespace AMMEdit.amm
{
    class MapFile
    {
        private List<IGenericFieldBlock> m_fields;

        public MapFile(List<IGenericFieldBlock> fields)
        {
            this.m_fields = fields;
        }

        public List<IGenericFieldBlock> GetGenericFields()
        {
            return this.m_fields;
        }

        public void SaveAs(string filename)
        {
            List<byte> orderedBytes = new List<byte>(); // TODO: make this a memory stream?

            m_fields.ForEach(f => orderedBytes.AddRange(f.ToBytes()));

            uint contentSize = Convert.ToUInt32(orderedBytes.Count());

            List<byte> content = new List<byte>();
            content.AddRange(new byte[] {
                0x46, 0x4F, 0x52, 0x4D
            });
            content.AddRange(UInt32ToBytes(contentSize));
            content.AddRange(new byte[] {
                0x4D, 0x41, 0x50, 0x20
            });
            content.AddRange(orderedBytes);

            File.WriteAllBytes(filename, content.ToArray());

            /*Bitmap bm = new Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            var bitmapData = bm.LockBits(new Rectangle(Point.Empty, bm.Size), ImageLockMode.ReadWrite, bm.PixelFormat);
            Marshal.Copy(m_fields[18].ToBytes(), 8, bitmapData.Scan0, 256*256);
            bm.UnlockBits(bitmapData);
            bm.Save(filename + ".move.bmp");*/
        }

        private byte[] UInt32ToBytes(UInt32 val)
        {
            Span<byte> buff = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(buff, val);

            return buff.Slice(0, 4).ToArray();
        }
    }
}
