using System;
using System.Drawing;
using System.Windows.Forms;
using PhotoMosaic.App;

namespace App
{
    public partial class Form1 : Form
    {
        private Bitmap _image = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                _image = new Bitmap(openFileDialog1.OpenFile());
                pictureBox1.Image = _image;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PhotoMosaicLib photoMosaicLib = new PhotoMosaicLib();
            int color = photoMosaicLib.CalculateAverageColor(_image);
            panelColor.BackColor = Color.FromArgb(color);
        }
    }
}
