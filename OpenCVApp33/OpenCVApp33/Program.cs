using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp33
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = new Mat(50, 512, MatType.CV_8UC1, Scalar.Black);
			Mat dst = new Mat(50, 512, MatType.CV_8UC1, Scalar.Black);

			for( int i  = 0; i < src.Rows;  i++ )
			{
				for(int j = 0; j<src.Cols; j++)
				{
					src.At<int>(i, j) = j / 2;
					dst.At<int>(i, j) = (j / 20) * 10;
				}
			}
			Cv2.ImShow("src", src);
			Cv2.ImShow("dst", dst);
			Cv2.WaitKey(0);
		}
	}
}
