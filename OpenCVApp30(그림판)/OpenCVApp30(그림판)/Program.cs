using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvSharp.XFeatures2D;

namespace OpenCVApp30_그림판_
{
    public static class IconFlags
    {
        public const int DRAW_RECTANGLE = 0;
        public const int DRAW_CIRCLE = 1;
        public const int DRAW_ELLIPSE = 2;
        public const int DRAW_LINE = 3;
        public const int DRAW_BRUSH = 4;
        public const int ERASE = 5;
        public const int OPEN = 6;
        public const int SAVE = 7;
        public const int PLUS = 8;
        public const int MINUS = 9;
        public const int CLEAR = 10;
        public const int COLOR = 11;
        public const int PALETTE = 12;
        public const int HUE_IDX = 13;
    }

    public class Menu
    {
        private int hue;
        private List<Rect> icons = new List<Rect>();
        private Mat image;

        public List<Rect> Icons => icons;
        public Mat Image => image;
        public int MouseMode { get; private set; } = 0; // 수정: MouseMode를 public으로 변경
        public int CommandMode { get; private set; } = 0; // 수정: CommandMode를 public으로 변경
        int drawMode = 0;
        Point pt1, pt2;

        public Menu()
        {
            image = new Mat(500, 800, MatType.CV_8UC3, Scalar.All(255));
        }

        public void PlaceIcons(Size size)
        {
            List<string> iconNames = new List<string>
            {
                "rect", "circle", "ellipse", "line", "brush", "eraser",
                "open", "save", "plus", "minus", "clear", "color"
            };

            int btnRows = (int)Math.Ceiling(iconNames.Count / 2.0);

            for (int i = 0, k = 0; i < btnRows; i++)
            {
                for (int j = 0; j < 2; j++, k++)
                {
                    Point pt = new Point(j * size.Width, i * size.Height);
                    icons.Add(new Rect(pt, size));

                    Mat icon = Cv2.ImRead(iconNames[k] + ".jpg", ImreadModes.Color);
                    if (icon.Empty()) continue;

                    Cv2.Resize(icon, icon, size);
                    icon.CopyTo(image.SubMat(icons[k]));
                }
            }
        }

        public void CreateHueIndex(Rect rect)
        {
            Mat hueIndex = image.SubMat(rect);
            float ratio = 180.0f / rect.Height;

            for (int i = 0; i < rect.Height; i++)
            {
                Scalar hueColor = new Scalar(i * ratio, 255, 255);
                hueIndex.Row(i).SetTo(hueColor);
            }
            Cv2.CvtColor(hueIndex, hueIndex, ColorConversionCodes.HSV2BGR);
        }

        public void CreatePalette(int posY, Rect paletteRect)
        {
            Mat palette = image.SubMat(paletteRect);
            float ratio1 = 180.0f / paletteRect.Height;
            float ratio2 = 256.0f / paletteRect.Width;
            float ratio3 = 256.0f / paletteRect.Height;

            hue = (int)Math.Round((posY - paletteRect.Top) * ratio1);

            for (int i = 0; i < palette.Rows; i++)
            {
                for (int j = 0; j < palette.Cols; j++)
                {
                    int saturation = (int)Math.Round(j * ratio2);
                    int intensity = (int)Math.Round((palette.Rows - i - 1) * ratio3);
                    palette.At<Vec3b>(i, j) = new Vec3b((byte)hue, (byte)saturation, (byte)intensity);
                }
            }
            Cv2.CvtColor(palette, palette, ColorConversionCodes.HSV2BGR);
        }

        public void OnMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userdata)
        {
            Point pt = new Point(x, y);
            if (@event == MouseEventTypes.LButtonUp)
            {
                Console.WriteLine("마우스 왼쪽 버튼 떼기");
                for (int i = 0; i < icons.Count; i++)
                {
                    if (icons[i].Contains(pt))
                    {
                        if (i < 6)
                        {
                            MouseMode = 0; // 수정: MouseMode를 변경
                            drawMode = i;
                        }
                        else
                            CommandMode = i; // 수정: CommandMode를 변경
                        return;
                    }
                }
                pt2 = pt;
                MouseMode = 1; // 수정: MouseMode를 변경
            }
            else if (@event == MouseEventTypes.LButtonDown)
            {
                pt1 = pt;
                MouseMode = 2; // 수정: MouseMode를 변경
                Console.WriteLine("마우스 왼쪽 버튼 누르기");
            }
            if (MouseMode >= 2) // 수정: MouseMode를 변경
            {
                Rect rect = new Rect(0, 0, 125, image.Rows);
                MouseMode = (rect.Contains(pt)) ? 0 : 3; // 수정: MouseMode를 변경
                pt2 = pt;
            }
        }

