using System.Drawing;

namespace PhotoMosaic.App
{
    public class Block<T>
    {
        public Block(Rectangle rect, T data)
        {
            Rect = rect;
            Data = data;
        }

        public Rectangle Rect { get; }
        public T Data { get; }
    }
}
