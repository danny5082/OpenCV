using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp24
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat image = new Mat("C:\\Temp\\img\\boat.jpg", ImreadModes.Color);

			//예외처리
			if (image.Empty())
			{
				throw new Exception("이미지를 찾을 수 없거나 읽을 수 없습니다");
			}

			//채널분리 
			Mat[] bgr = Cv2.Split(image);

			//원본 
			new Window("image", image);
			new Window("B", bgr[0]);
			new Window("G", bgr[1]);
			new Window("R", bgr[2]);

			Cv2.WaitKey();


		}
	}
}