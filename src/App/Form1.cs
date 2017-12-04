using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhotoMosaic.App
{
    public partial class Form1 : Form
    {
        private readonly Color _defaultPanelColor;
        private readonly PhotoMosaicLib _photoMosaicLib;

        private Bitmap _targetImage = null;
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
                if (_image != null)
                {
                    panelColor.BackColor = _defaultPanelColor;
                    pictureBox1.Image = null;
                    _image.Dispose();
                }

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

            string[] allFiles = Directory
                .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(p =>
                {
                    var extension = Path.GetExtension(p);
                    return extension == ".png" || extension == ".jpg";
                })
                .ToArray();

            var processorCount = Environment.ProcessorCount;

            for (int i = 0; i < allFiles.Length; i += processorCount)
            {
                string[] currentFiles = new string[processorCount];
                Array.Copy(allFiles, i, currentFiles, 0, processorCount);
                CreateAndSaveThumbnails(currentFiles, destFolderPath);
            }
        }

        private void CreateAndSaveThumbnails(string[] currentFiles, string destFolderPath)
        {
            ImageInfo[] images = currentFiles
                .AsParallel()
                .Select(src => CreateThumbnail(src, destFolderPath))
                .ToArray();

            foreach (var image in images)
            {
                image.Image.Save(image.Path);
            }

            DataAccess dataAccess = new DataAccess(destFolderPath);
            dataAccess.SaveSourceImageInfo(images);
        }

        private ImageInfo CreateThumbnail(string path, string destFolderPath)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }

            Size size = new Size(50, 50);

            using (Bitmap image = new Bitmap(Image.FromFile(path)))
            {
                var thumbnail = _photoMosaicLib.GetCenteredThumbnail(image, size);
                Color averageColor = _photoMosaicLib.CalculateAverageColor(new Bitmap(thumbnail));
                return new ImageInfo(path, thumbnail, averageColor, destFolderPath);
            }
        }

        private void buttonChooseTargetImage_Click(object sender, EventArgs e)
        {
            if (openImageFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (_targetImage != null)
                {
                    _targetImage.Dispose();
                }

                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (var fileStream = openImageFileDialog.OpenFile())
                    using (var dataAccess = new DataAccess(folderBrowserDialog.SelectedPath))
                    {
                        string filename = openImageFileDialog.FileName;
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, buffer.Length);
                        long imageId = dataAccess.SaveTargetImage(filename, buffer);

                        List<Block<Color>> blocks = new List<Block<Color>>();

                        using (var targetImage = new Bitmap(fileStream))
                        {
                            _photoMosaicLib.CalculateAverageColorByBlock(targetImage, new Size(50, 50),
                                (rect, pixel) =>
                                {
                                    blocks.Add(new Block<Color>(rect, pixel));
                                });
                        }

                        dataAccess.SaveTargetImageBlocks(imageId, blocks);
                    }
                }
            }
        }
    }
}
