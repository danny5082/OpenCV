using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;


namespace OpevCVApp25
{
	public partial class Form1 : Form
	{
		private Mat loadedImage;

		public Form1()
		{
			InitializeComponent();
		}

		private void LoadImage(string imagePath)
		{
			loadedImage = Cv2.ImRead(imagePath, ImreadModes.Unchanged);
			Bitmap imageBitmap = loadedImage.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			LoadImage("C:\\Temp\\img\\boat.jpg");
		}

		private void ProcessAndDisplayImage(Mat processedImage)
		{
			if (loadedImage != null)
			{
				Cv2.WaitKey(0);
				Bitmap imageBitmap = processedImage.ToBitmap();
				pictureBox1.Image = imageBitmap;
				pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Your image processing code here
			ProcessAndDisplayImage(loadedImage);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			// Your image processing code here
			Mat x_axis = new Mat();
			Cv2.Flip(loadedImage, x_axis, FlipMode.X);
			ProcessAndDisplayImage(x_axis);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			// Your image processing code here
			Mat y_axis = new Mat();
			Cv2.Flip(loadedImage, y_axis, FlipMode.Y);
			ProcessAndDisplayImage(y_axis);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			// Your image processing code here
			Mat xy_axis = new Mat();
			Cv2.Flip(loadedImage, xy_axis, FlipMode.XY);
			ProcessAndDisplayImage(xy_axis);
		}

		private void button6_Click(object sender, EventArgs e)
		{
			// Your image processing code here
			Mat rep_img = new Mat();
			Cv2.Repeat(loadedImage, 1, 2, rep_img);
			ProcessAndDisplayImage(rep_img);
		}

		private void button5_Click(object sender, EventArgs e)
		{
			// Your image processing code here
			Mat trans_img = new Mat();
			Cv2.Transpose(loadedImage, trans_img);
			ProcessAndDisplayImage(trans_img);
		}
	}
}
