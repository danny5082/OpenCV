using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVQuiz01
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = new Mat(new Size(500, 500), MatType.CV_8UC3, new Scalar(255, 255, 255));
			Cv2.ImShow("draw", src);
			MouseCallback cvMouseCallback = new MouseCallback(Event);
			Cv2.SetMouseCallback("draw", cvMouseCallback, src.CvPtr);
			Cv2.WaitKey(0);
			Cv2.DestroyAllWindows();

		}

		static void Event(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userdata)
		{
			Mat data = new Mat(userdata);

			if (@event == MouseEventTypes.LButtonDown)   // flags로 구분 ( 마우스 이벤트가 LButton 일 때 )
			{
				Console.WriteLine("마우스 왼쪽버튼 누르기");
			}
			else if (@event == MouseEventTypes.LButtonDoubleClick)
			{
				Console.WriteLine("마우스 왼쪽버튼 더블클릭");
			}
			else if (@event == MouseEventTypes.LButtonUp)
			{
				Console.WriteLine("마우스 왼쪽 버튼 떼기");
			}
			else if (@event == MouseEventTypes.RButtonDown)
			{
				Console.WriteLine("마우스 오른쪽버튼 누르기");
			}
			else if (@event == MouseEventTypes.RButtonUp)
			{
				Console.WriteLine("마우스 오른쪽 버튼 떼기");
			}
		}
	}
}