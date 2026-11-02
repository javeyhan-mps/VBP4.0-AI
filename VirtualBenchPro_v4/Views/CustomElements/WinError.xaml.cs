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
using System.Windows.Shapes;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    /// <summary>
    /// Interaction logic for WinError.xaml
    /// </summary>
    public partial class WinError : Window
    {
        public WinError(string msg = null)
        {
            InitializeComponent();
            if (msg != null)
            {
                msg_tb.Text = msg;
            }
            else
            {
                msg_tb.Text = "An error occured, please try again.";
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
