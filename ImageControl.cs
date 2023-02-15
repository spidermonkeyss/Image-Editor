﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor
{
    public class ImageControl : Control
    {
        //need four panel maybe
        //or do it all on one
        //i would like that better

        //checkered background
        //the image as a bitmap
        //the selection area

        //Put all these in imageBackgroundContainePanel
        //maybe make that the ImagePanel?
        //and the other three arent panels they just combine into the main panel
        
        public Bitmap imageBitmap;
        private Bitmap backgroundCheckerBitmap;
        private Panel backgroundCheckerPanel = new Panel();
        private Panel imagePanel = new Panel();

        Form1 form1;

        public bool isMouseDown = false;
        public int prevMouseX, prevMouseY;
      
        public ImageControl(Form1 form)
        {
            //This prevents flickering in the panel with the image
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, imagePanel, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, backgroundCheckerPanel, new object[] { true });

            form1 = form;

            form1.SuspendLayout();
            this.SuspendLayout();
            backgroundCheckerPanel.SuspendLayout();
            imagePanel.SuspendLayout();

            this.Size = new Size(500,300);
            this.Location = new Point(0, 0);
            this.Dock = DockStyle.None;
            this.TabIndex = 0;
            this.TabStop = false;
            this.Visible = true;
            this.Enabled = true;

            backgroundCheckerPanel.AutoSize = false;
            backgroundCheckerPanel.AutoScroll = false;
            backgroundCheckerPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
            backgroundCheckerPanel.Location = this.Location;
            backgroundCheckerPanel.Name = "Background Checker Panel";
            backgroundCheckerPanel.Size = this.Size;
            backgroundCheckerPanel.TabIndex = 0;
            backgroundCheckerPanel.TabStop = false;

            imagePanel.AutoSize = false;
            imagePanel.AutoScroll = false;
            imagePanel.AutoSizeMode = AutoSizeMode.GrowOnly;
            imagePanel.Location = this.Location;
            imagePanel.Name = "Image Panel";
            imagePanel.Size = this.Size;
            imagePanel.TabIndex = 0;
            imagePanel.TabStop = false;
            imagePanel.BackColor = Color.Transparent;
            imagePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownEvent);
            imagePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpEvent);
            imagePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveEvent);

            backgroundCheckerPanel.Controls.Add(imagePanel);
            this.Controls.Add(backgroundCheckerPanel);

            backgroundCheckerPanel.ResumeLayout(false);
            backgroundCheckerPanel.PerformLayout();
            imagePanel.ResumeLayout(false);
            imagePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            form1.ResumeLayout(false);
            form1.PerformLayout();
        }

        public void LoadMyImageFile(OpenFileDialog openFileDialog)
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
                return;
            }
            imageBitmap = new Bitmap(imageWidth, imageHeight);
            this.Size = imageBitmap.Size;

            int i = 0;
            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {

                    byte R = imageBytes[i + 0];
                    byte G = imageBytes[i + 1];
                    byte B = imageBytes[i + 2];
                    byte A = imageBytes[i + 3];
                    imageBitmap.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                    i += 4;
                }
            }
            fileStream.Close();

            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        public void LoadKnownFileType(string filePath)
        {
            imageBitmap = new Bitmap(filePath);
            this.Size = new Size(imageBitmap.Width, imageBitmap.Height);
            Console.WriteLine("PNG Loaded");
   
            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        public void ResizeImage(int width, int height)
        {
            Bitmap tempMap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x < imageBitmap.Width && y < imageBitmap.Height)
                        tempMap.SetPixel(x, y, imageBitmap.GetPixel(x, y));
                    else
                        tempMap.SetPixel(x, y, form1.clearColor);
                }
            }

            this.Size = new Size(width, height);
            imageBitmap = tempMap;

            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        public void NewImage()
        {
            imageBitmap = new Bitmap(this.Width, this.Height);
            this.Size = new Size(500, 300);

            for (int y = 0; y < imageBitmap.Height; y++)
            {
                for (int x = 0; x < imageBitmap.Width; x++)
                {
                    imageBitmap.SetPixel(x, y, form1.clearColor);
                }
            }

            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        private void UpdateTransparentBackground()
        {
            backgroundCheckerBitmap = new Bitmap(this.Width, this.Height);

            const int cellSize = 50;
            int currentCellWidth = 0;
            int currentCellHeight = 0;
            bool isWhiteCell = true;
            bool isFirstCellOfRowWhite = isWhiteCell;
            for (int y = 0; y < backgroundCheckerBitmap.Height; y++)
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

            backgroundCheckerPanel.Size = this.Size;
            backgroundCheckerPanel.BackgroundImage = backgroundCheckerBitmap;

            backgroundCheckerPanel.Invalidate();
            backgroundCheckerPanel.Update();
        }

        public void UpdatePanelImage()
        {
            imagePanel.Size = this.Size;
            imagePanel.BackgroundImage = imageBitmap;

            imagePanel.Invalidate();
            imagePanel.Update();
        }

        public void UpdateBoxSelction()
        {
            form1.selectionAreaTextBox.Text = "Selection Area:(" + form1.selectionArea.X + ", " + form1.selectionArea.Y + ") (" + form1.selectionArea.Right + ", " + form1.selectionArea.Bottom + ")";
        }

        public void RemoveBoxSelection()
        {
            form1.selectionArea.Size = new Size();
            form1.selectionArea.Location = new Point();
            form1.selectionAreaTextBox.Text = "";
        }

        /*----Events----*/
        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            Console.WriteLine("mouse down");
            form1.selectedTool.OnMouseDown(sender, e);
        }

        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            form1.selectedTool.OnMouseUp(sender, e);
        }

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                form1.selectedTool.OnMouseMove(sender, e);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //if (this.VScroll && (Control.ModifierKeys & Keys.Control) == Keys.Control)
            //{
            //    this.VScroll = false;
            //    base.OnMouseWheel(e);
            //    this.VScroll = true;
            //}
            //else
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                Console.WriteLine("Zoom");
            }
            else
            {
                Console.WriteLine("Scroll");
                base.OnMouseWheel(e);
            }
        }
    }
}