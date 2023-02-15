using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor
{
    public partial class ResizeForm : Form
    {
        private Form1 mainForm;

        public ResizeForm(Form1 form1, int currentImageWidth, int currentImageHeight)
        {
            mainForm = form1;
            InitializeComponent();
            widthInputTextBox.Text = currentImageWidth.ToString();
            heightInputTextBox.Text = currentImageHeight.ToString();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int widthValue;
            int heightValue;
            bool isWidthNum = int.TryParse(widthInputTextBox.Text, out widthValue);
            bool isHeightNum = int.TryParse(heightInputTextBox.Text, out heightValue);

            if (isWidthNum && isHeightNum)
            {
                mainForm.imageControl.ResizeImage(widthValue, heightValue);
                this.Close();
            }
            else
            {
                Console.WriteLine("NAN");
            }
        }
    }
}
