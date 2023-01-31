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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int widthValue;
            int heightValue;
            bool isWidthNum = int.TryParse(textBox1.Text, out widthValue);
            bool isHeightNum = int.TryParse(textBox2.Text, out heightValue);
            if (isWidthNum && isHeightNum)
            {
                Console.WriteLine(widthValue);
                Console.WriteLine(heightValue);
                this.Close();
            }
            else
            {
                Console.WriteLine("NAN");
            }
        }
    }
}
