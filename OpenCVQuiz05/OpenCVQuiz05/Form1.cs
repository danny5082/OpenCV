using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCVQuiz05
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			pictureBox1.Load("C:\\Temp\\img\\lenna.jpg");
		}
		

		private void button1_Click(object sender, EventArgs e)
		{

			using (Mat colorImage = new Mat("C:\\Temp\\img\\lenna.jpg", ImreadModes.Color))
			{
				Mat grayImage = new Mat();
				//흑백화면으로 변경하는 함수
				Cv2.CvtColor(colorImage, grayImage, ColorConversionCodes.BGR2GRAY);

				Bitmap bitmap = MatToBitmap(grayImage);
				pictureBox1.Image = bitmap;
			}
		}
		private Bitmap MatToBitmap(Mat image)
		{
			using (var stream = new MemoryStream())
			{
				image.WriteToStream(stream);
				return new Bitmap(stream);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			pictureBox1.Load("C:\\Temp\\img\\lenna.jpg");
		}

		private void button3_Click(object sender, EventArgs e)
		{
				Bitmap bitmap = (Bitmap)pictureBox1.Image;

				// 이미지를 저장할 경로와 파일 이름을 지정합니다.
				string savePath = "CV.jpeg";

				// 이미지를 JPEG 형식으로 저장합니다.
				bitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

				Console.WriteLine("Image saved to " + savePath);
		}
	}
}
