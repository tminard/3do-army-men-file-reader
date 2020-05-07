using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    class MapFile
    {
        private readonly Uri file;

        private Dictionary<FractionBlock, List<UnitBlock>> armies; // ordered list of fractions and their units. Order: Green, Tan, Blue, Grey

        public MapFile(Uri file)
        {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
        }

        private void loadFile()
        {
            // Load the given file, seek to the end, and populate fractions block
        }
    }
}
