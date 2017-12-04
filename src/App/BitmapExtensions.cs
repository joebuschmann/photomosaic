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

        public static void IterateBitmapByBlocks(this Bitmap image, Size blockSize, Action<Rectangle, Color[]> action)
        {
            if (action == null)
                return;

            int currentX = 0, currentY = 0;

            while (currentY < image.Height)
            {
                var width = currentX + blockSize.Width > image.Width ? image.Width - currentX : blockSize.Width;
                var height = currentY + blockSize.Height > image.Height ? image.Height - currentY : blockSize.Height;
                var rectangle = new Rectangle(currentX, currentY, width, height);
                var pixels = image.ToArray(rectangle);

                action(rectangle, pixels);

                currentX += blockSize.Width;

                if (currentX >= image.Width)
                {
                    currentX = 0;
                    currentY += blockSize.Height;
                }
            }
        }
    }
}