        public void Draw(Mat image, Scalar color = default)
        {
            if (color == default)
            {
                color = new Scalar(0, 0, 0);
            }

            if (drawMode == IconFlags.DRAW_RECTANGLE)
            {
                Cv2.Rectangle(image, pt1, pt2, color, -1, LineTypes.Link8);
            }
            else if (drawMode == IconFlags.DRAW_LINE)
            {
                Cv2.Line(image, pt1, pt2, color, 1, LineTypes.AntiAlias);
            }
            else if (drawMode == IconFlags.DRAW_BRUSH)
            {
                Cv2.Line(image, pt1, pt2, color, (1 * 3), LineTypes.Link8);
                pt1 = pt2;
            }
            else if (drawMode == IconFlags.ERASE)
            {
                Cv2.Line(image, pt1, pt2, new Scalar(255, 255, 255), (1 * 5), LineTypes.Link8);
                pt1 = pt2;
            }
            else if (drawMode == IconFlags.DRAW_CIRCLE)
            {
                Point2d pt3 = pt1 - pt2;
                int radius = (int)Math.Sqrt(pt3.X * pt3.X + pt3.Y * pt3.Y);
                Cv2.Circle(image, pt1, radius, color, -1, LineTypes.Link8);
            }
            else if (drawMode == IconFlags.DRAW_ELLIPSE)
            {
                Point center = new Point((int)((pt1.X + pt2.X) / 2.0), (int)((pt1.Y + pt2.Y) / 2.0));
                Size size = new Size(Math.Abs(pt1.X - pt2.X) / 2.0, Math.Abs(pt1.Y - pt2.Y) / 2.0);
                Cv2.Ellipse(image, center, size, 0, 0, 360, color, -1, LineTypes.AntiAlias);
            }

            Cv2.ImShow("그림판", image);
        }

        public void Command(int commandMode)
        {
            if (commandMode == IconFlags.PALETTE)
            {
                float ratio1 = 256.0f / icons[IconFlags.PALETTE].Height;
                float ratio2 = 256.0f / icons[IconFlags.PALETTE].Width;

                Point pt = pt2 - icons[IconFlags.PALETTE].TopLeft;
                int saturation = (int)Math.Round(pt.X * ratio2);
                int intensity = (int)Math.Round((icons[IconFlags.PALETTE].Height - pt.Y - 1) * ratio1);
                Scalar hsi = new Scalar(hue, saturation, intensity);

                Mat mColor = image.SubMat(icons[IconFlags.COLOR]);
                mColor.SetTo(hsi);
                Cv2.CvtColor(mColor, mColor, ColorConversionCodes.HSV2BGR);
                Cv2.Rectangle(image, icons[IconFlags.COLOR], new Scalar(0, 0, 0), 1);
            }
            else if (commandMode == IconFlags.PALETTE)
            {
                // MakePalette 메서드의 구현을 추가해야 합니다.
            }
            else if (commandMode == IconFlags.CLEAR)
            {
                image.SetTo(Scalar.All(255));
                MouseMode = 0; // 수정: MouseMode를 변경
            }
            else if (commandMode == IconFlags.OPEN)
            {
                Mat tmp = Cv2.ImRead("my_save.jpg", ImreadModes.Color);
                if (!tmp.Empty())
                {
                    Cv2.Resize(tmp, tmp, image.Size());
                    tmp.CopyTo(image);
                }
            }
            else if (commandMode == IconFlags.SAVE)
            {
                Cv2.ImWrite("my_save.jpg", image);
            }
            else if (commandMode == IconFlags.PLUS)
            {
                image += new Scalar(10, 10, 10);
            }
            else if (commandMode == IconFlags.MINUS)
            {
                image -= new Scalar(10, 10, 10);
            }

            Cv2.ImShow("그림판", image);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();

            menu.PlaceIcons(new Size(60, 60));

            Rect lastIcon = menu.Icons[menu.Icons.Count - 1];
            Point startPale = lastIcon.BottomRight + new Point(-120, 5);

            menu.Icons.Add(new Rect(startPale, new Size(100, 100)));
            menu.Icons.Add(new Rect(startPale + new Point(105, 0), new Size(15, 100))); // 수정: 괄호를 닫지 않음

            menu.CreateHueIndex(menu.Icons[IconFlags.HUE_IDX]);
            menu.CreatePalette(startPale.Y, menu.Icons[IconFlags.PALETTE]);

            Cv2.ImShow("그림판", menu.Image);

            Cv2.SetMouseCallback("그림판", (MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userdata) =>
            {
                menu.OnMouse(@event, x, y, flags, userdata);

                if (menu.MouseMode == 0)
                {
                    menu.Command(menu.CommandMode);
                }
                else
                {
                    menu.Draw(menu.Image);
                }

                Cv2.ImShow("그림판", menu.Image);
            });

            Cv2.WaitKey();
        }
    }
}
