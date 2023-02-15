using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ImageEditor.Tools
{
    public class BucketTool : DrawTool
    {
        public BucketTool(Form1 form) : base(form)
        {
            Tool_Type = ToolType.Bucket;
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            Fill(form1.selectionArea.Location, new Point(form1.selectionArea.Right, form1.selectionArea.Bottom));
            form1.imageControl.RemoveBoxSelection();
            form1.imageControl.UpdatePanelImage();
        }
    }
}
