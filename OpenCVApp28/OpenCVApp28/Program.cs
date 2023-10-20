using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp28
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = Cv2.ImRead("sudoku.png", ImreadModes.Grayscale);

			Cv2.ImShow("src", src);

			Mat dst = new Mat();
			Cv2.Threshold(src, dst, 0, 255,
				ThresholdTypes.Binary | ThresholdTypes.Otsu);
			Cv2.ImShow("dst", dst);

			Mat dst2 = new Mat();
			Cv2.AdaptiveThreshold(src, dst2, 255,
				AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 51, 7);
			Cv2.ImShow("dst2", dst2);



			Cv2.WaitKey(0);
		}
	}
}
