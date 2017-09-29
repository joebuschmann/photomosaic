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
            /*
                    0  1  2  3  4  5  6  7  8  9
                    =  =  =  =  =  =  =  =  =  =
                 0| 0  1  2  3  4  5  6  7  8  9
                 1|10 11 12 13 14 15 16 17 18 19
                 2|20 21 22 23 24 25 26 27 28 29
                 3|30 31 32 33 34 35 36 37 38 39
                 4|40 41 42 43 44 45 46 47 48 49
                 5|50 51 52 53 54 55 56 57 58 59
                 6|60 61 62 63 64 65 66 67 68 69
                 7|70 71 72 73 74 75 76 77 78 79
                 8|80 81 82 83 84 85 86 87 88 89
                 9|90 91 92 93 94 95 96 97 98 99
            */

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
    }
}
