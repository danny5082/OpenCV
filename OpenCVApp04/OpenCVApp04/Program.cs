
using OpenCvSharp;

namespace OpenCVApp04
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Point Pt1 = new Point(1, 2);
			Point Pt2 = new Point(3, 2);

			Console.WriteLine(Pt1 + Pt2);
			Console.WriteLine(Pt1 - Pt2);
			Console.WriteLine(Pt1 == Pt2);
			Console.WriteLine(Pt1 * 0.5);

			//거리
			Console.WriteLine(Pt1.DistanceTo(Pt2));
			//내적
			Console.WriteLine(Pt1.DotProduct(Pt2));
			//외적
			Console.WriteLine(Pt1.CrossProduct(Pt2));


			Scalar s1 = new Scalar(255,127);
			Scalar s2 = Scalar.Yellow;
			Scalar s3 = Scalar.All(99);

			Console.WriteLine(s1);
			Console.WriteLine(s2);
			Console.WriteLine(s3);


		}
	}
}