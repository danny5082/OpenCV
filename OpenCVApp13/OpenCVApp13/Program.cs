using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp13
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat src = Cv2.ImRead("C:\\Temp\\img\\OpenCV.png",ImreadModes.ReducedColor2);

			//Console.WriteLine(src);
			//Cv2.ImShow("logo", src);
			//Cv2.WaitKey(0);

			Cv2.NamedWindow("src", WindowFlags.GuiExpanded);
			Cv2.SetWindowProperty("src", WindowPropertyFlags.Fullscreen, 0);
			Cv2.ImShow("src", src);
			Cv2.WaitKey(0);
			Cv2.DestroyWindow("src");
		}
	}
}
