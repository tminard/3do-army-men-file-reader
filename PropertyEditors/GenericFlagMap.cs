using AMMEdit.amm.blocks.subfields;
using System;
using System.Collections.Generic;
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

            GenerateOverlay();
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
                throw new Exception("Requested map position greater than map size");
            }
        }

        private void GenerateOverlay()
        {
            Bitmap renderedMap = new Bitmap(256 * 16, 256 * 16, PixelFormat.Format32bppArgb);
            BufferedGraphicsContext currentContext;
            BufferedGraphics mapBuffer;
            currentContext = BufferedGraphicsManager.Current;

            mapBuffer = currentContext.Allocate(Graphics.FromImage(renderedMap), new Rectangle(0, 0, 256 * 16, 256 * 16));

            byte maxVal = ToBytes().Max();
            double factor = 0;
            if (maxVal > 0)
            {
                factor = 255.0 / Convert.ToDouble(Convert.ToInt32(maxVal) * 16);
            }

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (x >= 256 || y >= 256) continue;

                    UInt16 flagID = GetFlagAtLocation(x, y).Flag;
                    int flagValueScaled = Convert.ToInt32(Math.Round(Convert.ToDouble((flagID * 16)) * factor));

                    int posX = x * 16;
                    int posY = y * 16;

                    mapBuffer.Graphics.FillRectangle(
                        new SolidBrush(Color.FromArgb(255, flagValueScaled, flagValueScaled, flagValueScaled)),
                        new Rectangle(new Point(posX, posY), new Size(16, 16))
                    );
                }
            }

            mapBuffer.Render();
            mapBuffer.Dispose();

            Overlay = (Bitmap)renderedMap.Clone();

            renderedMap.Dispose();
        }

        internal byte[] ToBytes()
        {
            return RawContent.ConvertAll(f => f.Flag).ToArray();
        }
    }
}
