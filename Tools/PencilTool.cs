using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor.Tools
{
    public class PencilTool : DrawTool
    {
        public PencilTool(Form1 form) : base(form)
        {
            Tool_Type = ToolType.Pencil;
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            DrawPixel(e.X, e.Y);
            form1.UpdatePanelImage();
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            DrawLineBetweenPoints(form1.prevMouseX, form1.prevMouseY, e.X, e.Y);
        }
    }
}
