using System;
using System.Drawing;

namespace PhotoMosaic.App
{
    internal class PhotoMosaicLib
    {
        public class Bin
        {
            private double _runningTotal = 0;

            public Bin()
            {
                Count = 0;
            }

            public int Count { get; private set; }

            public int Average {
                get
                {
                    if (Count == 0)
                    {
                        throw new Exception("Count is zero.");
                    }

                    return (int) Math.Round(_runningTotal / Count);
                }
            }

            public void Add(byte colorComponent)
            {
                _runningTotal += colorComponent;
                Count++;
            }
        }

        public Color CalculateMostFrequentColor(Bitmap image)
        {
            return CalculateMostFrequentColor(image, 10);
        }

        public Color CalculateMostFrequentColor(Bitmap image, byte binSize)
        {
            int binCount = (int)Math.Ceiling(Convert.ToDouble(byte.MaxValue) / binSize);
            int GetBin(byte bite) => Math.Max((int) Math.Ceiling(Convert.ToDouble(bite) / binSize) - 1, 0);
            Bin[] a = new Bin[binCount];
            Bin[] r = new Bin[binCount];
            Bin[] g = new Bin[binCount];
            Bin[] b = new Bin[binCount];

            for (int i = 0; i < binCount; i++)
            {
                a[i] = new Bin();
                r[i] = new Bin();
                g[i] = new Bin();
                b[i] = new Bin();
            }

            foreach (var color in image.ToArray())
            {
                var aIndex = GetBin(color.A);
                var rIndex = GetBin(color.R);
                var gIndex = GetBin(color.G);
                var bIndex = GetBin(color.B);

                a[aIndex].Add(color.A);
                r[rIndex].Add(color.R);
                g[gIndex].Add(color.G);
                b[bIndex].Add(color.B);
            }

            int aMaxIndex = 0;
            int rMaxIndex = 0;
            int gMaxIndex = 0;
            int bMaxIndex = 0;

            for (int i = 0; i < binCount; i++)
            {
                if (a[i].Count > a[aMaxIndex].Count)
                {
                    aMaxIndex = i;
                }

                if (r[i].Count > r[rMaxIndex].Count)
                {
                    rMaxIndex = i;
                }

                if (g[i].Count > g[gMaxIndex].Count)
                {
                    gMaxIndex = i;
                }

                if (b[i].Count > b[bMaxIndex].Count)
                {
                    bMaxIndex = i;
                }
            }

            return Color.FromArgb(
                a[aMaxIndex].Average,
                r[rMaxIndex].Average,
                g[gMaxIndex].Average,
                b[bMaxIndex].Average);
        }

        public Color CalculateAverageColor(Bitmap image)
        {
            return CalculateAverageColor(image, new Size(10, 10));
        }

        public Color CalculateAverageColor(Bitmap image, Size chunkSize)
        {
            int currentX = 0, currentY = 0;
            double pixelCount = (image.Height * image.Width);
            double avgR = 0d, avgG = 0d, avgB = 0d, avgA = 0d;

            while (currentY < image.Height)
            {
                long r = 0, g = 0, b = 0, a = 0;
                var pixels = image.ToArray(new Rectangle(new Point(currentX, currentY), chunkSize));

                foreach (var pixel in pixels)
                {
                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;
                    a += pixel.A;
                }

                avgR += r / pixelCount;
                avgG += g / pixelCount;
                avgB += b / pixelCount;
                avgA += a / pixelCount;

                currentX += chunkSize.Width;

                if (currentX >= image.Width)
                {
                    currentX = 0;
                    currentY += chunkSize.Height;
                }
            }

            return Color.FromArgb(
                (int) Math.Round(avgA),
                (int) Math.Round(avgR),
                (int) Math.Round(avgG),
                (int) Math.Round(avgB));
        }

        public Image GetCenteredThumbnail(Bitmap image, Size thumbnailSize)
        {
            int squareLength = Math.Min(image.Width, image.Height);
            Size centeredSize = new Size(squareLength, squareLength);

            using (var centeredImage = ExtractCenteredImage(image, centeredSize))
            {
                return centeredImage.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, () => false, IntPtr.Zero);
            }
        }

        public Bitmap ExtractCenteredImage(Bitmap image, Size size)
        {
            if (image.Width == image.Height)
            {
                return image;
            }

            Point point;

            if (image.Height > image.Width)
            {
                // Center vertically
                int top = (image.Height - image.Width) / 2;
                point = new Point(0, top);
            }
            else
            {
                // Center horizontally
                int left = (image.Width - image.Height) / 2;
                point = new Point(left, 0);
            }

            Bitmap centeredImage = new Bitmap(size.Width, size.Height);

            using (Graphics graphics = Graphics.FromImage(centeredImage))
            {
                var destRect = new Rectangle(new Point(0, 0), size);
                var srcRect = new Rectangle(point, size);
                graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                graphics.Save();
                graphics.Dispose();
                return centeredImage;
            }
        }
    }
}
