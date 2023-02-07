﻿using ImageEditor.Tools;
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

namespace ImageEditor
{
    public partial class Form1 : Form
    {
        private Tool selectedTool;
        public Rectangle selectionArea;

        public Bitmap panelImageBitmap;
        private Bitmap backgroundCheckerBitmap;

        public bool isMouseDown = false;
        public int prevMouseX, prevMouseY;

        public Color drawColor = Color.Black;
        private Color clearColor = new Color();

        public Form1()
        {
            InitializeComponent();
            NewImage();
            
            imagePanel.BackColor = Color.Transparent;
            drawRadioButton.Checked = true;
            selectedTool = new PencilTool(this);
            selectionArea = new Rectangle();

            DoubleBuffered = true;

            //This prevents flickering in the panel with the image
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, imagePanel, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, boxSelectionPanel, new object[] { true });
        }

        private bool LoadMyImageFile(OpenFileDialog openFileDialog)
        {
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
                if (b == 32 && currentList < 2)
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

            int imageWidth;
            int imageHeight;
            bool isWidth = int.TryParse(Encoding.UTF8.GetString(imageWidthBytes.ToArray()), out imageWidth);
            bool isHeight = int.TryParse(Encoding.UTF8.GetString(imageHeightBytes.ToArray()), out imageHeight);

            if ((!isWidth || !isHeight) || imageBytes.Count % 4 != 0 || (imageWidth * imageHeight) != imageBytes.Count / 4)
            {
                Console.WriteLine("Open file failed. Could not read properly");
                fileStream.Close();
                return false;
            }
            panelImageBitmap = new Bitmap(imageWidth, imageHeight);
            imagePanel.Size = new Size(imageWidth, imageHeight);

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
            return true;
        }

        public void ResizeImage(int width, int height)
        {
            Bitmap tempMap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x < panelImageBitmap.Width && y < panelImageBitmap.Height)
                        tempMap.SetPixel(x, y, panelImageBitmap.GetPixel(x,y));
                    else
                        tempMap.SetPixel(x, y, clearColor);
                }
            }

            imagePanel.Size = new Size(width, height);
            panelImageBitmap = tempMap;

            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        private void NewImage()
        {
            panelImageBitmap = new Bitmap(500, 300);
            imagePanel.Size = new Size(500, 300);

            for (int y = 0; y < imagePanel.Height; y++)
            {
                for (int x = 0; x < imagePanel.Width; x++)
                {
                    panelImageBitmap.SetPixel(x, y, clearColor);
                }
            }
            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        private void UpdateTransparentBackground()
        {
            checkeredPanel.Size = imagePanel.Size;
            checkeredPanel.Location = imagePanel.Location;

            backgroundCheckerBitmap = new Bitmap(panelImageBitmap.Width, panelImageBitmap.Height);

            const int cellSize = 50;
            int currentCellWidth = 0;
            int currentCellHeight = 0;
            bool isWhiteCell = true;
            bool isFirstCellOfRowWhite = isWhiteCell;
            for(int y = 0; y < backgroundCheckerBitmap.Height; y++)
            {
                for (int x = 0; x < backgroundCheckerBitmap.Width; x++)
                {
                    if (isWhiteCell)
                    {
                        backgroundCheckerBitmap.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        backgroundCheckerBitmap.SetPixel(x, y, Color.LightGray);
                    }

                    currentCellWidth++;
                    if (currentCellWidth == cellSize)
                    {
                        currentCellWidth = 0;
                        isWhiteCell = !isWhiteCell;
                    }
                }
                
                currentCellWidth = 0;

                //Make sure cell is same color till new cell
                isWhiteCell = isFirstCellOfRowWhite;

                currentCellHeight++;
                if (currentCellHeight == cellSize)
                {
                    //Make sure first cell of row is oppsite of cell above
                    isWhiteCell = !isFirstCellOfRowWhite;
                    isFirstCellOfRowWhite = isWhiteCell;
                    currentCellHeight = 0;
                }
            }

            checkeredPanel.BackgroundImage = backgroundCheckerBitmap;

            imagePanel.Invalidate();
            imagePanel.Update();
        }

        public void UpdatePanelImage()
        {
            imagePanel.BackgroundImage = panelImageBitmap;

            imagePanel.Invalidate();
            imagePanel.Update();
        }

        public void UpdateBoxSelction()
        {
            boxSelectionPanel.BackColor = Color.FromArgb(100, 0, 0, 255);
            boxSelectionPanel.Visible = true;
            
            boxSelectionPanel.Size = selectionArea.Size;
            boxSelectionPanel.Location = selectionArea.Location;

            selectionAreaTextBox.Text = "Selection Area:(" + selectionArea.X + ", " + selectionArea.Y + ") (" + selectionArea.Right + ", " + selectionArea.Bottom + ")";

            boxSelectionPanel.Invalidate();
            boxSelectionPanel.Update();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            selectedTool.OnMouseDown(sender, e);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            selectedTool.OnMouseUp(sender, e);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                selectedTool.OnMouseMove(sender, e);
            }
        }

        private void pickColorBtn_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            //Custom colors
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = drawColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                drawColor = colorDialog.Color;
                colorDisplayTextBox.BackColor = drawColor;

                if (selectedTool.GetType().IsSubclassOf(typeof(DrawTool)))
                {
                    ((DrawTool)selectedTool).SetDrawColor(drawColor);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:/Users/Student/Pictures";
            openFileDialog.Filter = "myimage files (*.myimage)|*.myimage|png files (*.png)|*.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;
                string extension = Path.GetExtension(filePath);

                //Load known file types
                if (extension == ".png")
                {
                    panelImageBitmap = new Bitmap(filePath);
                    imagePanel.Size = new Size(panelImageBitmap.Width, panelImageBitmap.Height);
                    Console.WriteLine("PNG Loaded");
                }
                //Load my own file type
                else
                {
                    LoadMyImageFile(openFileDialog);
                }

                UpdateTransparentBackground();
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

        private void toolSelectionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (drawRadioButton.Checked)
            {
                selectedTool = new PencilTool(this);
            }
            else if (eraseRadioButton.Checked)
            {
                selectedTool = new EraseTool(this);
            }
            else if (boxRadioButton.Checked)
            {
                selectedTool = new BoxSelectionTool(this);
            }
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizeForm form2 = new ResizeForm(this, panelImageBitmap.Width, panelImageBitmap.Height);
            form2.Show();
        }
    }
}
