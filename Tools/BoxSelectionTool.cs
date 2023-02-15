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
            form1.imageControl.RemoveBoxSelection();
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point topLeft = new Point(Math.Min(e.X, boxInitalPoint.X), Math.Min(e.Y, boxInitalPoint.Y));
            Point botRight = new Point(Math.Max(e.X, boxInitalPoint.X), Math.Max(e.Y, boxInitalPoint.Y));

            form1.imageControl.selectionArea.Size = new Size(botRight.X - topLeft.X, botRight.Y - topLeft.Y);
            form1.imageControl.selectionArea.Location = topLeft;

            if (form1.imageControl.selectionArea.Size.IsEmpty)
            {
                form1.imageControl.RemoveBoxSelection();
            }
            else
            {
                form1.imageControl.UpdateBoxSelction();
            }
        }
    }
}
