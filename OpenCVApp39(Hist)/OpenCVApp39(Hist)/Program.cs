using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp39_Hist_
{

	internal class Program
	{
		static void calc_histo(Mat image, ref Mat hist, int bins, int range_max)
		{
			hist = new Mat(bins, 1, MatType.CV_32F, new Scalar(0));
			float gap = range_max / (float)bins;

			for (int i = 0; i < image.Rows; i++)
			{
				for (int j = 0; j < image.Cols; j++)
				{
					int idx = (int)(image.At<byte>(i, j) / gap);
					hist.At<float>(idx)++;
				}
			}
		}
		static void Main(string[] args)
		{
			Mat image = Cv2.ImRead("C:\\Temp\\img\\pixel_test.jpg", ImreadModes.Grayscale);
			if (image.Empty())
			{
				Console.WriteLine("이미지가 존재하지 않습니다.");
				Environment.Exit(0);
			}

			Mat hist = new Mat();
			calc_histo(image, ref hist, 256, 256);

			Console.Write("[");
			for (int i = 0; i < hist.Rows; i++)
			{
				Console.Write($"{hist.At<float>(i)} ");
			}
			Console.WriteLine("]");

			Cv2.ImShow("image", image);
			Cv2.WaitKey();
		}
	}
}
