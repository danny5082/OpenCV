using OpenCvSharp;
using System.Runtime.Intrinsics;

namespace OpenCVApp03
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Vec3i v1 = new Vec3i(1, 2, 3);
			Vec3f v2 = new Vec3f(1.1f, 2.2f, 3.3f);

			Console.WriteLine(v1);
			Console.WriteLine(v2);

			Point3d pt1 = new Vec3d(1.0, 2.0, 3.0);
			Point3d pt2 = pt1;
			Console.WriteLine(pt1);
			Console.WriteLine(pt2);
		}
		private static void Test1()
		{
			Console.WriteLine(Cv2.GetVersionString());

			Vec4d vector1 = new Vec4d(1.0, 2.0, 3.0, 4.0);
			Vec4d vector2 = new Vec4d(1.0, 2.0, 3.0, 4.0);

			Console.WriteLine(vector1);
			Console.WriteLine(vector1.Item0);
			Console.WriteLine(vector1.Item1);

			Console.WriteLine(vector1.Equals(vector2));
		}
	}
}