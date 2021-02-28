using AMMEdit.PropertyEditors;
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

        private TNAMBlock TextureNameBlock { get; set; }

        public CombinedTextureLayersBlock(TNAMBlock textureNameBlock, List<TLAYBlock> textureBlocks)
        {
            this.TextureNameBlock = textureNameBlock;
            this.textureBlocks = textureBlocks ?? throw new ArgumentNullException(nameof(textureBlocks));
        }

        public string DisplayFieldName => "Final Map";

        public string FieldID => "MPFINAL";

        public bool CanEditProperties => true;

        public void ShowPropertyEditor(IWin32Window current)
        {
            TextureMap tm = new TextureMap(TextureNameBlock, textureBlocks.First(), textureBlocks.Last());

            tm.Show(current);
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
