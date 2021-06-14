using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.axs
{
    public partial class AxsFile
    {
        public class FrameData
        {
            private UInt32 m_x;
            private UInt32 m_y;

            public FrameData(uint x, uint y)
            {
                X = x;
                Y = y;
            }

            public uint X { get => m_x; private set => m_x = value; }
            public uint Y { get => m_y; private set => m_y = value; }
        }
    }
}
