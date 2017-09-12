using System;
using System.Drawing;
using System.Linq;

namespace PhotoMosaic.App
{
    internal class PhotoMosaicLib
    {
//        public int CalculateMostFrequentColor(Bitmap image, byte swagA, byte swagR, byte swagG, byte swagB)
//        {
//            
//        }

        public int CalculateAverageColor(Bitmap image)
        {
            return CalculateAverageColor(image, new Size(10, 10));
        }

        public int CalculateAverageColor(Bitmap image, Size chunkSize)
        {
            int currentX = 0, currentY = 0;
            double average = 0d, pixelCount = (image.Height * image.Width);

            while (currentY < image.Height)
            {
                long total = image.ToArray(new Rectangle(new Point(currentX, currentY), chunkSize)).Sum();

                average += total / pixelCount;
                currentX += chunkSize.Width;

                if (currentX >= image.Width)
                {
                    currentX = 0;
                    currentY += chunkSize.Height;
                }
            }

            return (int)Math.Round(average);
        }
    }
}
