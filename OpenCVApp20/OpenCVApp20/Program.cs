using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp20
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//사진 불러오기 
			Mat src = Cv2.ImRead("C:\\Temp\\img\\balloon.jpg");

			//목적파일이 있어야 함
			Mat dst = new Mat(src.Size(), MatType.CV_8UC3);


			Cv2.CvtColor(src, dst, ColorConversionCodes.BGR2HSV);

			Cv2.ImShow("dst", dst);
			Cv2.WaitKey();
			Cv2.DestroyAllWindows();

		}
	}
}