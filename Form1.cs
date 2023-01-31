using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ImageEditor
{
    public partial class Form1 : Form
    {
        Bitmap panelImageBitmap;
        bool isMouseDraw = false;
        int prevMouseX, prevMouseY;
        Color drawColor = Color.Black;

        public Form1()
        {
            InitializeComponent();
            NewImage();
        }

        private void NewImage()
        {
            panelImageBitmap = new Bitmap(256, 64);
            panel1.Size = new Size(256, 64);

            for (int y = 0; y < panel1.Height; y++)
            {
                for (int x = 0; x < panel1.Width; x++)
                {
                    panelImageBitmap.SetPixel(x, y, Color.White);
                }
            }
            UpdatePanelImage();

            //This prevents flickering in the panel with the image
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, panel1, new object[] { true });
        }

        private void UpdatePanelImage()
        {
            panel1.BackgroundImage = panelImageBitmap;

            panel1.Invalidate();
            panel1.Update();
        }

        private void DrawPixel(int x, int y)
        {
            if (x < 0 || x >= panelImageBitmap.Width)
            {
                Console.WriteLine("x out of panel");
                isMouseDraw = false;
                return;
            }
            if (y < 0 || y >= panelImageBitmap.Height)
            {
                Console.WriteLine("y out of panel");
                isMouseDraw = false;
                return;
            }

            panelImageBitmap.SetPixel(x, y, drawColor);
            prevMouseX = x;
            prevMouseY = y;

            UpdatePanelImage();
        }

        private void DrawLineBetweenPoints(int startX, int startY, int endX, int endY)
        {
            float m, b;
            int botY, topY;

            //Mouse not moving left or right
            if (startX == endX)
            {
                Console.Write("e");
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
        }

        /*--PanelImage stuff--*/
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDraw = true;
            DrawPixel(e.X, e.Y);
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDraw = false;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDraw)
            {
                DrawLineBetweenPoints(prevMouseX, prevMouseY ,e.X, e.Y);
            }
        }

        private void pickColorBtn_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = drawColor;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                drawColor = MyDialog.Color;
                colorDisplayTextBox.BackColor = drawColor;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:/Users/Student/Desktop";
            openFileDialog.Filter = "myimage files (*.myimage)|*.myimage";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;
                //Read the contents of the file into a stream
                Stream fileStream = openFileDialog.OpenFile();
                
                fileStream.Position = 0;
                List<byte> imageWidthBytes = new List<byte>();
                List<byte> imageHeightBytes = new List<byte>();
                List<byte> imageBytes = new List<byte>();
                int currentList = 0;
                while (fileStream.Position < fileStream.Length)
                {
                    int intByte = fileStream.ReadByte();
                    if (intByte == -1)
                    {
                        break;
                    }
                    byte b = (byte)intByte;
                    //32 is space
                    if (b == 32)
                    {
                        currentList++;
                    }
                    else
                    {
                        switch (currentList)
                        {
                            case 0:
                                imageWidthBytes.Add(b);
                                break;
                            case 1:
                                imageHeightBytes.Add(b);
                                break;
                            case 2:
                                imageBytes.Add(b);
                                break;
                        }
                    }
                }

                if (currentList > 2)
                {
                    Console.WriteLine("Open file failed. Could not read properly");
                    fileStream.Close();
                    return;
                }

                int imageWidth = int.Parse(Encoding.UTF8.GetString(imageWidthBytes.ToArray()));
                int imageHeight = int.Parse(Encoding.UTF8.GetString(imageHeightBytes.ToArray()));
                panelImageBitmap = new Bitmap(imageWidth, imageHeight);
                panel1.Size = new Size(imageWidth, imageHeight);

                int i = 0;
                for (int y = 0; y < imageHeight; y++)
                {
                    for (int x = 0; x < imageWidth; x++)
                    {

                        byte R = imageBytes[i + 0];
                        byte G = imageBytes[i + 1];
                        byte B = imageBytes[i + 2];
                        byte A = imageBytes[i + 3];
                        panelImageBitmap.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                        i += 4;
                    }
                }
                fileStream.Close();
                UpdatePanelImage();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "myimage files (*.myimage)|*.myimage";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    StreamReader reader = new StreamReader(myStream);
                    // Code to write the stream goes here.
                    string imageWidth = panelImageBitmap.Width.ToString();
                    string imageHeight = panelImageBitmap.Height.ToString();
                    
                    byte[] imageBytes = new byte[panelImageBitmap.Width * panelImageBitmap.Height * 4];
                    int i = 0;
                    for (int y = 0; y < panelImageBitmap.Height; y++)
                    {
                        for (int x = 0; x < panelImageBitmap.Width; x++)
                        {
                            imageBytes[i + 0] = panelImageBitmap.GetPixel(x, y).R;
                            imageBytes[i + 1] = panelImageBitmap.GetPixel(x, y).G;
                            imageBytes[i + 2] = panelImageBitmap.GetPixel(x, y).B;
                            imageBytes[i + 3] = panelImageBitmap.GetPixel(x, y).A;
                            i += 4;
                        }
                    }

                    myStream.Write(Encoding.UTF8.GetBytes(imageWidth), 0, imageWidth.Length);
                    myStream.Write(Encoding.UTF8.GetBytes(" "), 0, 1);
                    myStream.Write(Encoding.UTF8.GetBytes(imageHeight), 0, imageWidth.Length);
                    myStream.Write(Encoding.UTF8.GetBytes(" "), 0, 1);
                    myStream.Write(imageBytes, 0, imageBytes.Length);
                    myStream.Close();
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewImage();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
