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

namespace VirtualBenchPro_v4
{
    /// <summary>
    /// Interaction logic for MB_CheckDongle.xaml
    /// </summary>
    public partial class MB_UsbUnavailable : Page
    {
        Window ParentWindow;
        public MB_UsbUnavailable(Window parent)
        {
            InitializeComponent();
            ParentWindow = parent;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Close();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            //Do something
        }
    }
}
