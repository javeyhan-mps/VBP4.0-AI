using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    public partial class Help_Icon : UserControl
    {
        public Help_Icon() { }
        public Help_Icon(bool mode, string text)
        {
            InitializeComponent();

            //mode true = Show text as Tooltip 
            if (mode)
            {
                Info_Icon.ToolTip = text;
            }
            //mode false = pop-up with image
            else
            {
                //Pop-up with image
            }
        }
    }
}
