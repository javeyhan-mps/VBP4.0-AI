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

namespace VirtualBenchPro_v4.Views.DeviceViews
{

    public partial class WriteROMWarningWindow : Window
    {
        public bool IsDontShowAgainChecked { get; set; }

        public Visibility ReadbackContentVisibility { get; set; }

        public WriteROMWarningWindow(bool isReadbackRelatedContentShow = false)
        {
            InitializeComponent();
            DataContext = this;

            if (isReadbackRelatedContentShow)
            {
                ReadbackContentVisibility = Visibility.Visible;
            }
            else
            {
                ReadbackContentVisibility = Visibility.Collapsed;
                Height = 210;
            }
        }

        private void OK_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
