// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    public static class BitmapExtensions
    {
        public static Color[] ToArray(this Bitmap image)
        {
            Color[] imageAsArray = new Color[image.Width * image.Height];
            int idx = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    imageAsArray[idx] = image.GetPixel(x, y);
                    idx++;
                }
            }

            return imageAsArray;
        }

        public static Color[] ToArray(this Bitmap image, Rectangle rect)
        {
            Color[] imageAsArray = new Color[rect.Width * rect.Height];
            int idx = 0;

            for (int y = rect.Y; y < rect.Bottom && y < image.Height; y++)
            {
                for (int x = rect.X; x < rect.Right && x < image.Width; x++)
                {
                    imageAsArray[idx] = image.GetPixel(x, y);
                    idx++;
                }
            }

            return imageAsArray;
        }
    }
}