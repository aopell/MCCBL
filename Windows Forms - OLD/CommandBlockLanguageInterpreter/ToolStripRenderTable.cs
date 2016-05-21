using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandBlockLanguageInterpreter
{
    public class ToolStripRenderTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return Color.White;
            }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color ToolStripBorder
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color MenuBorder
        {
            get { return Color.DimGray; }
        }

    }
}
