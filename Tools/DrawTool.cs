using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor.Tools
{
    public abstract class DrawTool : Tool
    {
        protected Color drawColor;

        public DrawTool(Form1 form) : base(form)
        {
            drawColor = form1.drawColor;
        }

        public void SetDrawColor(Color color)
        {
            drawColor = color;
        }

        protected void DrawPixel(int x, int y)
        {
            if (x < 0 || x >= form1.imageControl.imageBitmap.Width)
            {
                Console.WriteLine("x out of panel");
                form1.imageControl.IsMouseDown = false;
                return;
            }
            if (y < 0 || y >= form1.imageControl.imageBitmap.Height)
            {
                Console.WriteLine("y out of panel");
                form1.imageControl.IsMouseDown = false;
                return;
            }

            form1.imageControl.imageBitmap.SetPixel(x, y, drawColor);

            form1.imageControl.PrevMouseX = x;
            form1.imageControl.PrevMouseY = y;
        }

        protected void DrawLineBetweenPoints(int startX, int startY, int endX, int endY)
        {
            float m, b;
            int botY, topY;

            //Mouse not moving left or right
            if (startX == endX)
            {
                //Mouse moving up
                if (startY < endY)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        DrawPixel(startX, y);
                    }
                }
                //Mouse moving down
                else
                {
                    for (int y = startY; y >= endY; y--)
                    {
                        DrawPixel(startX, y);
                    }
                }
            }
            //Mouse moving right
            else if (startX < endX)
            {
                m = (float)(endY - startY) / (float)(endX - startX);
                b = startY - (m * startX);
                //Mouse moving down
                if (startY < endY)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        botY = (int)Math.Floor((m * (x - 1)) + b);
                        topY = (int)Math.Floor((m * x) + b);
                        for (int y = botY; y <= topY; y++)
                        {
                            DrawPixel(x, y);
                        }
                    }
                }
                //Mouse moving up
                else
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        botY = (int)Math.Floor((m * x) + b);
                        topY = (int)Math.Floor((m * (x - 1)) + b);
                        for (int y = topY; y >= botY; y--)
                        {
                            DrawPixel(x, y);
                        }
                    }
                }
            }
            //Mouse moving left
            else
            {
                m = (float)(startY - endY) / (float)(startX - endX);
                b = startY - (m * startX);
                if (startY < endY)
                {
                    for (int x = startX; x >= endX; x--)
                    {
                        botY = (int)Math.Floor((m * (x + 1)) + b);
                        topY = (int)Math.Floor((m * x) + b);
                        for (int y = botY; y <= topY; y++)
                        {
                            DrawPixel(x, y);
                        }
                    }
                }
                //Mouse moving up
                else
                {
                    for (int x = startX; x >= endX; x--)
                    {
                        botY = (int)Math.Floor((m * x) + b);
                        topY = (int)Math.Floor((m * (x + 1)) + b);
                        for (int y = topY; y >= botY; y--)
                        {
                            DrawPixel(x, y);
                        }
                    }
                }
            }

            form1.imageControl.UpdatePanelImage();
        }

        protected void Fill(Point topLeft, Point botRight)
        {
            for (int y = topLeft.Y; y < botRight.Y; y++)
            {
                for (int x = topLeft.X; x < botRight.X; x++)
                {
                    DrawPixel(x, y);
                }
            }
        }
    }
}
