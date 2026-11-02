using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for WinAbout.xaml
    /// </summary>
    public partial class WinAbout : Window
    {
        public WinAbout()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.WinAboutView();
        }
    }
}
