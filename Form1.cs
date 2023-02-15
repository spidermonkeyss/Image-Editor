using ImageEditor.Tools;
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
        public Tool selectedTool;
        public Rectangle selectionArea;

        public Color drawColor = Color.Black;
        public Color clearColor = new Color();

        public ImageControl imageControl;
        
        public Form1()
        {
            InitializeComponent();
            clearColor = Color.Transparent;

            imageControl = new ImageControl(this);
            workspacePanel.Controls.Add(imageControl);
            imageControl.NewImage();

            drawRadioButton.Checked = true;
            selectedTool = new PencilTool(this);
            selectionArea = new Rectangle();

            DoubleBuffered = true;
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
                    imageControl.LoadKnownFileType(filePath);
                }
                //Load my own file type
                else
                {
                    imageControl.LoadMyImageFile(openFileDialog);
                }
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
                    string imageWidth = imageControl.imageBitmap.Width.ToString();
                    string imageHeight = imageControl.imageBitmap.Height.ToString();
                    
                    byte[] imageBytes = new byte[imageControl.imageBitmap.Width * imageControl.imageBitmap.Height * 4];
                    int i = 0;
                    for (int y = 0; y < imageControl.imageBitmap.Height; y++)
                    {
                        for (int x = 0; x < imageControl.imageBitmap.Width; x++)
                        {
                            imageBytes[i + 0] = imageControl.imageBitmap.GetPixel(x, y).R;
                            imageBytes[i + 1] = imageControl.imageBitmap.GetPixel(x, y).G;
                            imageBytes[i + 2] = imageControl.imageBitmap.GetPixel(x, y).B;
                            imageBytes[i + 3] = imageControl.imageBitmap.GetPixel(x, y).A;
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
            imageControl.NewImage();
        }

        //This will sometimes get called twice because one button is turning off then calling and one button is turning on then calling
        //Havent found any problems with this
        private void toolSelectionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (drawRadioButton.Checked)
            {
                selectedTool = new PencilTool(this);
                Console.WriteLine("Using pencil tool");
            }
            else if (eraseRadioButton.Checked)
            {
                selectedTool = new EraseTool(this);
                Console.WriteLine("Using erase tool");
            }
            else if (boxRadioButton.Checked)
            {
                selectedTool = new BoxSelectionTool(this);
                Console.WriteLine("Using box selection tool");
            }
            else if (bucketRadioButton.Checked)
            {
                selectedTool = new BucketTool(this);
                Console.WriteLine("Using bucket tool");
            }
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizeForm form2 = new ResizeForm(this, imageControl.imageBitmap.Width, imageControl.imageBitmap.Height);
            form2.Show();
        }
    }
}
