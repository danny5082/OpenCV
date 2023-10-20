using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;

namespace OpenCVApp25
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Mat m1 = Cv2.ImRead("C:\\Temp\\img\\boat.jpg", ImreadModes.Unchanged);
			Bitmap imageBitmap = m1.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Mat m1 = Cv2.ImRead("C:\\Temp\\img\\boat.jpg", ImreadModes.Unchanged);
			Bitmap imageBitmap = m1.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\boat.jpg");
			Mat x_axis = new Mat();
			Cv2.Flip(src, x_axis, FlipMode.X);
			Cv2.WaitKey(0);
			Bitmap imageBitmap = x_axis.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\boat.jpg");
			Mat y_axis = new Mat();
			Cv2.Flip(src, y_axis, FlipMode.Y);
			Cv2.WaitKey(0);
			Bitmap imageBitmap = y_axis.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\boat.jpg");
			Mat xy_axis = new Mat();
			Cv2.Flip(src, xy_axis, FlipMode.XY);
			Cv2.WaitKey(0);
			Bitmap imageBitmap = xy_axis.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

		}

		private void button6_Click(object sender, EventArgs e)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\boat.jpg");
			Mat rep_img = new Mat();
			Cv2.Repeat(src, 1, 2, rep_img);
			Cv2.WaitKey(0);
			Bitmap imageBitmap = rep_img.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\boat.jpg");
			Mat trans_img = new Mat();
			Cv2.Transpose(src, trans_img);
			Cv2.WaitKey(0);
			Bitmap imageBitmap = trans_img.ToBitmap();
			pictureBox1.Image = imageBitmap;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}
	}
}