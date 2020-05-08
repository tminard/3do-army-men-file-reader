using System;
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
    }
}
