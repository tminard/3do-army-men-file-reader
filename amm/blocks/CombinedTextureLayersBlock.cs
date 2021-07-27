using AMMEdit.objects;
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

        private readonly DatFile dataFileRef;
        private readonly List<Tuple<OLAYBlock, OATTBlock>> objectBlocks;
        private readonly List<GenericFieldBlock> flagBlocks;

        private TNAMBlock TextureNameBlock { get; set; }

        public CombinedTextureLayersBlock(TNAMBlock textureNameBlock, List<TLAYBlock> textureBlocks, List<Tuple<OLAYBlock, OATTBlock>> objectBlocks = null, DatFile dataFile = null, List<GenericFieldBlock> flagBlocks = null)
        {
            this.TextureNameBlock = textureNameBlock;
            this.textureBlocks = textureBlocks ?? throw new ArgumentNullException(nameof(textureBlocks));
            this.objectBlocks = objectBlocks;
            this.dataFileRef = dataFile;
            this.flagBlocks = flagBlocks;
        }

        public string DisplayFieldName => "Final Map";

        public string FieldID => "MPFINAL";

        public bool CanEditProperties => true;

        public void ShowPropertyEditor(IWin32Window current)
        {
            TextureMap tm = new TextureMap(TextureNameBlock, textureBlocks.First(), textureBlocks.Last(), objectBlocks, dataFileRef, flagBlocks);

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
