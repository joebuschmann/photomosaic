using System.Drawing;
using NUnit.Framework;

namespace PhotoMosaic.Tests
{
    [TestFixture]
    public class BitmapExtensionsTests
    {
        private Bitmap _bitmap;

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
                    _bitmap.SetPixel(x, y, Color.FromArgb(color));
                    color++;
                }
            }
        }

        [Test]
        public void ValidateToArray()
        {
            Color[] colors = _bitmap.ToArray();

            for (int i = 0; i < colors.Length; i++)
            {
                Assert.That(colors[i].ToArgb(), Is.EqualTo(i));
            }
        }

        [Test]
        public void ValidateToArrayWithRect()
        {
            Color[] colors = _bitmap.ToArray(new Rectangle(4, 2, 2, 3));

            Assert.That(colors.Length, Is.EqualTo(6));

            Assert.That(colors[0].ToArgb(), Is.EqualTo(24));
            Assert.That(colors[1].ToArgb(), Is.EqualTo(25));
            Assert.That(colors[2].ToArgb(), Is.EqualTo(34));
            Assert.That(colors[3].ToArgb(), Is.EqualTo(35));
            Assert.That(colors[4].ToArgb(), Is.EqualTo(44));
            Assert.That(colors[5].ToArgb(), Is.EqualTo(45));
        }
    }
}
