using Microsoft.WindowsAPICodePack.Dialogs;
using MPS.Common.Model;
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
    public partial class System_View : System.Windows.Controls.Page
    {
        DeviceView dv;
        MainWindow mw;

        public System_View(ProjectConfig pobj, DeviceView devView, MainWindow mainView)
        {
            InitializeComponent();

            if (this.DataContext == null)
                this.DataContext = new SystemView(pobj, devView, mainView.LicenseInfo);

            dv = devView;
            mw = mainView;
            //Console.WriteLine(mainView.ProConfig.);
        }

        private void Data_Table_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid dt = sender as DataGrid;
           
            if (dt.Columns.Count > 0)
            {
                dt.Visibility = Visibility.Visible;
                dt.Columns[0].CellStyle = FindResource("FirstColumn") as Style;
                dt.Columns[0].HeaderStyle = FindResource("DGCHeaderStyle") as Style;
            }
            else
            {
                dt.Visibility = Visibility.Collapsed;
            }
           
        }

        private void OpenProjectClick(object sender, MouseButtonEventArgs e)
        {
            dv.OpenConfigMethod();
        }
        private void CreateNewProjectClick(object sender, MouseButtonEventArgs e)
        {
            mw.Project_Title = "<untitled>";
            mw.ChangeProjectView(1);
            //dv.RefreshDeiveMethod();
            //(this.DataContext as SystemView).AutoScanSelectMethod();
        }

        private void lstData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            mw.Device_Button_Click(sender, e);
        }

    }
}