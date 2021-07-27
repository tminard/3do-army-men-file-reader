using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.objects
{
    public class AMObject
    {
        [Category("Class"), Description("The object type enumeration.")]
        public int TypeKey { get; private set; }

        [Category("Class"), Description("The object type instance. Each type can have multiple instances.")]
        public int InstanceKey { get; private set; }

        [Category("Class"), Description("The frame number for the instance. Used for destruction rendering.")]
        public int InstanceSequence { get; private set; }

        [Category("Class"), Description("The encoded type and instance as a single integer.")]
        public int EncodedTypeKey { get; private set; }

        public string LabelText { get; }

        public string ClassificationName { get; private set; }

        public bool Placeable { get; private set; }

        public Bitmap SpriteImage { get; private set; }

        public Bitmap ShadowImage { get; private set; }

        public AMObject(UInt32 encodedKey, UInt32 typeKey, UInt32 instanceKey, Bitmap spriteImage, int instanceSequence, Bitmap shadowImage = null)
        {
            EncodedTypeKey = Convert.ToInt32(encodedKey);
            TypeKey = Convert.ToInt32(typeKey);
            InstanceKey = Convert.ToInt32(instanceKey);
            InstanceSequence = instanceSequence;
            SpriteImage = spriteImage;
            ShadowImage = shadowImage;
            ClassificationName = GetNameForTypeKey(typeKey);
            Placeable = (ClassificationName != "Unknown");
            LabelText = ClassificationName + " (" + instanceKey + ")";
        }

        private String GetNameForTypeKey(UInt32 typeKey)
        {
            switch (typeKey)
            {
                case 0:
                    return "Rock";
                case 1:
                    return "Bush";
                case 2:
                    return "Tree (small)";
                case 3:
                    return "Tree (large)";
                case 4:
                    return "Fallen tree";
                case 5:
                    return "Fence";
                case 6:
                    return "Wall";
                case 7:
                    return "Bridge";
                case 8:
                    return "Footbridge";
                case 9:
                    return "Explosive";
                case 10:
                    return "Gun turret";
                case 11:
                    return "AA gun";
                case 12:
                    return "Base";
                case 13:
                    return "Aux base";
                case 14:
                    return "Airstrip";
                case 15:
                    return "Helipad";
                case 16:
                    return "Prison camp";
                case 17:
                    return "Building";
                case 18:
                    return "Pillbox";
                case 19:
                    return "Powerplant";
                case 20:
                    return "Gate";
                case 21:
                    return "Wall gate";
                case 22:
                    return "Radar building";
                case 23:
                    return "Miscellaneous";
                case 24:
                    return "Overpass";
                case 25:
                    return "Special item";
                case 160:
                case 96:
                    return "Special building (AM2+)";
                default:
                    return "Unknown";
            }
        }
    }
}
