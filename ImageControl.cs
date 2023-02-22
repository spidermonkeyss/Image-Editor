using System;
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
        public Bitmap imageBitmap;
        public Rectangle selectionArea = new Rectangle();
        public bool IsMouseDown { get; set; } = false;
        public int PrevMouseX { get; set; }
        public int PrevMouseY { get; set; }
        
        private Form1 form1;

        private Bitmap backgroundCheckerBitmap;
        private PictureBox backgroundCheckerPanel = new PictureBox();
        private PictureBox imagePanel = new PictureBox();
        private Panel selectionBoxPanel = new Panel();

        private float scale = 1;

        public ImageControl(Form1 form)
        {
            //This prevents flickering in the panel with the image
            typeof(PictureBox).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, imagePanel, new object[] { true });

            typeof(PictureBox).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, backgroundCheckerPanel, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, selectionBoxPanel, new object[] { true });

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
            backgroundCheckerPanel.SizeMode = PictureBoxSizeMode.Zoom;
            backgroundCheckerPanel.Location = this.Location;
            backgroundCheckerPanel.Name = "Background Checker Panel";
            backgroundCheckerPanel.Size = this.Size;
            backgroundCheckerPanel.TabIndex = 0;
            backgroundCheckerPanel.TabStop = false;

            imagePanel.AutoSize = false;
            imagePanel.SizeMode = PictureBoxSizeMode.Zoom;  
            imagePanel.Location = this.Location;
            imagePanel.Name = "Image Panel";
            imagePanel.Size = this.Size;
            imagePanel.TabIndex = 0;
            imagePanel.TabStop = false;
            imagePanel.BackColor = Color.Transparent;
            imagePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownEvent);
            imagePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpEvent);
            imagePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveEvent);

            selectionBoxPanel.AutoSize = false;
            selectionBoxPanel.AutoScroll = false;
            selectionBoxPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            selectionBoxPanel.Location = selectionArea.Location;
            selectionBoxPanel.Name = "Selection Box Panel";
            selectionBoxPanel.Size = selectionArea.Size;
            selectionBoxPanel.TabIndex = 0;
            selectionBoxPanel.TabStop = false;
            selectionBoxPanel.Enabled = false;
            selectionBoxPanel.BackColor = Color.FromArgb(100, 0, 0, 255);

            this.Controls.Add(backgroundCheckerPanel);
            backgroundCheckerPanel.Controls.Add(imagePanel);
            imagePanel.Controls.Add(selectionBoxPanel);

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

            if (!isWidth || !isHeight || imageBytes.Count % 4 != 0 || (imageWidth * imageHeight) != imageBytes.Count / 4)
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
                        tempMap.SetPixel(x, y, new Color());
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
                    imageBitmap.SetPixel(x, y, new Color());
                }
            }

            UpdateTransparentBackground();
            UpdatePanelImage();
        }

        private void UpdateImageControl()
        {
            UpdateTransparentBackground();
            UpdatePanelImage();
            UpdateBoxSelction();
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
            backgroundCheckerPanel.Image = backgroundCheckerBitmap;

            backgroundCheckerPanel.Invalidate();
            backgroundCheckerPanel.Update();
        }

        public void UpdatePanelImage()
        {
            imagePanel.Size = this.Size;
            imagePanel.Image = imageBitmap;

            imagePanel.Invalidate();
            imagePanel.Update();
        }

        public void UpdateBoxSelction()
        {
            form1.selectionAreaTextBox.Text = "Selection Area:(" + selectionArea.X + ", " + selectionArea.Y + ") (" + selectionArea.Right + ", " + selectionArea.Bottom + ")";

            selectionBoxPanel.Location = selectionArea.Location;
            selectionBoxPanel.Size = selectionArea.Size;

            selectionBoxPanel.Invalidate();
            selectionBoxPanel.Update();
        }

        public void RemoveBoxSelection()
        {
            selectionArea.Size = new Size();
            selectionArea.Location = new Point();

            selectionBoxPanel.Location = selectionArea.Location;
            selectionBoxPanel.Size = selectionArea.Size;

            form1.selectionAreaTextBox.Text = "";

            selectionBoxPanel.Invalidate();
            selectionBoxPanel.Update();
        }

        /*----Events----*/
        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            form1.selectedTool.OnMouseDown(sender, e);
        }

        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            form1.selectedTool.OnMouseUp(sender, e);
        }

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
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
                scale = 1 + (e.Delta * 0.0001f);
                Console.WriteLine(scale);
                Scale(new SizeF(scale, scale));
                UpdateImageControl();
            }
            else
            {
                Console.WriteLine("Scroll");
                base.OnMouseWheel(e);
            }
        }
    }
}
