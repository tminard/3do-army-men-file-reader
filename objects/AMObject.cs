using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.objects
{
    public class AMObject
    {
        public int TypeKey { get; private set; }
        public int InstanceKey { get; private set; }
        public Bitmap SpriteImage { get; private set; }

        public Bitmap ShadowImage { get; private set; }

        public AMObject(UInt32 typeKey, UInt32 instanceKey, Bitmap spriteImage, Bitmap shadowImage = null)
        {
            TypeKey = Convert.ToInt32(typeKey);
            InstanceKey = Convert.ToInt32(instanceKey);
            SpriteImage = spriteImage;
            ShadowImage = shadowImage;
        }
    }
}
