using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        [Test]
        public void ValidateIterateBitmapByBlocks()
        {
            // Arrange
            var expectedRectangles = new List<Rectangle>
            {
                new Rectangle(0, 0, 3, 3),
                new Rectangle(3, 0, 3, 3),
                new Rectangle(6, 0, 3, 3),
                new Rectangle(9, 0, 1, 3),
                new Rectangle(0, 3, 3, 3),
                new Rectangle(3, 3, 3, 3),
                new Rectangle(6, 3, 3, 3),
                new Rectangle(9, 3, 1, 3),
                new Rectangle(0, 6, 3, 3),
                new Rectangle(3, 6, 3, 3),
                new Rectangle(6, 6, 3, 3),
                new Rectangle(9, 6, 1, 3),
                new Rectangle(0, 9, 3, 1),
                new Rectangle(3, 9, 3, 1),
                new Rectangle(6, 9, 3, 1),
                new Rectangle(9, 9, 1, 1)
            };

            var actualRectangles = new List<Rectangle>();

            var expectedPixels = GetExpectedColorsForIterateBitmapByBlocks();
            var actualPixels = new List<int[]>();

            // Act
            _bitmap.IterateBitmapByBlocks(new Size(3, 3), (r, pixels) =>
            {
                actualRectangles.Add(r);
                actualPixels.Add(pixels.Select(c => c.ToArgb()).ToArray());
            });

            // Assert
            Assert.AreEqual(expectedRectangles.Count, actualRectangles.Count);

            for (int i = 0; i < expectedRectangles.Count; i++)
            {
                Assert.AreEqual(expectedRectangles[i], actualRectangles[i]);
            }

            Assert.AreEqual(expectedPixels.Count, actualPixels.Count);

            for (int i = 0; i < expectedPixels.Count; i++)
            {
                Assert.AreEqual(expectedPixels[i], actualPixels[i]);
            }
        }

        private List<int[]> GetExpectedColorsForIterateBitmapByBlocks()
        {
            return new List<int[]>
            {
                new int[] {0, 1, 2, 10, 11, 12, 20, 21, 22},
                new int[] {3, 4, 5, 13, 14, 15, 23, 24, 25},
                new int[] {6, 7, 8, 16, 17, 18, 26, 27, 28},
                new int[] {9, 19, 29},
                new int[] {30, 31, 32, 40, 41, 42, 50, 51, 52},
                new int[] {33, 34, 35, 43, 44, 45, 53, 54, 55},
                new int[] {36, 37, 38, 46, 47, 48, 56, 57, 58},
                new int[] {39, 49, 59},
                new int[] {60, 61, 62, 70, 71, 72, 80, 81, 82},
                new int[] {63, 64, 65, 73, 74, 75, 83, 84, 85},
                new int[] {66, 67, 68, 76, 77, 78, 86, 87, 88},
                new int[] {69, 79, 89},
                new int[] {90, 91, 92},
                new int[] {93, 94, 95},
                new int[] {96, 97, 98},
                new int[] {99}
            };
        }
    }
}
