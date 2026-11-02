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
using VirtualBenchPro_v4.ViewModel;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for ScanView.xaml
    /// </summary>
    public partial class ScanView : Window
    {
        public ScanView()
        {
            InitializeComponent();
            Loaded += ScanView_Loaded;
        }

        private void ScanView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ScanViewModel;
        }

        void OnChecked(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ScanViewModel;
            if(vm != null)
            {
                vm.RaisePropertyChanged(nameof(vm.AtLeastOneAddressChecked));
            }
        }

        private void Scan_Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
