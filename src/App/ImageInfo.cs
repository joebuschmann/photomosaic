using System;
using System.Drawing;
using System.IO;

namespace PhotoMosaic.App
{
    public class ImageInfo
    {
        private readonly Guid _id = Guid.NewGuid();

        public ImageInfo(string path, Image image, Color color, string destFolderPath)
        {
            Image = image;
            Color = color;

            FileInfo fileInfo = new FileInfo(path);
            Extension = fileInfo.Extension;
            Path = $"{destFolderPath}\\{_id.ToString()}{fileInfo.Extension}";
        }
            
        public Guid Id {
            get { return _id; }
        }
        public Image Image { get; private set; }
        public Color Color { get; private set; }
        public string Extension { get; private set; }
        public string Path { get; private set; }
    }
}
