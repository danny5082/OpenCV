using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCVAPP_FunctionCamera_
{
	public partial class Form1 : Form
	{
		private VideoCapture capture;
		private Mat frame;
		private Mat resultFrame;
		private bool isRunning = false;
		private bool isColor = true;
		private bool isEdgeMode = false;
		private bool isHistogramOn = false;
		private Bitmap displayBitmap = null;
		private bool isCustomEffectOn = false;
		private CascadeClassifier faceCascade;
		// private bool isMouthRed = false;

		bool isFlipped = false;
		bool isSharpening = false;
		bool isFlippedY = false;
		// bool isCannyEnabled = false; // 캐니 엣지 활성화 여부를 나타내는 변수 추가
		bool Blur = false;             //블러
		bool isConvex = false;         //반전 
		bool increase_brightness = false;
		bool lower_the_brightness = false;
		bool isConvexMirror = false;
		bool isConcaveMirror = false;
		bool embossing = false;


		public Form1()
		{
			InitializeComponent();
			//capture.Set(VideoCaptureProperties.FrameWidth, pictureBox1.Width);
			//capture.Set(VideoCaptureProperties.FrameHeight, pictureBox1.Height);
			faceCascade = new CascadeClassifier("C:\\Users\\Admin\\source\\OpenCV\\OpenCVAPP(FunctionCamera)\\haarcascade_frontalface_default.xml");
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			capture = new VideoCapture(0);
			frame = new Mat();
			capture.Set(VideoCaptureProperties.FrameWidth, 640);
			capture.Set(VideoCaptureProperties.FrameHeight, 480);
		}
		private async void ProcessFrames()
		{
			while (isRunning)
			{
				if (capture.IsOpened() && capture.Read(frame))
				{
					//Mat resultFrame = frame.Clone();

					if (!isColor)
					{
						Cv2.CvtColor(frame, frame, ColorConversionCodes.BGR2GRAY);
						Cv2.CvtColor(frame, frame, ColorConversionCodes.GRAY2BGR);
					}

					if (isEdgeMode)
					{
						var grayFrame = new Mat();
						Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);
						Mat edges = new Mat();
						Cv2.Canny(grayFrame, edges, 25, 200);
						Cv2.CvtColor(edges, frame, ColorConversionCodes.GRAY2BGR);
					}

					if (isCustomEffectOn)
					{
						var grayFrame = new Mat();
						Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);
						Cv2.EqualizeHist(grayFrame, grayFrame);

						var faces = faceCascade.DetectMultiScale(
							grayFrame, scaleFactor: 1.1, minNeighbors: 5, minSize: new OpenCvSharp.Size(30, 30));

						foreach (var rect in faces)
						{
							var faceRegion = new OpenCvSharp.Rect(rect.X, rect.Y, rect.Width, rect.Height);

							// 초록색 사각형 그리기
							frame.Rectangle(faceRegion, Scalar.Green, 2);

							// 얼굴 영역에 모자이크 효과 적용
							var mosaicRegion = new Mat(frame, faceRegion).Clone();

							// 얼굴 영역에 모자이크 효과 적용
							Cv2.Resize(mosaicRegion, mosaicRegion, new OpenCvSharp.Size(0, 0), 1.0 / 10, 1.0 / 10, InterpolationFlags.Nearest);
							Cv2.Resize(mosaicRegion, frame[faceRegion], new OpenCvSharp.Size(faceRegion.Width, faceRegion.Height), 0, 0, InterpolationFlags.Nearest);
						}
					}

					///////////////////////////////////////////////////////////

					if (isConvex)
					{
						Cv2.CvtColor(~frame, frame, ColorConversionCodes.BGR2GRAY);
					}
					if (embossing)
					{
						frame = ApplyEmbossing(frame);
					}

					if (lower_the_brightness)
					{
						frame -= new Scalar(60, 60, 60); // 명도를 -100 만큼 감소시킴

						// 명도를 클리핑하여 0보다 작은 값이 없도록 함
						for (int y = 0; y < frame.Rows; y++)
						{
							for (int x = 0; x < frame.Cols; x++)
							{
								Vec3b pixel = frame.At<Vec3b>(y, x);
								for (int c = 0; c < 3; c++)
								{
									if (pixel[c] < 0)
									{
										pixel[c] = 0;
									}
								}
								frame.Set(y, x, pixel);
							}
						}
					}
					if (increase_brightness)
					{
						frame += new Scalar(100, 100, 100); // 명도를 -100 만큼 감소시킴

						// 명도를 클리핑하여 0보다 작은 값이 없도록 함
						for (int y = 0; y < frame.Rows; y++)
						{
							for (int x = 0; x < frame.Cols; x++)
							{
								Vec3b pixel = frame.At<Vec3b>(y, x);
								for (int c = 0; c < 3; c++)
								{
									if (pixel[c] < 0)
									{
										pixel[c] = 0;
									}
								}
								frame.Set(y, x, pixel);
							}
						}
					}
					if (Blur)
					{
						Cv2.GaussianBlur(frame, frame, new OpenCvSharp.Size(15, 15), 50, 50,
						BorderTypes.Isolated);
					}
					if (isConvexMirror)
					{
						Mat destination = new Mat(frame.Size(), MatType.CV_8UC3);
						OpenCvSharp.Point center = new OpenCvSharp.Point(frame.Width / 2 + 40, frame.Height / 2 - 20);
						double newRadius = 3.0 * center.Y; // 원하는 왜곡 정도에 따라 조절

						for (int y = 0; y < frame.Height; y++)
						{
							for (int x = 0; x < frame.Width; x++)
							{
								OpenCvSharp.Point current = new OpenCvSharp.Point(x, y); // 현재 위치
								double distance = center.DistanceTo(current);

								if (distance <= newRadius)
								{
									double angle = Math.Atan2(y - center.Y, x - center.X);
									double radius = Math.Pow(newRadius, 2) / (5.0 * (newRadius - distance));
									int newX = (int)(center.X + radius * Math.Cos(angle));
									int newY = (int)(center.Y + radius * Math.Sin(angle));

									if (newX >= 0 && newX < frame.Width && newY >= 0 && newY < frame.Height)
									{
										Vec3b pixel = frame.At<Vec3b>(newY, newX);
										destination.Set(y, x, pixel);
									}
								}
								else
								{
									destination.Set(y, x, new Vec3b(0, 0, 0)); // 원 밖은 검은색으로 채움
								}
							}
						}

						frame = destination;
						pictureBox1.Image = BitmapConverter.ToBitmap(frame);
					}
					if (isConcaveMirror)
					{
						Mat destination = new Mat(frame.Size(), MatType.CV_8UC3);
						OpenCvSharp.Point center = new OpenCvSharp.Point(frame.Width / 2 + 40, frame.Height / 2 - 20);
						double newRadius = center.Y / 0.7; // 원하는 왜곡 정도에 따라 조절

						for (int y = 0; y < frame.Height; y++)
						{
							for (int x = 0; x < frame.Width; x++)
							{
								OpenCvSharp.Point current = new OpenCvSharp.Point(x, y); // 현재 위치
								double distance = center.DistanceTo(current);

								if (distance <= newRadius)
								{
									double angle = Math.Atan2(y - center.Y, x - center.X);
									double radius = Math.Pow(newRadius, 2) / (5.0 * (newRadius - distance));
									int newX = (int)(center.X + radius * Math.Cos(angle));
									int newY = (int)(center.Y + radius * Math.Sin(angle));

									if (newX >= 0 && newX < frame.Width && newY >= 0 && newY < frame.Height)
									{
										Vec3b pixel = frame.At<Vec3b>(newY, newX);
										destination.Set(y, x, pixel);
									}
								}
								else
								{
									destination.Set(y, x, new Vec3b(0, 0, 0)); // 원 밖은 검은색으로 채움
								}
							}

						}
						frame = destination; // 오목 거울 효과가 적용된 프레임으로 업데이트
						pictureBox1.Image = BitmapConverter.ToBitmap(frame);
					}

					if (lower_the_brightness)
					{
						frame -= new Scalar(80,80,80); // 명도를 -100 만큼 감소시

						// 명도를 클리핑하여 0보다 작은 값이 없도록 함
						for (int y = 0; y < frame.Rows; y++)
						{
							for (int x = 0; x < frame.Cols; x++)
							{
								Vec3b pixel = frame.At<Vec3b>(y, x);
								for (int c = 0; c < 3; c++)
								{
									if (pixel[c] < 0)
									{
										pixel[c] = 0;
									}
								}
								frame.Set(y, x, pixel);
							}
						}
					}
					if (increase_brightness)
					{
						frame += new Scalar(80,80,80); // 명도를 -100 만큼 감소시킴

						// 명도를 클리핑하여 0보다 작은 값이 없도록 함
						for (int y = 0; y < frame.Rows; y++)
						{
							for (int x = 0; x < frame.Cols; x++)
							{
								Vec3b pixel = frame.At<Vec3b>(y, x);
								for (int c = 0; c < 3; c++)
								{
									if (pixel[c] < 0)
									{
										pixel[c] = 0;
									}
								}
								frame.Set(y, x, pixel);
							}
						}
					}
					if (Blur)
					{
						Cv2.GaussianBlur(frame, frame, new OpenCvSharp.Size(15, 15), 50, 50,
						BorderTypes.Isolated);
					}

					if (isFlippedY)
					{
						Cv2.Flip(frame, frame, FlipMode.X);
					}
					if (isFlipped)
					{
						//Mat flippedFrame = frame.Clone(); // 프레임 복제
						//Cv2.Flip(flippedFrame, flippedFrame, FlipMode.Y);
						//pictureBox1.Image = BitmapConverter.ToBitmap(flippedFrame);
						//flippedFrame.Release(); // 복제한 프레임 해제
						Cv2.Flip(frame, frame, FlipMode.Y);
					}

					else
					{
						pictureBox1.Image = BitmapConverter.ToBitmap(frame);
					}
					if (isSharpening)
					{
						Mat sharpenedFrame = ApplySharpening(frame);
						if (isFlipped)
						{
							Cv2.Flip(sharpenedFrame, sharpenedFrame, FlipMode.Y);
						}
						pictureBox1.Image = BitmapConverter.ToBitmap(sharpenedFrame);
						sharpenedFrame.Release(); // 먼저 해제하지 않음
					}
					//pictureBox1.Image = BitmapConverter.ToBitmap(frame);
					//pictureBox1.Image = BitmapConverter.ToBitmap(resultFrame);

				}
				await Task.Delay(33);
			}
		}
		private async void  button1_Click(object sender, EventArgs e)
		{
			if (isRunning)
			{
				isRunning = false;
				button1.Text = "카메라 꺼짐";
				return;
			}

			button1.Text = "카메라 켜짐";
			isRunning = true;

			ProcessFrames();

			while (isRunning)
			{
				Application.DoEvents();
				await Task.Delay(33);
			}
		}
		private void button2_Click(object sender, EventArgs e)
		{
			isColor = false;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			isColor = true;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			isEdgeMode = !isEdgeMode;

			if (isEdgeMode)
			{
				button4.Text = "캐니에지 켜짐";
			}

			else
			{
				button4.Text = "캐니에지 꺼짐";
			}
		}

		private async void button5_Click(object sender, EventArgs e)
		{
			isHistogramOn = !isHistogramOn;

			if (isHistogramOn)
			{
				button5.Text = "히스토그램 평활화 켜짐";
				isColor = false;

				Mat grayFrame = new Mat();

				while (isHistogramOn)
				{
					if (capture.IsOpened() && capture.Read(frame))
					{
						Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);
						Cv2.EqualizeHist(grayFrame, grayFrame);


						displayBitmap = BitmapConverter.ToBitmap(grayFrame);
						pictureBox1.Image = displayBitmap;
					}
					await Task.Delay(10);
				}
			}
			else
			{
				button5.Text = "히스토그램 평활화 꺼짐";
				isColor = true;
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			isCustomEffectOn = !isCustomEffectOn;

			if (isCustomEffectOn)
			{
				button6.Text = "모자이크 효과 끄기";
			}
			else
			{
				button6.Text = "모자이크 효과 켜기";
			}
		}

		private void button7_Click(object sender, EventArgs e)
		{
			if (isRunning && capture.IsOpened())
			{
				// 화면 좌우 반전 토글
				isFlipped = !isFlipped;

				// 화면 좌우 반전
				if (isFlipped)
				{
					Cv2.Flip(frame, frame, FlipMode.Y);
				}

				// 이미지 업데이트
				pictureBox1.Image = BitmapConverter.ToBitmap(frame);
			}
		}

		private void button8_Click(object sender, EventArgs e)
		{
			isSharpening = !isSharpening;
		}

		private Mat ApplySharpening(Mat inputImage)
		{
			// 샤프닝을 위한 커널 정의
			Mat kernel = new Mat(3, 3, MatType.CV_32F, new float[]
			{
			-1, -1, -1,
			-1,  9, -1,
			-1, -1, -1
			});

			// 커널을 사용하여 필터링
			Mat result = new Mat();
			Cv2.Filter2D(inputImage, result, inputImage.Depth(), kernel);

			// 결과 반환
			return result;
		}
		private void button9_Click(object sender, EventArgs e)
		{
			if (isConcaveMirror == false) isConcaveMirror = true;
			else if (isConcaveMirror == true) isConcaveMirror = false;

		}

		private void button10_Click(object sender, EventArgs e)
		{
			if (isConvexMirror == false) isConvexMirror = true;
			else if (isConvexMirror == true) isConvexMirror = false;


		}

		private void button11_Click(object sender, EventArgs e)
		{
			if (isRunning && capture.IsOpened())
			{
				// 화면 좌우 반전 토글
				isFlippedY = !isFlippedY;

				// 화면 좌우 반전
				if (isFlippedY)
				{
					Cv2.Flip(frame, frame, FlipMode.X);
				}

				// 이미지 업데이트
				pictureBox1.Image = BitmapConverter.ToBitmap(frame);
			}
		}

		private void button12_Click(object sender, EventArgs e)
		{
			if (isConvex == false) isConvex = true;
			else if (isConvex == true) isConvex = false;
		}

		private void button13_Click(object sender, EventArgs e)
		{
			if (Blur == false) Blur = true;
			else if (Blur == true) Blur = false;
		}


		//캐니
		private void button14_Click(object sender, EventArgs e)
		{
			if (increase_brightness == false) increase_brightness = true;
			else if (increase_brightness == true) increase_brightness = false;
		}

		private void button15_Click(object sender, EventArgs e)
		{
			if (lower_the_brightness == false) lower_the_brightness = true;
			else if (lower_the_brightness == true) lower_the_brightness = false;
		}
		private void button16_Click(object sender, EventArgs e)
		{
			isColor = false;
			embossing = !embossing;
		}
		private Mat ApplyEmbossing(Mat inputImage)
		{
			Mat kernel = new Mat(3, 3, MatType.CV_32F, new float[]
				   {
						-2, -1,  0,
						-1,  1,  1,
						 0,  1,  2
				   });

			Mat result = new Mat();
			Cv2.Filter2D(inputImage, result, inputImage.Depth(), kernel);

			return result;
		}
	}
}