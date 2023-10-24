using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp31
{
	internal class Program
	{
		static void Main(string[] args)
		{
			String Path = "C:\\Temp\\img\\moon.jpg";
			Mat src = Cv2.ImRead(Path);
			Mat dst = new Mat(src.Size(), MatType.CV_8UC3);
			Mat dst2 = new Mat(src.Size(), MatType.CV_8UC3);

			//GaussianBlur 사용
			Cv2.GaussianBlur(src, dst, new Size(9, 9), 3, 3,
				BorderTypes.Isolated);


			//양방향 필터 
			Cv2.BilateralFilter(src, dst2, 100, 33, 11,
				BorderTypes.Isolated);

			Cv2.ImShow("src", src);
			Cv2.ImShow("dst", dst);
			Cv2.ImShow("dst2", dst2);
			Cv2.WaitKey();
			Cv2.DestroyAllWindows();
		}
	}
}