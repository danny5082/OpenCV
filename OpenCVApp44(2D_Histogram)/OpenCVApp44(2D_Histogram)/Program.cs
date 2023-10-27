using System;
using System.Collections.Generic;
using System.IO;
using OpenCvSharp;

namespace YourNamespace
{
	class Histo
	{
		public static void CalcHisto(Mat img, Mat hist, Vec3i bins, Vec3f range, int dims)
		{
			int dimensions = (dims <= 0) ? img.Channels() : dims;
			int[] channels = { 0, 1, 2 };
			int[] histSize = { bins.Item0, bins.Item1, bins.Item2 };

			float[] range1 = { 0, range.Item0 };
			float[] range2 = { 0, range.Item1 };
			float[] range3 = { 0, range.Item2 };
			float[][] ranges = { range1, range2, range3 };

			Cv2.CalcHist(new[] { img }, channels, null, hist, dimensions, histSize, ranges);
			Cv2.Normalize(hist, hist, 0, 1, NormTypes.MinMax);
		}

		public static Mat DrawHisto(Mat hist)
		{
			if (hist.Dims != 2)
			{
				Console.WriteLine("히스토그램이 2차원 데이터가 아닙니다.");
				Environment.Exit(1);
			}

			float ratioValue = 512;
			float ratioHue = 180f / hist.Rows;
			float ratioSat = 256f / hist.Cols;

			Mat graph = new Mat(hist.Size(), MatType.CV_8UC3);
			for (int i = 0; i < hist.Rows; i++)
			{
				for (int j = 0; j < hist.Cols; j++)
				{
					float value = (float)(hist.Get<float>(i, j) * ratioValue);
					float hue = i * ratioHue;
					float sat = j * ratioSat;
					graph.Set(i, j, new Vec3b((byte)hue, (byte)sat, (byte)value));
				}
			}

			Cv2.CvtColor(graph, graph, ColorConversionCodes.HSV2BGR);
			Cv2.Resize(graph, graph, new Size(0, 0), 10, 10, InterpolationFlags.Linear);

			return graph;
		}

		public static List<Mat> LoadHisto(Vec3i bins, Vec3f ranges, int nImages)
		{
			List<Mat> DBHists = new List<Mat>();
			for (int i = 0; i < nImages; i++)
			{
				string fname = string.Format("img_{0:D2}.jpg", i);
				if (File.Exists(fname))
				{
					Mat img = new Mat(fname, ImreadModes.Color);
					Mat hsv = new Mat();
					Mat hist = new Mat();
					Cv2.CvtColor(img, hsv, ColorConversionCodes.BGR2HSV);
					CalcHisto(hsv, hist, bins, ranges, 2);
					DBHists.Add(hist);
				}
			}
			Console.WriteLine($"{DBHists.Count} 개의 파일을 로드 및 히스토그램 계산 완료");
			return DBHists;
		}

		public static Mat QueryImg()
		{
			int qNo = 74;
			do
			{
				Console.Write("질의 영상 번호를 입력하세요: ");
				if (int.TryParse(Console.ReadLine(), out qNo))
				{
					string fname = string.Format("img_{0:D2}.jpg", qNo);
					if (File.Exists(fname))
					{
						Mat query = new Mat(fname, ImreadModes.Color);
						if (!query.Empty())
							return query;
					}
				}
				Console.WriteLine("질의 영상 번호가 잘못되었습니다.");
			} while (true);
		}

		public static Mat CalcSimilarity(Mat queryHist, List<Mat> DBHists)
		{
			Mat DBSimilarities = new Mat(DBHists.Count, 1, MatType.CV_64FC1);
			int i = 0;
			foreach (Mat hist in DBHists)
			{
				double compare = Cv2.CompareHist(queryHist, hist, HistCompMethods.Correl);
				// 새 값을 DBSimilarities에 추가합니다.
				DBSimilarities.Set(i, 0, compare);
				i++;
			}

			return DBSimilarities;
		}

		public static void SelectView(double sinc, Mat DBSimilarities, Vec3i bins, Vec3f ranges)
		{
			Mat mIdx = new Mat();
			Mat sortedSim = new Mat();
			Cv2.Sort(DBSimilarities, sortedSim, SortFlags.EveryColumn | SortFlags.Descending);
			Cv2.SortIdx(DBSimilarities, mIdx, SortFlags.EveryColumn | SortFlags.Descending);



			for (int i = 0; i < sortedSim.Total(); i++)
			{
				double sim = sortedSim.At<double>(i);
				if (sim > sinc)
				{
					int idx = mIdx.At<int>(i);
					string fname = string.Format("img_{0:D2}.jpg", idx);
					Mat img = new Mat(fname, ImreadModes.Color);
					Mat hsv = new Mat();
					Mat hist = new Mat();
					Cv2.CvtColor(img, hsv, ColorConversionCodes.BGR2HSV);
					CalcHisto(hsv, hist, bins, ranges, 2);
					Mat histImg = DrawHisto(hist);
					Mat tmp = MakeImg(img, histImg);

					string title = string.Format("img_{0:D3} - {1:F2}", idx, sim);
					Console.WriteLine(title);
					Cv2.ImShow(title, tmp);
				}
			}
		}

		public static Mat MakeImg(Mat img, Mat histImg)
		{
			int w = img.Cols + histImg.Cols + 10;
			int h = Math.Max(img.Rows, histImg.Rows);
			Mat tmp = new Mat(h, w, MatType.CV_8UC3, new Scalar(255, 255, 255));

			int gap = Math.Abs(img.Rows - histImg.Rows) / 2;
			Rect r1 = new Rect(new Point(0, 0), img.Size());
			Rect r2 = new Rect(new Point(img.Cols + 5, gap), histImg.Size());

			img.CopyTo(tmp[r1]);
			histImg.CopyTo(tmp[r2]);

			return tmp;
		}

		public static void Main()
		{
			Vec3i bins = new Vec3i(30, 42, 0);
			Vec3f ranges = new Vec3f(180, 256, 0);

			List<Mat> DBHists = LoadHisto(bins, ranges, 100);
			Mat query = QueryImg();

			Mat hsv = new Mat();
			Mat queryHist = new Mat();
			Cv2.CvtColor(query, hsv, ColorConversionCodes.BGR2HSV);
			CalcHisto(hsv, queryHist, bins, ranges, 2);
			Mat histImg = DrawHisto(queryHist);

			Mat DBSimilarities = CalcSimilarity(queryHist, DBHists);

			double sinc;
			Console.Write("기준 유사도 입력: ");
			sinc = Convert.ToDouble(Console.ReadLine());

			SelectView(sinc, DBSimilarities, bins, ranges);

			Cv2.ImShow("image", query);
			Cv2.ImShow("hist_img", histImg);
			Cv2.WaitKey();
		}
	}
}