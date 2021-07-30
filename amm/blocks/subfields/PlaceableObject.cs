using AMMEdit.PropertyEditors;
using AMMEdit.PropertyEditors.dialogs;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;

namespace AMMEdit.amm.blocks.subfields
{
    /**
     * Appears to contain metadata for <see cref="OLAYObject"/>.
     */
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlaceableObject : IPlaceableObject
    {
        public class PlaceableObjectPropertyEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (provider != null)
                {
                    var _service = ((IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService)));

                    PlaceableObject placeableTemplate = (PlaceableObject)value;
                    SelectedTileDataSource dataSource = (SelectedTileDataSource)context.Instance;

                    if (dataSource.SelectedOATTBlock == null)
                    {
                        // we cannot update or add without a reference to the OATT block
                        return value;
                    }

                    bool newEntry = false;
                    if (_service != null && value == null)
                    {
                        placeableTemplate = new PlaceableObject(dataSource.SelectedOATTBlock.m_keyValuePairs)
                        {
                            ObjectIndex = dataSource.SelectedObjectIndex
                        };
                        newEntry = true;
                    }

                    var editorForm = new EditPlaceableObjectProperties(placeableTemplate);

                    if (editorForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (newEntry)
                        {
                            // update the OATT block and grab the reference
                            var addedPlaceable = dataSource.SelectedOATTBlock.AddPlaceable(editorForm.PlaceableObject);

                            if (addedPlaceable != null)
                            {
                                value = addedPlaceable;
                            }
                            else
                            {
                                Debug.WriteLine("Attempted to add placeable but got NULL - is the placeable valid?");
                            }
                        }
                        else
                        {
                            ((PlaceableObject)value).CopyFrom(editorForm.PlaceableObject);
                        }
                    }
                }

                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        private Dictionary<string, Int32> m_fields; // field name, and byte length
        private Dictionary<string, List<byte>> m_values;
        private string m_name; // nil terminated when written. Two objects cannot share the same name, UNLESS the name is empty

        public string Description
        {
            get
            {
                return String.Join("\n", ToFormattedDescription());
            }
        }

        public int ObjectIndex
        {
            get
            {
                return GetFieldValue("INDX");
            }
            private set
            {
                SetFieldValue("INDX", value);
            }
        }

        public int NameLength
        {
            get
            {
                return GetFieldValue("SCRI");
            }
        }

        [Category("Properties"), Description("Movement block - unknown affect"), DisplayName("MOVE")]
        [Editor(typeof(FlagEditor.TypeEditor), typeof(UITypeEditor))]
        public byte MoveBlock
        {
            get
            {
                return GetByteFieldValue("MOVE");
            }
            set
            {
                SetByteFieldValue("MOVE", value);
            }
        }

        [Category("Properties"), Description("ELOW block - unknown affect"), DisplayName("ELOW")]
        [Editor(typeof(FlagEditor.TypeEditor), typeof(UITypeEditor))]
        public byte ELOWBlock
        {
            get
            {
                return GetByteFieldValue("ELOW");
            }
            set
            {
                SetByteFieldValue("ELOW", value);
            }
        }

        [Category("Properties"), Description("TRIG block - unknown affect"), DisplayName("TRIG")]
        public byte TRIGBlock
        {
            get
            {
                return GetByteFieldValue("TRIG");
            }
            set
            {
                SetByteFieldValue("TRIG", value);
            }
        }

        [Category("Properties"), Description("GRUP block - unknown affect"), DisplayName("GRUP")]
        public byte GRUPBlock
        {
            get
            {
                return GetByteFieldValue("GRUP");
            }
            set
            {
                SetByteFieldValue("GRUP", value);
            }
        }

        [Category("Properties"), Description("Bullets in power up"), DisplayName("Bullets")]
        public byte NumBullets
        {
            get
            {
                return GetByteFieldValue("NUMB");
            }
            set
            {
                SetByteFieldValue("NUMB", value);
            }
        }

        [Category("Properties"), Description("Reference to use in scripts"), DisplayName("Script Name")]
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (value.Trim().Length > 12)
                {
                    value = value.Trim().Substring(0, 11);
                }
                else
                {
                    value = value.Trim();
                }

                if (!value.EndsWith("\0") && value.Length > 0)
                {
                    value += "\0";
                }
                else if (value.Length == 0)
                {
                    value = ""; // only nil terminated if non-empty
                }

                m_name = value;

