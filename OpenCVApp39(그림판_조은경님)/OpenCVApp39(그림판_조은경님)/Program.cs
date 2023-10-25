using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PaintBrush_01

{
	public static class IconFlag
	{
		public const int DRAW_RECTANGLE = 0;
		public const int DRAW_CIRCLE = 1;
		public const int DRAW_ECLIPSE = 2;
		public const int DRAW_LINE = 3;
		public const int DRAW_BRUSH = 4;


		public const int ERASE = 5;
		public const int OPEN = 6;
		public const int SAVE = 7;
		public const int PLUS = 8;
		public const int MINUS = 9;
		public const int CREAR = 10;
		public const int COLOR = 11;
		public const int PALETTE = 12;
		public const int PALE_IDX = 13;


	}

	public class Palette
	{
		private int hue;    //색상
		private List<Rect> icons = new List<Rect>();   //아이콘 사각형틀
		private Mat image = new Mat();          //그림판 영상
		private Mat canvas = new Mat();

		private Point palette_pt = new Point();
		private Point pt1 = new Point();
		private Point pt2 = new Point();
		private Scalar Color = new Scalar(0, 0, 0); //색상 변수 

		public int mouse_mode = 0;
		public int draw_mode = 0;
		public int command_mode = 0;
		public int thickness = 3;

		//외부에서 접속
		public List<Rect> Icons => icons;
		public Mat Image => image;
		public Scalar color => Color;
		public Point Palette_pt => palette_pt;

		public void SetImage(Mat img)
		{
			this.image = img;
		}

		public void SetCanvas(Rect rect)
		{
			this.canvas = image.SubMat(rect);
		}

		public void SetPalette_pt(Point pt)
		{
			this.palette_pt = pt;
		}

		public void place_icons(Size size)
		{
			List<string> icon_name = new List<string>() //아이콘 파일 이름
            {
				"rect", "circle", "ellipse", "line", "brush", "eraser",

				"open", "save", "plus", "minus", "clear", "color"
			};


			int btn_rows = (int)Math.Ceiling(icon_name.Count / 2.0); //2열 버튼의 행수 - 올림

			for (int i = 0, k = 0; i < btn_rows; i++)
			{
				for (int j = 0; j < 2; j++, k++)
				{
					Point pt = new Point(j * size.Width, i * size.Height); //각 아이콘 시작 위치
					icons.Add(new Rect(pt, size));                         //각 아이콘 크기

					Mat icon = Cv2.ImRead(icon_name[k] + ".jpg", ImreadModes.Color);
					if (icon.Empty()) continue; //예외처리


					Cv2.Resize(icon, icon, size); //사이즈 크기 조정
					icon.CopyTo(image.SubMat(icons[k])); //아이콘  복사 - 이해 불가능 왜 image에 []를 붙이지?
				}
			}
		}
		public void create_hueIndex(Rect rect) //색상 인덱스 rect
		{
			Mat m_hueIdx = image.SubMat(rect); //원하는 부분 추출
			float ratio = 180.0f / rect.Height; //세로 크기의 hue영역 (색상)


			for (int i = 0; i < rect.Height; i++)
			{
				Scalar hue_color = new Scalar(i * ratio, 255, 255); //HSV 색상 지정
				m_hueIdx.Row(i).SetTo(hue_color);                   //한행에 그 색상을 지정
			}
			Cv2.CvtColor(m_hueIdx, m_hueIdx, ColorConversionCodes.HSV2BGR);
		}

		public void create_palette(int pos_y, Rect rect_pale) //pos_y : 마우스 클릭 y좌표, rect_pale: 팔레트 영역
		{
			Mat m_palatte = image.SubMat(rect_pale); //원하는 부분 추출
			float ratio1 = 180.0f / rect_pale.Height; //hue 비율
			float ratio2 = 256.0f / rect_pale.Width;  //saturation 비율(채도)
			float ratio3 = 256.0f / rect_pale.Height; //intensity 비율 (명암


			hue = (int)Math.Round((pos_y - rect_pale.Y) * ratio1);   //색상 - 반올림
			for (int i = 0; i < m_palatte.Rows; i++)
			{
				for (int j = 0; j < m_palatte.Cols; j++)
				{
					int saturation = (int)Math.Round(j * ratio2); //채도 계산
					int intensity = (int)Math.Round((m_palatte.Rows - i - 1) * ratio3); //명도 계산
					m_palatte.At<Vec3b>(i, j) = new Vec3b((byte)hue, (byte)saturation, (byte)intensity);
				}
			}
			Cv2.CvtColor(m_palatte, m_palatte, ColorConversionCodes.HSV2BGR);
		}

		public void command(int command_mode) //일반 명령 수행
		{
			Console.WriteLine($"코멘드를 불렀습니다. : {command_mode}");

			if (command_mode == IconFlag.PALETTE)
			{
				float ratio1 = 256.0f / Icons[IconFlag.PALETTE].Height; //높이로 명도 비율
				float ratio2 = 256.0f / Icons[IconFlag.PALETTE].Width; //너비로 채도 비율


				Point pt = pt2 - palette_pt; //바뀐 것
				//Point pt = pt2 - Icons[IconFlag.PALETTE].BottomRight; //팔레트 내 클릭좌표
				//Point pt = pt2 - Icons[IconFlag.PALETTE].TopLeft;
				int saturation = (int)Math.Round(pt.X * ratio2);
				int intensity = (int)Math.Round((Icons[IconFlag.PALETTE].Height - pt.Y - 1) * ratio1);
				Scalar hsi = new Scalar((byte)hue, (byte)saturation, (byte)intensity);

				Mat m_color = Image.SubMat(Icons[IconFlag.COLOR]); // 색상 아이콘 참조
				m_color.SetTo(hsi);
				Cv2.CvtColor(m_color, m_color, ColorConversionCodes.HSV2BGR);
				Cv2.Rectangle(Image, Icons[IconFlag.COLOR], new Scalar(0, 0, 0), 1); //사각형을 그린다.
				Vec3b vec = m_color.At<Vec3b>(10, 10);
				Color = new Scalar(vec.Item0, vec.Item1, vec.Item2);
			}
			else if (command_mode == IconFlag.PALE_IDX)
			{
				create_palette(pt2.Y, Icons[IconFlag.PALETTE]); //팔레트 다시 그리기
			}
			else if (command_mode == IconFlag.CREAR)
			{
				canvas.SetTo(Scalar.All(255));
				mouse_mode = 0;
			}
			else if (command_mode == IconFlag.OPEN)
			{
				Mat tmp = Cv2.ImRead("my_save.jpg", ImreadModes.Color);
				if (tmp == null || tmp.Empty())
				{
					Console.WriteLine("파일이 존재하지 않습니다.");
				}

				Cv2.Resize(tmp, tmp, canvas.Size());
				tmp.CopyTo(canvas);
			}
			else if (command_mode == IconFlag.SAVE)
			{
				Cv2.ImWrite("my_save.jpg", canvas);
			}
			else if (command_mode == IconFlag.PLUS)
			{
				//canvas = canvas.Clone() + Scalar.All(10);
				Cv2.Add(canvas, Scalar.All(10), canvas);
			}
			else if (command_mode == IconFlag.MINUS)
			{
				//canvas = canvas.Clone() -  Scalar.All(10);
				Cv2.Add(canvas, Scalar.All(-10), canvas);
			}

			Cv2.ImShow("image", image);
		}
		public void onMouse(MouseEventTypes eventTypes, int x, int y, MouseEventFlags flags, IntPtr userdata)
		{

			Point pt = new Point(x, y);
			if (eventTypes == MouseEventTypes.LButtonUp) //왼쪽 버튼 떼기
			{
				for (int i = 0; i < Icons.Count; i++) // 메뉴 아이콘 사각형 조회
				{
					if (Icons[i].Contains(pt)) //메뉴 클릭 여부 검사
					{
						if (i < 6)
						{
							mouse_mode = 0; //마우스 상태 초기화
							draw_mode = i;  //그리기 모드

						}
						else command(i); //일반 명령이면
						return;
					}
				}
				pt2 = pt; //종료 좌표 저장
				mouse_mode = 1; //벝튼 떼기 상태 지정
				Draw(image, Color);
			}
			else if (eventTypes == MouseEventTypes.LButtonDown)
			{
				pt1 = pt; //시작 좌표 저장
				mouse_mode = 2;

				Console.WriteLine("왼쪽 마우스가 클릭되었습니다.");
			}

			if (mouse_mode >= 2) //왼쪽 버튼 누르기 or 드래그
			{
				Rect rect = new Rect(0, 0, 125, Image.Rows);
				mouse_mode = rect.Contains(pt) ? 0 : 3; //마우스 상태 지정
				pt2 = pt;
			}
		}
		public void Draw(Mat Drawimage, Scalar color) //Mat image, Scalar color = new Scalar(200,200,200)
		{
			//Scalar color = new Scalar(200,200,200);

			switch (draw_mode)
			{
				case IconFlag.DRAW_RECTANGLE:
					Cv2.Rectangle(Drawimage, pt1, pt2, color, thickness);
					break;
				case IconFlag.DRAW_LINE:
					Cv2.Line(Drawimage, pt1, pt2, color, thickness);
					break;
				case IconFlag.DRAW_BRUSH:
					Cv2.Line(Drawimage, pt1, pt2, color, thickness * 3);
					pt1 = pt2;
					break;
				case IconFlag.ERASE:
					Cv2.Line(Drawimage, pt1, pt2, Scalar.All(255), thickness * 5);
					pt1 = pt2;
					break;
				case IconFlag.DRAW_CIRCLE:
					Point2d pt3 = pt1 - pt2;
					int radius = (int)Math.Sqrt(pt3.X * pt3.X + pt3.Y * pt3.Y); //두 좌표간 거리
					Cv2.Circle(Drawimage, pt1, radius, color, thickness);
					break;
				case IconFlag.DRAW_ECLIPSE:
					Point center = new Point((pt1.X + pt2.X) / 2.0, (pt1.Y + pt2.Y) / 2.0);
					Size size = new Size((pt1.X - pt2.X) / 2.0, (pt1.Y - pt2.Y) / 2.0);
					size.Width = Math.Abs(size.Width);
					size.Height = Math.Abs(size.Height);
					Cv2.Ellipse(Drawimage, center, size, 0, 0, 360, color, thickness);
					break;
			}
			Cv2.ImShow("image", image);
		}
		public void onTrackbar(int value, IntPtr userdata)
		{
			mouse_mode = 0;
		}
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			Palette menu = new Palette();

			Mat image = new Mat(500, 800, MatType.CV_8UC3, Scalar.All(255));
			menu.SetImage(image);

			menu.place_icons(new Size(60, 60));

			Rect last_icon = menu.Icons.Last();                           //아이콘 사각형 마지막 원소
			//Point start_pale = new Point(0, last_icon.BottomRight.Y + 5); //팔레트 시작 위치
			Point pt = new Point(0, last_icon.BottomRight.Y + 5);
			menu.SetPalette_pt(pt);

			menu.Icons.Add(new Rect(menu.Palette_pt, new Size(100, 100)));     //팔레트 사각형 추가
			menu.Icons.Add(new Rect(menu.Palette_pt + new Point(105, 0), new Size(15, 100))); //색상 인덱스 사각형
			//menu.Icons.Add(new Rect(start_pale, new Size(100, 100)));     //팔레트 사각형 추가
			//menu.Icons.Add(new Rect(start_pale + new Point(105, 0), new Size(15, 100))); //색상 인덱스 사각형

			menu.create_hueIndex(menu.Icons[IconFlag.PALE_IDX]);           //팔레트 생성
			//menu.create_palette(start_pale.Y, menu.Icons[IconFlag.PALETTE]); //색상 인텍스 생성
			menu.create_palette(menu.Palette_pt.Y, menu.Icons[IconFlag.PALETTE]); //색상 인텍스 생성

			Cv2.ImShow("image", menu.Image);
			MouseCallback mouseCallback = new MouseCallback(menu.onMouse);
			Cv2.SetMouseCallback("image", mouseCallback, menu.Image.CvPtr);

			Cv2.CreateTrackbar("thickness", "image", ref menu.thickness, 20, menu.onTrackbar); //트랙바 등록
			int x = menu.Icons[1].BottomRight.X; //두번째 아이콘 x좌표
			Rect canvas_rect = new Rect(x, 0, menu.Image.Cols - x, menu.Image.Rows);
			menu.SetCanvas(canvas_rect);

			while (true)
			{
				if (menu.mouse_mode == 1)                // 마우스 버튼 떼기
					menu.Draw(image, menu.color);
				else if (menu.mouse_mode == 3)           // 마우스 드래그
				{
					if (menu.draw_mode == IconFlag.DRAW_BRUSH || menu.draw_mode == IconFlag.ERASE)
					{
						menu.Draw(image, menu.color);
					}
					else
					{
						menu.Draw(image.Clone(), Scalar.All(200));
					}
				}
				if (Cv2.WaitKey(30) == 27) break;
			}

			Cv2.WaitKey();
		}
	}
}