using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp15
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string path = "C:\\Temp\\img\\vtest.avi";
			
			VideoCapture capture = new VideoCapture(path);
			//VideoCapture capture = new VideoCapture(0);
			Mat frame = new Mat();
			capture.Set(VideoCaptureProperties.FrameWidth, 640);
			capture.Set(VideoCaptureProperties.FrameHeight, 640);

			while (true)
			{
				if (capture.PosFrames == capture.FrameCount)
					capture.Open(path);

				capture.Read(frame);
				Cv2.ImShow("VideoFrame", frame);

				if (Cv2.WaitKey(33) == 'q')
				break;
			}

			capture.Release();
			Cv2.DestroyAllWindows();

		}
	}
}
