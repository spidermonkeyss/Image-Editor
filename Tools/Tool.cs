using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor.Tools
{
    public abstract class Tool
    {
        public enum ToolType { Pencil, Erase, BoxSelection, Bucket}
        public ToolType Tool_Type { get; protected set; }

        protected Form1 form1;

        public Tool(Form1 form)
        {
            form1 = form;
        }

        public virtual void OnMouseDown(object sender, MouseEventArgs e)
        {

        }

        public virtual void OnMouseUp(object sender, MouseEventArgs e)
        {

        }

        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
