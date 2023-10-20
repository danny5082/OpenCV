using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp29
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\tomato.jpg");
			Mat hsv = new Mat(src.Size(), MatType.CV_8UC3);
			Mat dst = new Mat(src.Size(), MatType.CV_8UC3);

			Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);
			Mat[] HSV = Cv2.Split(hsv);
			//HSV 각각 채널을 분리
			//Hue 색깔에 해당하는 시각적 감각의 속성
			//Saturation 채도 색상의 깊이
			//Value(명도) 밝고 어두운 정도

			Mat H_orange = new Mat(src.Size(),MatType.CV_8UC1);
			Cv2.InRange(HSV[0], new Scalar(8), new Scalar(20), H_orange);

			Cv2.BitwiseAnd(hsv, hsv, dst, H_orange);
			Cv2.CvtColor(dst, dst, ColorConversionCodes.HSV2BGR);

			Cv2.ImShow("Orange", dst); 
			Cv2.ImShow("src", src);
			Cv2.WaitKey();
		}
	}
}
