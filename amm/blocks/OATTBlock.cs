using AMMEdit.amm.blocks.subfields;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.amm.blocks
{
    class OATTBlock : IGenericFieldBlock
    {
        private Dictionary<string, Int32> m_keyValuePairs;
        private Int32 m_blockLength; // untrustworthy. Appears to be off by one in several files in unpredictable ways
        private Int32 m_correctedBlockLength;
        private List<PlaceableObject> m_placeables;

        private OLAYBlock m_olay;

        public OATTBlock(BinaryReader r, OLAYBlock olay)
        {
            if (r is null)
            {
                throw new ArgumentNullException(nameof(r));
            }

            if (olay is null)
            {
                throw new ArgumentNullException(nameof(olay));
            }

            DisplayFieldName = "Objects (OATT)";
            FieldID = Guid.NewGuid().ToString();

            m_olay = olay; // needed to map indexes to objects

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
            m_correctedBlockLength = (4 * 2 * numFields) + 4;

            // read the object placements
            int numPlacements = r.ReadInt32();
            m_correctedBlockLength += 4;

            for (int i = 0; i < numPlacements; i++)
            {
                m_placeables.Add(new PlaceableObject(m_keyValuePairs, r));
                m_correctedBlockLength += m_placeables.Last().ToBytes().Length;
            }
        }

        public string DisplayFieldName { get; }

        public string FieldID { get; }

        public bool CanEditProperties => false;

        public void ShowPropertyEditor(IWin32Window current)
        {
            throw new NotImplementedException();
        }

        public byte[] ToBytes()
        {
            // Im sure theres a better way to do this (perhaps using structs with explicit layout, or custom serializers), but lets just get this done for now
            // This is one of the more complex blocks in the file spec.
            List<byte> list = new List<byte>();
            Span<byte> buff = stackalloc byte[1024];

            // ID
            list.AddRange(ASCIIEncoding.ASCII.GetBytes("OATT".ToArray()));

            // length
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(0), m_correctedBlockLength);
            list.AddRange(buff.Slice(0, 4).ToArray());

            // numfields
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(0), Convert.ToInt32(m_keyValuePairs.Count));
            list.AddRange(buff.Slice(0, 4).ToArray());

            // key-value pairs
            // key is the field name. Value is the number of bytes in the data
            m_keyValuePairs.ToList().ForEach(kv => {
                list.AddRange(ASCIIEncoding.ASCII.GetBytes(kv.Key.ToArray()));

                byte[] val = new byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(val, kv.Value);
                list.AddRange(val);
            });

            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(0), Convert.ToInt32(m_placeables.Count));
            list.AddRange(buff.Slice(0, 4).ToArray());

            // placements
            m_placeables.ForEach(x => list.AddRange(x.ToBytes()));

            // return the byte array
            return list.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            List<string> lines = new List<string>
            {
                string.Format("Field: {0}", DisplayFieldName),
                string.Format("Actual size (in bytes): {0}", m_correctedBlockLength),
                string.Format("Original size in file header: {0}", m_blockLength),
                string.Empty,
                string.Format("Key-Values: {0}", m_keyValuePairs.Count),
                string.Format("Placeables defined: {0}", m_placeables.Count),
                string.Empty,
                "== Key-Values defined =="
            };
            m_keyValuePairs.ToList().ForEach(kv => lines.Add(string.Format("{0}\t=\t`{1}`", kv.Key, kv.Value)));
            lines.Add(string.Empty);
            lines.Add("== Placeables defined ==");
            m_placeables.ToList().ForEach(kv =>
            {
                int objInx = kv.ObjectIndex;

                lines.Add("[");
                foreach (var line in kv.ToFormattedDescription(m_olay.GetObjectByIndex(objInx)))
                {
                    lines.Add(string.Format("\t{0}", line));
                }
                lines.Add("]");
            });

            return lines.ToArray();
        }
    }
}
