using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp38_Contrast_
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat image = new Mat("C:\\Temp\\img\\contrast_test.jpg", ImreadModes.Grayscale);

			if (image.Empty())
			{
				Console.WriteLine("영상을 읽지 못 했습니다.");
				Environment.Exit(1);
			}

			Scalar meanValue = Cv2.Mean(image);
			double avg = meanValue.Val0 / 2.0;

			Mat dst1 = image * 0.5;
			Mat dst2 = image * 0.2;
			Mat dst3 = new Mat(); //평균이용 대비감소
			Mat dst4 = new Mat(); //평균이용 대비중가

			Cv2.AddWeighted(image, 0.5, Mat.Ones(image.Size(), image.Type()) * avg, 1, 0, dst3);
			Cv2.AddWeighted(image, 2.0, Mat.Ones(image.Size(), image.Type()) * -avg, 1, 0, dst4);

			Cv2.ImShow("image", image);
			Cv2.ImShow("dst1", dst1);
			Cv2.ImShow("dst2", dst2);
			Cv2.ImShow("dst3", dst3);
			Cv2.ImShow("dst4", dst4);


			Cv2.WaitKey(0);

		}
	}
}
