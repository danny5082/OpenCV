using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCVApp27
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Mat image1 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			OpenCvSharp.Point center = new OpenCvSharp.Point(image1.Width / 2, image1.Height / 2);
			Cv2.Circle(image1, center, 80, Scalar.All(255), -1);
			Bitmap bitmap1 = MatToBitmap(image1);
			pictureBox1.Image = bitmap1;
		}

		private Bitmap MatToBitmap(Mat img)
		{
			using (var stream = new MemoryStream())
			{
				img.WriteToStream(stream);
				return new Bitmap(stream);
			}
		}


		private void button2_Click(object sender, EventArgs e)
		{
			Mat image2 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Cv2.Rectangle(image2, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(125, 250), Scalar.All(255), -1);
			Bitmap bitmap2 = MatToBitmap(image2);
			pictureBox1.Image = bitmap2;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Mat image1 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			OpenCvSharp.Point center = new OpenCvSharp.Point(image1.Width / 2, image1.Height / 2);
			Cv2.Circle(image1, center, 80, Scalar.All(255), -1);
			Bitmap bitmap1 = MatToBitmap(image1);

			Mat image2 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Cv2.Rectangle(image2, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(125, 250), Scalar.All(255), -1);
			Bitmap bitmap2 = MatToBitmap(image2);

			Mat image3 = new Mat();

			Cv2.BitwiseOr(image1, image2, image3);  // image1과 image2의 Or연산

			Bitmap bitmap3 = MatToBitmap(image3);
			pictureBox1.Image = bitmap3;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			Mat image1 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			OpenCvSharp.Point center = new OpenCvSharp.Point(image1.Width / 2, image1.Height / 2);
			Cv2.Circle(image1, center, 80, Scalar.All(255), -1);
			Bitmap bitmap1 = MatToBitmap(image1);

			Mat image2 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Cv2.Rectangle(image2, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(125, 250), Scalar.All(255), -1);
			Bitmap bitmap2 = MatToBitmap(image2);

			Mat image3 = new Mat();

			Cv2.BitwiseAnd(image1, image2, image3);  // image1과 image2의 Or연산

			Bitmap bitmap3 = MatToBitmap(image3);
			pictureBox1.Image = bitmap3;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			Mat image1 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			OpenCvSharp.Point center = new OpenCvSharp.Point(image1.Width / 2, image1.Height / 2);
			Cv2.Circle(image1, center, 80, Scalar.All(255), -1);
			Bitmap bitmap1 = MatToBitmap(image1);

			Mat image2 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Cv2.Rectangle(image2, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(125, 250), Scalar.All(255), -1);
			Bitmap bitmap2 = MatToBitmap(image2);

			Mat image3 = new Mat();

			Cv2.BitwiseXor(image1, image2, image3);  // image1과 image2의 Or연산

			Bitmap bitmap3 = MatToBitmap(image3);
			pictureBox1.Image = bitmap3;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			Mat image1 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			OpenCvSharp.Point center = new OpenCvSharp.Point(image1.Width / 2, image1.Height / 2);
			Cv2.Circle(image1, center, 80, Scalar.All(255), -1);
			Bitmap bitmap1 = MatToBitmap(image1);

			Mat image2 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Cv2.Rectangle(image2, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(125, 250), Scalar.All(255), -1);
			Bitmap bitmap2 = MatToBitmap(image2);

			Mat image3 = new Mat();

			Cv2.BitwiseNot(image1, image3);  // image1과 image2의 Or연산

			Bitmap bitmap3 = MatToBitmap(image3);
			pictureBox1.Image = bitmap3;
		}
	}
}