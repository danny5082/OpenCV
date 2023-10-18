using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp07
{
		internal class Program
		{
			//히스토그램 
			static void Main(string[] args)
			{
				//ImRead = OpenCV에서 그림을 읽어오는 것 
				Mat src = Cv2.ImRead("C:\\Temp\\img\\image.jpg");
				Mat gray = new Mat(); //비어있는 화면
				Mat hist = new Mat();
				Mat result = Mat.Ones(new Size(256, src.Height), MatType.CV_8UC1);
				Mat dst = new Mat();


				// OpenCv 사진 변환 CvtColor
				// 컬러 -- > 흑백 
				Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

				//히스토그램 그리기 = 화소값 누적  
				Cv2.CalcHist(new Mat[] { gray },
							 new int[] { 0 },
							 null,
							 hist,
							 1,
							 new int[] { 256 },
							 new Rangef[] { new Rangef(0, 256) });

				//평탄화
				Cv2.Normalize(hist, hist, 0, 255, NormTypes.MinMax);

				for (int i = 0; i < hist.Rows; i++)
				{
					Cv2.Line(result, new Point(i, src.Height),
						new Point(i, src.Height - hist.Get<float>(i)),
						Scalar.White);

				}
				Cv2.HConcat(new Mat[] { gray, result }, dst);
				//Imshow = 사진 출력 
				Cv2.ImShow("원본파일", src);
				Cv2.ImShow("gray", gray);
				Cv2.ImShow("dst", dst);
				//Cv2.ImShow("hist", hist);
				// 0초하면 무한대로 사진 출력 
				Cv2.WaitKey(0);

			}
		}
	}