using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenCVApp43_Histogram_Equalize
{
	internal class Program
	{
		static void draw_histo(Mat his, ref Mat hist_img, Size size)
		{
			Mat hist = his.Clone();
			hist_img = new Mat(size, MatType.CV_8U, Scalar.All(255));
			float bin = (float)hist_img.Cols / hist.Rows; //한 계급 너비

			//Mat src, Mat dst, 정규화 히스토그램 최솟값, 정규화 히스토그램 최댓값, 정규화방법 
			Cv2.Normalize(hist, hist, 0, hist_img.Rows, NormTypes.MinMax);

			for (int i = 0; i < hist.Rows; i++)
			{
				float start_x = i * bin;
				float end_x = (i + 1) * bin;
				Point pt1 = new Point(start_x, 0);
				Point pt2 = new Point(end_x, hist.At<float>(i));

				if (pt2.Y > 0)
					Cv2.Rectangle(hist_img, pt1, pt2, Scalar.All(0), -1);
			}
			Cv2.Flip(hist_img, hist_img, FlipMode.X);
		}

		static void create_hist(Mat img, ref Mat hist, ref Mat hist_img)
		{
			int histsize = 256, range = 256;
			Cv2.CalcHist(new Mat[] { img }, new int[] { 0 }, null, hist, 1, new int[] { histsize }, new Rangef[] { new Rangef(0, range) });
			draw_histo(hist, ref hist_img, new Size(256, 200));
		}
		static void Main(string[] args)
		{
			Mat image = Cv2.ImRead("C:\\Temp\\img\\equalize_test.jpg", ImreadModes.Grayscale);
			if (image.Empty())
			{
				Console.WriteLine("이미지가 없습니다.");
				Environment.Exit(0);
			}

			Mat hist = new Mat();
			Mat dst1 = new Mat();
			Mat dst2 = new Mat();
			Mat hist_img = new Mat();
			Mat hist_img1 = new Mat();
			Mat hist_img2 = new Mat();

			// 히스토그램 평활화 직접하기
			create_hist(image, ref hist, ref hist_img);

			Mat accum_hist = new Mat(hist.Size(), hist.Type(), new Scalar(0));
			accum_hist.At<float>(0) = hist.At<float>(0);
			for (int i = 1; i < hist.Rows; i++)
			{
				//히스토그램 빈도 행렬
				accum_hist.At<float>(i) = accum_hist.At<float>(i - 1) + hist.At<float>(i);
			}

			accum_hist /= Cv2.Sum(hist)[0]; //누적합의 정규화
			accum_hist *= 255;
			dst1 = new Mat(image.Size(), MatType.CV_8U);
			for (int i = 0; i < image.Rows; i++)
			{
				for (int j = 0; j < image.Cols; j++)
				{
					int idx = image.At<byte>(i, j);
					dst1.At<byte>(i, j) = (byte)accum_hist.At<float>(idx);
				}
			}

			////////////////////////OpenCv 히스토그램 평활화 ////////////////
			Cv2.EqualizeHist(image, dst2);

			//생성 및 출력
			create_hist(dst1, ref hist, ref hist_img1);
			create_hist(dst2, ref hist, ref hist_img2);

			Cv2.ImShow("image", image);
			Cv2.ImShow("img_hist", hist_img);
			Cv2.ImShow("dst1-User", dst1);
			Cv2.ImShow("User_hist", hist_img1);
			Cv2.ImShow("dst2-OpenCV", dst2);
			Cv2.ImShow("OpenCV_hist", hist_img2);

			Cv2.WaitKey();
		}
	}
}