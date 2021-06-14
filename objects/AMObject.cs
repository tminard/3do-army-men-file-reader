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

        [Category("Class"), Description("The encoded type and instance as a single integer.")]
        public int EncodedTypeKey { get; private set; }

        public string LabelText { get; }

        public Bitmap SpriteImage { get; private set; }

        public Bitmap ShadowImage { get; private set; }

        public AMObject(UInt32 encodedKey, UInt32 typeKey, UInt32 instanceKey, Bitmap spriteImage, Bitmap shadowImage = null)
        {
            EncodedTypeKey = Convert.ToInt32(encodedKey);
            TypeKey = Convert.ToInt32(typeKey);
            InstanceKey = Convert.ToInt32(instanceKey);
            SpriteImage = spriteImage;
            ShadowImage = shadowImage;
            LabelText = "Type: " + typeKey + " Instance: " + instanceKey;
        }
    }
}
