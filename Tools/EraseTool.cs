﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor.Tools
{
    public class EraseTool : DrawTool
    {
        public EraseTool(Form1 form) : base(form)
        {
            Tool_Type = ToolType.Erase;
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            drawColor = Color.FromArgb(0, 0, 0, 0);
            DrawPixel(e.X, e.Y);
            form1.UpdatePanelImage();
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            drawColor = Color.FromArgb(0, 0, 0, 0);
            DrawLineBetweenPoints(form1.prevMouseX, form1.prevMouseY, e.X, e.Y);
        }
    }
}
