using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp26
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat image1 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Mat image2 = new Mat(250, 250, MatType.CV_8UC1, Scalar.All(0));
			Mat image3 = new Mat();
			Mat image4 = new Mat();
			Mat image5 = new Mat();
			Mat image6 = new Mat();

			Point center = new Point(image1.Width/2, image1.Height/2);
			Cv2.Circle(image1 , center ,80 , Scalar.All(255),-1);
			Cv2.Rectangle(image2 , new Point(0,0) , new Point(125,250), Scalar.All(255),-1);

			Cv2.BitwiseOr(image1, image2, image3);
			Cv2.BitwiseAnd(image1, image2, image4);
			Cv2.BitwiseXor(image1, image2, image5);
			Cv2.BitwiseNot(image1,image6);


			Cv2.ImShow("image1", image1);
			Cv2.ImShow("image2", image2);
			Cv2.ImShow("image3", image3);
			Cv2.ImShow("image4", image4);
			Cv2.ImShow("image5", image5);
			Cv2.ImShow("image6", image6);
			Cv2.WaitKey(0);
		}
	}
}
