using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp14
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = new Mat(new Size(500, 500),
				MatType.CV_8UC3,
				new Scalar(255, 255, 255));

			Cv2.ImShow("draw", src);
			MouseCallback cvMouseCallback = new MouseCallback(Event);
			Cv2.SetMouseCallback("draw", cvMouseCallback,src.CvPtr);

			Cv2.WaitKey(0);

		}

		static void Event(MouseEventTypes @event, int x, int y,
			MouseEventFlags flags, IntPtr userdata)
		{
			Mat data = new Mat(userdata);

			if(flags == MouseEventFlags.LButton)
			{
				Cv2.Circle(data, new Point(x, y), 10, new Scalar(0, 0, 200), -1);
				Cv2.ImShow("draw", data);
			}
		}
	}
}
