using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PhotoMosaic.App;

namespace App
{
    public partial class Form1 : Form
    {
        private readonly Color _defaultPanelColor;
        private readonly PhotoMosaicLib _photoMosaicLib;

        private Bitmap _image = null;

        public Form1()
        {
            InitializeComponent();
            _defaultPanelColor = panelColor.BackColor;
            _photoMosaicLib = new PhotoMosaicLib();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openImageFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _image = new Bitmap(openImageFileDialog.OpenFile());
                pictureBox1.Image = _image;
                panelColor.BackColor = _defaultPanelColor;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelColor.BackColor = _photoMosaicLib.CalculateAverageColor(_image);
        }

        private void buttonAnalyzeFiles_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                AnalyzeFiles(folderBrowserDialog.SelectedPath);
            }
        }

        private void AnalyzeFiles(string folderPath)
        {
            
            string destFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                    "\\resized-images\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
            Directory.CreateDirectory(destFolderPath);

            string[] allFiles = Directory.EnumerateFiles(folderPath, "*.png", SearchOption.AllDirectories).ToArray();
//            var processorCount = Environment.ProcessorCount;
            var processorCount = 1;

            for (int i = 0; i < allFiles.Length; i += processorCount)
            {
                string[] currentFiles = new string[processorCount];
                Array.Copy(allFiles, i, currentFiles, 0, processorCount);
                CreateAndSaveThumbnails(currentFiles, destFolderPath);
            }

//            ImageInfo[] images = Directory
//                .EnumerateFiles(folderPath, "*.png", SearchOption.AllDirectories)
//                .AsParallel()
//                .Select(path => CreateThumbnail(path, size))
//                .ToArray();
//
//            foreach (var image in images)
//            {
//                image.Image.Save(destFolderPath + "\\" + image.Name);
//            }
        }

        private void CreateAndSaveThumbnails(string[] currentFiles, string destFolderPath)
        {
            ImageInfo[] images = currentFiles
//                .AsParallel()
                .Select(CreateThumbnail)
                .ToArray();
            
            foreach (var image in images)
            {
                image.Image.Save(destFolderPath + "\\" + image.Name);
            }
        }

        private ImageInfo CreateThumbnail(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }

            Size size = new Size(50, 50);

            using (Bitmap image = new Bitmap(Image.FromFile(path)))
            {
                var thumbnail = _photoMosaicLib.GetCenteredThumbnail(image, size);
                return new ImageInfo(path, thumbnail);
            }
        }

        private class ImageInfo
        {
            public ImageInfo(string path, Image image)
            {
                FileInfo fileInfo = new FileInfo(path);

                Name = fileInfo.Name;
                Image = image;
            }

            public string Name { get; private set; }
            public Image Image { get; private set; }
        }
    }
}
