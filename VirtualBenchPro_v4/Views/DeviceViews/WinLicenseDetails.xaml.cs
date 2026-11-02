using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for WinLicenseDetails.xaml
    /// </summary>
    public partial class WinLicenseDetails : Window
    {
        public LicenseInfo licenseInformation { get; set; }
        public bool RemoveFlg { get; set; }
        public WinLicenseDetails(LicenseInfo info)
        {
            InitializeComponent();
            licenseInformation = info;
            this.DataContext = info;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Change_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private string PATH = System.IO.Path.Combine(System.Environment.CurrentDirectory, "license.xml");
        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            
                RemoveFlg = true;
                this.DialogResult = false;
              
            
        }
    }
}
