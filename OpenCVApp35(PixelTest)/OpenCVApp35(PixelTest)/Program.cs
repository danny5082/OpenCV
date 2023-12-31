﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp35_PixelTest_
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat image = new Mat("C:\\Temp\\Img\\pixel_test.jpg",ImreadModes.Grayscale);
			if (image.Empty())
			{
				Console.WriteLine("영상을 읽지 못했습니다.");
				Environment.Exit(0);
			}

			Rect roi = new Rect(135, 95, 20, 15);
			Mat roi_img = image.SubMat(roi);

			for(int i = 0; i < roi_img.Rows; i++)
			{
				for(int j = 0; j < roi_img.Cols; j++)
				{
					Console.Write($"{roi_img.At<byte>(i, j),5}");
				}
				Console.WriteLine();
			}
			image.Rectangle(roi, Scalar.White, 1);
			Cv2.ImShow("image", image);
			Cv2.ImShow("roiImage", roi_img.Resize(new Size(200,100)));
			Cv2.WaitKey(0);

		}
	}
}
