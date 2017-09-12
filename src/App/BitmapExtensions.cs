// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    public static class BitmapExtensions
    {
        public static int[] ToArray(this Bitmap image)
        {
            int[] imageAsArray = new int[image.Width * image.Height];
            int idx = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    imageAsArray[idx] = image.GetPixel(x, y).ToArgb();
                    idx++;
                }
            }

            return imageAsArray;
        }

        public static int[] ToArray(this Bitmap image, Rectangle rect)
        {
            int[] imageAsArray = new int[rect.Width * rect.Height];
            int idx = 0;

            for (int y = rect.Y; y < rect.Bottom && y < image.Height; y++)
            {
                for (int x = rect.X; x < rect.Right && x < image.Width; x++)
                {
                    imageAsArray[idx] = image.GetPixel(x, y).ToArgb();
                    idx++;
                }
            }

            return imageAsArray;
        }
    }
}