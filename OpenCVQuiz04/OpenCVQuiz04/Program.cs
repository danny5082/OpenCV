using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVQuiz04
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat img = Cv2.ImRead("C:\\Temp\\img\\image.jpg");
			bool save;

			ImageEncodingParam[] prms = new ImageEncodingParam[]
			{
				new ImageEncodingParam(ImwriteFlags.JpegQuality , 100),
				new ImageEncodingParam(ImwriteFlags.JpegProgressive , 1)

			};
			Mat grayImage = new Mat();
			//흑백화면으로 변경하는 함수
			Cv2.CvtColor(img, grayImage, ColorConversionCodes.BGR2GRAY);


			save = Cv2.ImWrite("CV.jpeg", grayImage, prms);
			Console.WriteLine(save);
		}
	}
}