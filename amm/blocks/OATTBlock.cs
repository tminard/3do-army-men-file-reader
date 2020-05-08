using AMMEdit.amm.blocks.subfields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks
{
    class OATTBlock : IGenericFieldBlock
    {
        private Dictionary<string, Int32> m_keyValuePairs;
        private Int32 m_blockLength; // untrustworthy. Appears to be off by one in several files in unpredictable ways
        private List<PlaceableObject> m_placeables;

        public OATTBlock(BinaryReader r, bool longerHeader)
        {
            if (r is null)
            {
                throw new ArgumentNullException(nameof(r));
            }

            DisplayFieldName = "Objects (OATT)";
            FieldID = Guid.NewGuid().ToString();

            m_keyValuePairs = new Dictionary<string, int>();
            m_placeables = new List<PlaceableObject>();

            // read the stream to extract all the key-values
            m_blockLength = r.ReadInt32();

            int numFields = r.ReadInt32();
            for (int i = 0; i < numFields; i++)
            {
                string n = new string(r.ReadChars(4));
                Int32 v = r.ReadInt32();

                m_keyValuePairs.Add(n, v);
            }

            // read the object placements
            int numPlacements = r.ReadInt32();
            for (int i = 0; i < numPlacements; i++)
            {
                // TODO: review assembly to determine how fields are parsed
                Int32 index = r.ReadInt32();
                Int32 unknownA = r.ReadInt32(); // unknown
                byte unknownB = 0;
                if (longerHeader)
                {
                    unknownB = r.ReadByte(); // read an extra byte - no idea what it is yet
                }
                Int32 nameLength = r.ReadInt32();
                string name = "Unnamed"; // TODO: let Placeable handle this

                if (nameLength > 0)
                {
                    name = new string(r.ReadChars(nameLength));
                }

                m_placeables.Add(new PlaceableObject(index, unknownA, unknownB, name));
            }
        }

        public string DisplayFieldName { get; }

        public string FieldID { get; }
    }
}
