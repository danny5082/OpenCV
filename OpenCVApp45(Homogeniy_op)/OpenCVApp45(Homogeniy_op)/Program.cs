using OpenCvSharp;
using System;

namespace OpenCVApp45_Homogeniy_op_
{
	internal class Program
	{
		static void HomogenOp(Mat img, out Mat dst, int maskSize) { 

			dst = new Mat(img.Size()  , MatType.CV_8UC1 , Scalar.All(0));

			Point h_m = new Point(maskSize / 2, maskSize / 2);

			for (int i = h_m.Y; i < img.Rows - h_m.Y; i++)
			{
				for (int j = h_m.X; j < img.Cols - h_m.X; j++)
				{
					byte max = 0;
					for (int u = 0; u < maskSize; u++)
					{
						for (int v = 0; v < maskSize; v++)
						{
							int y = i + u - h_m.Y;
							int x = j + v - h_m.X;
							byte difference = (byte)Math.Abs(img.Get<byte>(i, j) - img.Get<byte>(y, x));
							if (difference > max) max = difference;
						}
					}
					dst.Set<byte>(i, j, max);
				}
			}
		}
		static void Main(string[] args)
		{
			Mat img = new Mat("edge_test.jpg", ImreadModes.Grayscale);
			if (img.Empty())
			{
				Console.WriteLine("영상을 읽지 못했습니다.");
				Environment.Exit(0);
			}

			int maskSize = 3;
			Mat dst = new Mat();

			HomogenOp(img, out dst, maskSize);

			// 이미지 출력 및 대기
			Cv2.ImShow("img", img);
			Cv2.ImShow("dst", dst);

			Cv2.WaitKey(0);
		}
	}
}