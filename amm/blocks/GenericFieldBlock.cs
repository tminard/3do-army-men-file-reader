using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    class GenericFieldBlock : IGenericFieldBlock
    {
        private readonly char[] fieldName;
        private readonly Int32 sizeInBytes;
        private readonly byte[] content;

        public string DisplayFieldName { get; }
        public string FieldID { get; }

        public GenericFieldBlock(char[] name, Int32 contentSizeInBytes, byte[] content)
        {
            this.fieldName = name;
            this.sizeInBytes = contentSizeInBytes;
            this.content = content;

            this.DisplayFieldName = new string(this.fieldName);
            this.FieldID = Guid.NewGuid().ToString();
        }

        public byte[] ToBytes()
        {
            List<byte> list = new List<byte>();
            Span<byte> buff = stackalloc byte[1024];

            // ID
            list.AddRange(ASCIIEncoding.ASCII.GetBytes(fieldName));

            // length
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(0), content.Length);
            list.AddRange(buff.Slice(0, 4).ToArray());

            // content
            list.AddRange(this.content);

            return list.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            List<string> lines = new List<string>();

            lines.Add(string.Format("Field: {0}", DisplayFieldName));
            lines.Add(string.Format("Size (in bytes): {0}", sizeInBytes));
            lines.Add(string.Empty);
            lines.Add("No preview is available for this block");

            return lines.ToArray();
        }
    }
}