                SetFieldValue("SCRI", Math.Max(0, m_name.Length));
            }
        }

        public void CopyFrom(PlaceableObject other)
        {
            m_fields = new Dictionary<string, int>(other.m_fields);
            m_values = new Dictionary<string, List<byte>>(other.m_values);
            m_name = other.m_name;
        }

        public void SetObjectIndex(int index)
        {
            ObjectIndex = index;
        }

        public PlaceableObject(PlaceableObject existing)
        {
            m_fields = new Dictionary<string, int>(existing.m_fields);
            m_values = new Dictionary<string, List<byte>>(existing.m_values);
            m_name = existing.m_name;
        }

        public PlaceableObject(Dictionary<string, Int32> fields)
        {
            m_fields = fields;
            m_values = new Dictionary<string, List<byte>>();
            m_name = "";

            m_fields.ToList().ForEach(kv =>
            {
                List<byte> d;
                switch (kv.Value)
                {
                    case 1:
                        d = new List<byte>(1)
                        {
                            0b0
                        };

                        m_values.Add(kv.Key, d);
                        break;
                    default:
                        d = new List<byte>(kv.Value);
                        byte[] emptyList = new byte[kv.Value];
                        d.AddRange(emptyList);

                        m_values.Add(kv.Key, d);
                        break;
                }
            });
        }

        public PlaceableObject(Dictionary<string, Int32> fields, BinaryReader r)
        {
            m_fields = fields;
            m_values = new Dictionary<string, List<byte>>();

            m_fields.ToList().ForEach(kv =>
            {
                List<byte> d;
                switch (kv.Value)
                {
                    case 1:
                        d = new List<byte>(1)
                        {
                            r.ReadByte()
                        };

                        m_values.Add(kv.Key, d);
                        break;
                    default:
                        d = new List<byte>(kv.Value);
                        d.AddRange(r.ReadBytes(kv.Value));

                        m_values.Add(kv.Key, d);
                        break;
                }
            });

            int nameLen = BinaryPrimitives.ReadInt32LittleEndian(m_values["SCRI"].ToArray());
            if (nameLen > 0)
            {
                m_name = new string(r.ReadChars(nameLen));
            }
            else
            {
                m_name = "";
            }
        }

        public byte[] ToBytes()
        {
            int contentSize = m_values.Sum(kv => kv.Value.Count);
            int size = contentSize + Name.Length; // strings are already nil terminated
            List<byte> content = new List<byte>(size);

            m_values.ToList().ForEach(kv => content.AddRange(kv.Value));

            // nil terminated name (ASCII)
            if (Name.Length > 0)
            {
                byte[] b = UTF8Encoding.UTF8.GetBytes(Name);

                content.AddRange(b);
            }

            return content.ToArray();
        }

        public string[] ToFormattedDescription()
        {
            return ToFormattedDescription(null);
        }

        public string[] ToFormattedDescription(OLAYObject obj)
        {
            List<string> lines = new List<string> {
                string.Format("Script Name:\t{0}", GetFormattedName())
            };

            if (obj != null)
            {
                lines.Add(string.Empty);
                lines.AddRange(obj.ToFormattedPreview());
                lines.Add(string.Empty);
            }

            m_fields.ToList().ForEach(kv =>
            {
                lines.Add(string.Format("{0}:\t{1}", kv.Key, GetFieldValue(kv.Key)));
            });

            return lines.ToArray();
        }

        private void SetFieldValue(string key, int value)
        {
            if (!m_fields.ContainsKey(key)) return;
            Span<byte> buff = stackalloc byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(buff, value);

            m_values[key] = buff.ToArray().ToList();
        }

        private void SetByteFieldValue(string key, byte value)
        {
            if (!m_fields.ContainsKey(key)) return;
            m_values[key][0] = value;
        }

        private byte GetByteFieldValue(string key)
        {
            if (!m_fields.ContainsKey(key)) return 0b0;
            return m_values[key][0];
        }

        private int GetFieldValue(string key)
        {
            if (!m_fields.ContainsKey(key)) return 0;
            byte[] data = m_values[key].ToArray();

            if (data.Length == 1)
            {
                return Convert.ToInt32(data[0]);
            }
            else
            {
                return BinaryPrimitives.ReadInt32LittleEndian(m_values[key].ToArray());
            }
        }

        private string GetFormattedName()
        {
            char[] cstring = Name.ToArray();

            if (cstring.Length > 0)
            {
                char[] str = new char[Name.Length - 1];
                Array.Copy(cstring, str, Name.Length - 1);

                return new string(str);
            }
            else
            {
                return "(unnamed)";
            }
        }

        public override string ToString()
        {
            return "(defined)";
        }
    }
}
