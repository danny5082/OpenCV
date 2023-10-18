using OpenCvSharp;

namespace OpenCvQuiz03
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Mat one = new Mat("1.jpg");
			Mat two = new Mat("2.jpg");
			Mat three = new Mat("3.jpg");
			Mat four = new Mat("4.jpg");

			Mat left = new Mat();
			Mat right = new Mat();
			Mat dst = new Mat();

			Cv2.VConcat(new Mat[] { one, three }, left);
			Cv2.VConcat(new Mat[] { two, four }, right);
			Cv2.HConcat(new Mat[] { left, right }, dst);

			Cv2.ImShow("dst", dst);
			Cv2.WaitKey();
			Cv2.DestroyAllWindows();

		}
	}
}