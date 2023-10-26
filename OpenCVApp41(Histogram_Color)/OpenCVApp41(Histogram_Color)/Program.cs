using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp41_Histogram_Color_
{
	internal class Program
	{
		static Mat make_palete(int rows)
		{
			Mat hsv = new Mat(rows, 1, MatType.CV_8UC3);

			for (int i = 0; i < rows; i++)
			{
				//부동소수점을 반올림하거나 행렬 범위를 못 벗어나게 하는 Saturate Cast
				float huefloat = (float)i / rows * 180;
				byte huebyte = (byte)Math.Min(Math.Max(huefloat, 0), 255);

				hsv.At<Vec3b>(i) = new Vec3b(huebyte, 255, 255);
			}
			Cv2.CvtColor(hsv, hsv, ColorConversionCodes.HSV2BGR);
			return hsv;
		}
		static void draw_histo_hue(Mat hist, ref Mat hist_img, Size size)
		{
			Mat hsv_palatte = make_palete(hist.Rows);

			hist_img = new Mat(size, MatType.CV_8UC3, Scalar.All(255));
			float bin = (float)hist_img.Cols / hist.Rows; //계급 개수
			Cv2.Normalize(hist, hist, 0, hist_img.Rows, NormTypes.MinMax); //정규화

			for (int i = 0; i < hist.Rows; i++)
			{
				float start_x = (i * bin);
				float end_x = (i + 1) * bin;
				Point pt1 = new Point(start_x, 0);
				Point pt2 = new Point(end_x, hist.At<float>(i));

				Vec3b vec = hsv_palatte.At<Vec3b>(i);
				Scalar color = new Scalar((byte)vec.Item0, (byte)vec.Item1, (byte)vec.Item2);

				if (pt2.Y > 0)
					Cv2.Rectangle(hist_img, pt1, pt2, color, -1);
			}
			Cv2.Flip(hist_img, hist_img, FlipMode.X);
		}

		static void calc_Histo(Mat image, out Mat hist, int bins, int rangeMax = 256)
        {
            int[] histSize = { bins };
            Rangef[] ranges = { new Rangef(0, rangeMax) };
            int[] channels = { 0 };
            hist = new Mat();
		    Cv2.CalcHist(new[] { image }, channels, null, hist, 1, histSize, ranges);
        }
		static void Main(string[] args)
		{
			Mat image = Cv2.ImRead("C:\\Temp\\img\\pixel_test.jpg", ImreadModes.Color);
			if (image.Empty())
			{
				Console.WriteLine("이미지 오류.");
				Environment.Exit(0);
			}

			Mat HSV_img = new Mat();
			Mat[] HSV_arr = new Mat[3];
			Cv2.CvtColor(image, HSV_img, ColorConversionCodes.BGR2HSV);
			HSV_arr = Cv2.Split(HSV_img);

			Mat hue_hist = new Mat();
			Mat hue_hist_img = new Mat();

			//Mat[], int 채널[] 명암 0, mask, Mat, dims 명암1, x축 개수[], y축 Rangef[]
			Cv2.CalcHist(new Mat[] { HSV_arr[0] }, new int[] { 0 }, null, hue_hist, 1, new int[] { 18 }, new Rangef[] { new Rangef(0, 180) });
			draw_histo_hue(hue_hist, ref hue_hist_img, new Size(360, 200));

			Cv2.ImShow("image", image);
			Cv2.ImShow("Hue_hist_img", hue_hist_img);
			Cv2.WaitKey();
		}
	}
}