using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp36_Bright.test_
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = new Mat("C:\\Temp\\img\\bright.jpg",ImreadModes.Grayscale);
			if (src.Empty())
			{
				Console.WriteLine("영상을 읽지 못했습니다.");
				Environment.Exit(0);
			}
			Mat dst1 = src + 100;
			Mat dst2 = src - 100;
			Mat dst3 = new Mat(src.Size(), src.Type(), Scalar.All(255)) - src;
			
			//픽셀단위 접근
			Mat dst4 = new Mat(src.Size(), src.Type());
			Mat dst5 = new Mat(src.Size(), src.Type());

			for(int i =0; i < src.Rows; i++)
			{
				for(int j = 0; j < src.Cols; j++)
				{
					dst4.At<byte>(i, j) = (byte)Math.Min(src.At<byte>(i, j) + 100, 255);
					dst5.At<byte>(i, j) = (byte)(255 - src.At<byte>(i, j));

				}
			}

			Cv2.ImShow("src", src);
			Cv2.ImShow("dst1", dst1);
			Cv2.ImShow("dst2", dst2);
			Cv2.ImShow("dst3", dst3);
			Cv2.ImShow("dst4", dst4);
			Cv2.ImShow("dst5", dst5);

			Cv2.WaitKey(0);


		}
	}
}
