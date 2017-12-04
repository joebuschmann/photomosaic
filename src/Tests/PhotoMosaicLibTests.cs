using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using PhotoMosaic.App;

namespace PhotoMosaic.Tests
{
    [TestFixture]
    public class PhotoMosaicLibTests
    {
        private Bitmap _bitmap;
        private readonly PhotoMosaicLib _photoMosaicLib = new PhotoMosaicLib();

        [SetUp]
        public void CreateTestBitmap()
        {
            _bitmap = new Bitmap(10, 10);
            int color = 0;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    // Assign ARGB values based on color integer.
                    // Stagger the values by 10s to vary the average.
                    int a = color, r = color + 10, g = color + 20, b = color + 30;
                    _bitmap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    color++;
                }
            }
        }

        [Test]
        public void CalculateAverageColorByBlock()
        {
            var expectedPixels = GetExpectedAvgColorByBlock();
            var actualPixels = new List<Color>();

            _photoMosaicLib.CalculateAverageColorByBlock(_bitmap, new Size(3, 3), (rect, pixel) =>
            {
                actualPixels.Add(pixel);
            });

            Assert.AreEqual(expectedPixels.Count, actualPixels.Count);

            for (int i = 0; i < expectedPixels.Count; i++)
            {
                Assert.AreEqual(expectedPixels[i], actualPixels[i]);
            }
        }

        [Test]
        public void CalculateAverageColor()
        {
            Color color = _photoMosaicLib.CalculateAverageColor(_bitmap, new Size(3, 3));

            Assert.AreEqual(49, color.A);
            Assert.AreEqual(60, color.R);
            Assert.AreEqual(70, color.G);
            Assert.AreEqual(80, color.B);
        }

        [Test]
        public void CalculateAverageSolidColor()
        {
            Bitmap bitmap = new Bitmap(10, 10);
            var expectedColor = Color.Chocolate;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    bitmap.SetPixel(i, j, expectedColor);
                }
            }

            Color color = _photoMosaicLib.CalculateAverageColor(bitmap);

            Assert.That(color.A, Is.EqualTo(expectedColor.A));
            Assert.That(color.B, Is.EqualTo(expectedColor.B));
            Assert.That(color.G, Is.EqualTo(expectedColor.G));
            Assert.That(color.R, Is.EqualTo(expectedColor.R));
        }

        [Test]
        public void ExtractCenteredImageFromWideImage()
        {
            Bitmap bitmap = new Bitmap(10, 3);
            int color = 0;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 10; x++)
                    {
                    bitmap.SetPixel(x, y, Color.FromArgb(0, 0, color));
                    color++;
                }
            }

            Bitmap centeredImage = _photoMosaicLib.ExtractCenteredImage(bitmap, new Size(3, 3));

            Assert.AreEqual(3, centeredImage.GetPixel(0, 0).B);
            Assert.AreEqual(4, centeredImage.GetPixel(1, 0).B);
            Assert.AreEqual(5, centeredImage.GetPixel(2, 0).B);

            Assert.AreEqual(13, centeredImage.GetPixel(0, 1).B);
            Assert.AreEqual(14, centeredImage.GetPixel(1, 1).B);
            Assert.AreEqual(15, centeredImage.GetPixel(2, 1).B);

            Assert.AreEqual(23, centeredImage.GetPixel(0, 2).B);
            Assert.AreEqual(24, centeredImage.GetPixel(1, 2).B);
            Assert.AreEqual(25, centeredImage.GetPixel(2, 2).B);
        }

        [Test]
        public void ExtractCenteredImageFromTallImage()
        {
            Bitmap bitmap = new Bitmap(3, 10);
            int color = 0;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(0, 0, color));
                    color++;
                }
            }

            Bitmap centeredImage = _photoMosaicLib.ExtractCenteredImage(bitmap, new Size(3, 3));

            Assert.AreEqual(9, centeredImage.GetPixel(0, 0).B);
            Assert.AreEqual(10, centeredImage.GetPixel(1, 0).B);
            Assert.AreEqual(11, centeredImage.GetPixel(2, 0).B);

            Assert.AreEqual(12, centeredImage.GetPixel(0, 1).B);
            Assert.AreEqual(13, centeredImage.GetPixel(1, 1).B);
            Assert.AreEqual(14, centeredImage.GetPixel(2, 1).B);

            Assert.AreEqual(15, centeredImage.GetPixel(0, 2).B);
            Assert.AreEqual(16, centeredImage.GetPixel(1, 2).B);
            Assert.AreEqual(17, centeredImage.GetPixel(2, 2).B);
        }

        [Test]
        public void CreateThumbnail()
        {
            Bitmap bitmap = new Bitmap(3, 10);
            int color = 0;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(0, 0, color));
                    color++;
                }
            }

            Size size = new Size(3, 3);
            Bitmap thumbnailImage = new Bitmap(_photoMosaicLib.GetCenteredThumbnail(bitmap, size));
            
            Assert.AreEqual(size, thumbnailImage.Size);

            Assert.AreNotEqual(0, thumbnailImage.GetPixel(0, 0).ToArgb());
            Assert.AreNotEqual(0, thumbnailImage.GetPixel(1, 0).ToArgb());
            Assert.AreNotEqual(0, thumbnailImage.GetPixel(2, 0).ToArgb());

            Assert.AreNotEqual(0, thumbnailImage.GetPixel(0, 1).ToArgb());
            Assert.AreNotEqual(0, thumbnailImage.GetPixel(1, 1).ToArgb());
            Assert.AreNotEqual(0, thumbnailImage.GetPixel(2, 1).ToArgb());

            Assert.AreNotEqual(0, thumbnailImage.GetPixel(0, 2).ToArgb());
            Assert.AreNotEqual(0, thumbnailImage.GetPixel(1, 2).ToArgb());
            Assert.AreNotEqual(0, thumbnailImage.GetPixel(2, 2).ToArgb());
        }

        private List<Color> GetExpectedAvgColorByBlock()
        {
            return new List<Color>
            {
                Color.FromArgb(11, 21, 31, 41),
                Color.FromArgb(14, 24, 34, 44),
                Color.FromArgb(17, 27, 37, 47),
                Color.FromArgb(19, 29, 39, 49),
                Color.FromArgb(41, 51, 61, 71),
                Color.FromArgb(44, 54, 64, 74),
                Color.FromArgb(47, 57, 67, 77),
                Color.FromArgb(49, 59, 69, 79),
                Color.FromArgb(71, 81, 91, 101),
                Color.FromArgb(74, 84, 94, 104),
                Color.FromArgb(77, 87, 97, 107),
                Color.FromArgb(79, 89, 99, 109),
                Color.FromArgb(91, 101, 111, 121),
                Color.FromArgb(94, 104, 114, 124),
                Color.FromArgb(97, 107, 117, 127),
                Color.FromArgb(99, 109, 119, 129)
            };
        }
    }
}
