using Microsoft.WindowsAPICodePack.Dialogs;
using MPS.Common.Model;
using MPS.Instrument;
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
using System.Xml.Linq;
using VirtualBenchPro_v4.ViewModel;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;
//using VirtualBenchPro_v4.Views.MonitorViews;

namespace VirtualBenchPro_v4.Views
{
    public partial class Evaluation_View : System.Windows.Controls.Page
    {
        
        public Evaluation_View(ProjectConfig pobj)
        {
            InitializeComponent();
            this.DataContext = new ProjectToolsView(pobj);

        }

        private void Run_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:eOrder@monolithicpower.com");
        }

        private void Run_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://www.monolithicpower.com/en/contact.html");
            switch (ConstantModel.mpsWebServer)
            {
                case "Production":
                    System.Diagnostics.Process.Start(ConstantModel.WSproductionURL + "en/contact.html");
                    break;
                case "Staging":
                    System.Diagnostics.Process.Start(ConstantModel.WSStagingURL + "en/contact.html");
                    break;
                default:
                    throw new ArgumentException("Invalid mpsWebServer value:" + ConstantModel.mpsWebServer);
            }

        }

      

        private void btnOS_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://www.monolithicpower.com/");
            switch (ConstantModel.mpsWebServer)
            {
                case "Production":
                    System.Diagnostics.Process.Start(ConstantModel.WSproductionURL);
                    break;
                case "Staging":
                    System.Diagnostics.Process.Start(ConstantModel.WSStagingURL);
                    break;
                default:
                    throw new ArgumentException("Invalid mpsWebServer value:" + ConstantModel.mpsWebServer);
            }
        }

        private void btnRD_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://www.monolithicpower.com/en/support/mps-now.html");
            switch (ConstantModel.mpsWebServer)
            {
                case "Production":
                    System.Diagnostics.Process.Start(ConstantModel.WSproductionURL + "en/support/mps-now.html");
                    break;
                case "Staging":
                    System.Diagnostics.Process.Start(ConstantModel.WSStagingURL + "en/support/mps-now.html");
                    break;
                default:
                    throw new ArgumentException("Invalid mpsWebServer value:" + ConstantModel.mpsWebServer);
            }
        }
    }
}