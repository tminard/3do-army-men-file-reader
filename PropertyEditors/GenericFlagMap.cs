using AMMEdit.amm.blocks.subfields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace AMMEdit.PropertyEditors
{
    public class GenericFlagMap
    {
        public GenericFlagMap(List<byte> rawContent, int width, int height)
        {
            RawContent = new List<MapFlag>(rawContent.Count);
            RawContent.AddRange(rawContent.ConvertAll(flag => new MapFlag(flag)));
            Width = width;
            Height = height;
        }

        List<MapFlag> RawContent { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Bitmap Overlay { get; set; }

        public MapFlag GetFlagAtLocation(int x, int y)
        {
            int pos = (Width * y) + x;

            if (pos < RawContent.Count && pos >= 0 && x < Width)
            {
                return RawContent[pos];
            }
            else
            {
                Debug.WriteLine("Requested map position greater than map size");
                return new MapFlag(0b0);
            }
        }

        public void GenerateOverlay(Bitmap completeMap)
        {
            if (Width * Height > 2048 * 2048)
            {
                throw new Exception(String.Format("Image size {0}x{1} too large", Width, Height));
            }

            Bitmap renderedMap = new Bitmap(Width * 16, Height * 16);
            BufferedGraphicsContext currentContext;
            BufferedGraphics mapBuffer;
            currentContext = BufferedGraphicsManager.Current;

            mapBuffer = currentContext.Allocate(Graphics.FromImage(renderedMap), new Rectangle(0, 0, Width * 16, Height * 16));

            mapBuffer.Graphics.DrawImage(completeMap, new Point(0, 0));

            byte maxVal = ToBytes().Max();
            double factor = 0;
            if (maxVal > 0)
            {
                factor = 255.0 / Convert.ToDouble(Convert.ToInt32(maxVal) * 16);
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x >= Width || y >= Height) continue;

                    UInt16 flagID = GetFlagAtLocation(x, y).Flag;
                    int flagValueScaled = Convert.ToInt32(Math.Round(Convert.ToDouble((flagID * 16)) * factor));

                    int posX = x * 16;
                    int posY = y * 16;

                    mapBuffer.Graphics.FillRectangle(
                        new SolidBrush(Color.FromArgb(200, flagValueScaled, flagValueScaled, flagValueScaled)),
                        new Rectangle(new Point(posX, posY), new Size(16, 16))
                    );
                }
            }

            mapBuffer.Render();
            mapBuffer.Dispose();

            if (Overlay != null)
            {
                Overlay.Dispose();
            }

            Overlay = (Bitmap)renderedMap.Clone();

            renderedMap.Dispose();
        }

        internal byte[] ToBytes()
        {
            return RawContent.ConvertAll(f => f.Flag).ToArray();
        }
    }
}
