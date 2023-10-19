using OpenCvSharp;
using System;


using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVApp16
{
	internal class Program
	{
		static void Main(string[] args)
		{
			int[] data = new int[9];
			for(int i  = 0; i < data.Length; i++)
			{
				data[i] = i + 1;
			}

			Mat m1 = new Mat(3, 3, MatType.CV_32S, data);
			Console.WriteLine(m1.Dump());
		}
	}
}