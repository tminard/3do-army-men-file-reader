using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AMMEdit.axs
{
    public partial class AxsFile
    {
        public class Animation
        {
            private List<FrameImageData> m_frames = new List<FrameImageData>();
            private AnimationSequence m_animation;

            private bool m_show_center_point = true;

            private bool m_show_offset_point = true;

            public Animation(AnimationSequence sequence, List<FrameImageData> srcImages)
            {
                AnimationData = sequence;
                for (uint i = 0; i < AnimationData.Frame_indices.Length; i++)
                {
                    FrameImageData srcImage = srcImages[Convert.ToInt32(AnimationData.Frame_indices[i])];

                    Frames.Add(srcImage);
                }
            }

            public List<FrameImageData> Frames { get => m_frames; set => m_frames = value; }

            public List<Bitmap> Images { get => m_frames.Select(f => f.Sprite_image).ToList(); }
            public AnimationSequence AnimationData { get => m_animation; set => m_animation = value; }

            public String Name { get => m_animation.AnimationName; }

            public bool ShowCenterPoint { get => m_show_center_point; set => m_show_center_point = value; }

            public bool ShowOffsetPoint { get => m_show_offset_point; set => m_show_offset_point = value; }
        }
    }
}
