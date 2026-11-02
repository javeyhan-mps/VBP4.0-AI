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
using System.Windows.Threading;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    /// <summary>
    /// Interaction logic for SquareProgressBar.xaml
    /// </summary>
    public partial class SquareProgressBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularProgressBar"/> class.
        /// </summary>
        public SquareProgressBar()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                // ignored
            }
        }


    }
}
