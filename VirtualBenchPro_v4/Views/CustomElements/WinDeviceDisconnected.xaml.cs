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
    /// Interaction logic for WinDeviceDisconnected.xaml
    /// </summary>
    public partial class WinDeviceDisconnected : Window
    {
        public WinDeviceDisconnected()
        {
            InitializeComponent();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e) => Close();
    }
}
