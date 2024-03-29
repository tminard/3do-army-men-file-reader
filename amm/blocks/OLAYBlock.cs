﻿using AMMEdit.amm.blocks.subfields;
using AMMEdit.objects;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AMMEdit.amm.blocks
{
    public class OLAYBlock : IGenericFieldBlock
    {
        public static string FIELD_NAME = "Object Definition Layer (OLAY)";

        private List<OLAYObject> m_indexedObjects;

        private Int32 m_blockLength;
        public Int32 m_numObjects { get; private set; }

        public OLAYBlock(BinaryReader r)
        {
            if (r is null)
            {
                throw new ArgumentNullException(nameof(r));
            }

            DisplayFieldName = FIELD_NAME;
            FieldID = Guid.NewGuid().ToString();

            m_blockLength = r.ReadInt32();
            m_numObjects = r.ReadInt32();

            m_indexedObjects = new List<OLAYObject>(m_numObjects);

            for (int i = 0; i < m_numObjects; i++)
            {
                m_indexedObjects.Add(new OLAYObject(r));
            }
        }

        public List<KeyValuePair<int, OLAYObject>> GetObjectsByLocation(int x, int y, DatFile datFile)
        {
            if (datFile == null)
            {
                return new List<KeyValuePair<int, OLAYObject>>();
            }

            Point p = new Point(x, y);

            return m_indexedObjects.Where(obj =>
            {
                AMObject objInstance = datFile.GetObject(obj.m_itemCategory, obj.m_itemSubType);
                if (objInstance == null)
                {
                    return false;
                }

                Rectangle sprite = new Rectangle(obj.m_itemPosX, obj.m_itemPosY, objInstance.SpriteImage.Width, objInstance.SpriteImage.Height);

                if (sprite.Contains(p))
                {
                    // sample and check if this is a transparent color
                    Point px = new Point(
                        x - obj.m_itemPosX,
                        y - obj.m_itemPosY
                        );

                    return objInstance.SpriteImage.GetPixel(px.X, px.Y).A == 255;
                }

                return false;
            }).Select(obj =>
            new KeyValuePair<int, OLAYObject>(m_indexedObjects.FindIndex(objI => objI.Equals(obj)), obj)
            ).ToList();
        }

        public List<OLAYObject> GetRenderOrderedObjects(DatFile datFile)
        {
            return m_indexedObjects;
        }

        public OLAYObject GetObjectByIndex(int index)
        {
            return m_indexedObjects[index];
        }

        public void AddObject(OLAYObject amObject)
        {
            m_indexedObjects.Add(amObject);
            m_numObjects = m_indexedObjects.Count;
        }

        public void DeleteObject(int index)
        {
            m_indexedObjects.RemoveAt(index);
            m_numObjects = m_indexedObjects.Count;
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
            List<byte> list = new List<byte>();
            Span<byte> buff = stackalloc byte[8];

            Int32 contentBytes = m_indexedObjects.Aggregate(0, (cur, n) => cur += n.ToBytes().Length) + 4;

            // ID
            list.AddRange(ASCIIEncoding.ASCII.GetBytes("OLAY".ToArray()));

            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(0), contentBytes);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(4), Convert.ToInt32(m_indexedObjects.Count));

            list.AddRange(buff.Slice(0, 8).ToArray());
            buff.Clear();

            m_indexedObjects.ToList().ForEach(kv =>
            {
                list.AddRange(kv.ToBytes());
            });

            return list.ToArray();
        }

        public string[] ToFormattedPreview()
        {
            List<string> lines = new List<string>
            {
                string.Format("Field: {0}", DisplayFieldName),
                string.Format("Original size in file header: {0}", m_blockLength),
                string.Empty,
                string.Format("Objects defined: {0}", m_indexedObjects.Count),
                string.Empty,
                "== Objects defined =="
            };
            m_indexedObjects.ToList().ForEach(kv =>
            {
                lines.Add("[");
                foreach (var line in kv.ToFormattedPreview())
                {
                    lines.Add(string.Format("\t{0}", line));
                }
                lines.Add("]");
            });

            return lines.ToArray();
        }
    }
}
