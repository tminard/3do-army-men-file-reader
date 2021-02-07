using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.amm.blocks
{
    class CombinedTextureLayersBlock : IGenericFieldBlock
    {
        private readonly List<TLAYBlock> textureBlocks;

        public CombinedTextureLayersBlock(List<TLAYBlock> textureBlocks)
        {
            this.textureBlocks = textureBlocks ?? throw new ArgumentNullException(nameof(textureBlocks));
        }

        public string DisplayFieldName => "Final Map";

        public string FieldID => "MPFINAL";

        public bool CanEditProperties => true;

        public void ShowPropertyEditor(IWin32Window current)
        {
            throw new NotImplementedException();
        }

        /**
         * We return an empty array here as we do not wish to write content to disk
         **/
        public byte[] ToBytes()
        {
            return new byte[] { };
        }

        public string[] ToFormattedPreview()
        {
            return new string[]
            {
                "Preview all layers combined",
                string.Format("Layers: {0}", textureBlocks.Count())
            };
        }
    }
}
