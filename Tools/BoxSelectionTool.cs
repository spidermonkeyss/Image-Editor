using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor.Tools
{
    public class BoxSelectionTool : Tool
    {
        private Point boxInitalPoint;

        public BoxSelectionTool(Form1 form) : base(form)
        {
            Tool_Type = ToolType.BoxSelection;
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            boxInitalPoint = new Point(e.X, e.Y);
            form1.UpdateBoxSelction(e.X, e.Y, boxInitalPoint);
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            form1.UpdateBoxSelction(e.X, e.Y, boxInitalPoint);
        }
    }
}
