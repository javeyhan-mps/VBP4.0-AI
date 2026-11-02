using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MatchAddressWindow.xaml
    /// </summary>
    public partial class MatchAddressWindow : Window
    {
        public bool Back { get; set; }

        private MatchAddressView view;

        public MatchAddressWindow(Chip cp, bool canBack = true)
        {
            InitializeComponent();
            if (!canBack)
            {
                BackButton.Visibility = Visibility.Collapsed;
            }
            view = new MatchAddressView(cp);
            this.DataContext = view;
            Back = false;

        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Login_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var obj = e.Parameter as MatchAddressView;

            if (obj == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = (obj.AddressSelected != null);
        }

        private void Login_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (view.Public_Addr_ROhms_List.Count() != 0)
            {

                var temp_list = view.Public_Addr_ROhms_List.Where(o => o.Protocol == view.AddressSelected);
                Register_Map_DataGrid.ItemsSource = new ObservableCollection<AddrVsROhms>(temp_list); ;
                view.Register_Map_DataGrid_Address_Tile = view.AddressSelected;
               
               
                btnOK.Opacity = 0.5;
                btnOK.IsEnabled = false;

                var protocol_addr = view.Public_Addr_ROhms_List.FirstOrDefault(o => o.Protocol == view.AddressSelected);
               
                if (protocol_addr!=null&& protocol_addr.Address=="")
                {
                    DialogResult = true;
                }


                if (view.AddressSelected!="I2C"&& view.AddressSelected != "SPI" && view.AddressSelected != "UART")
                {
                    DialogResult = true;
                }
              

             
            }
            else
            {
                DialogResult = true;
            }
            
        }

      

        private void Register_Map_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecteditem = Register_Map_DataGrid.SelectedItem as AddrVsROhms;
            if (selecteditem == null)
                return;

            view.AddressSelected = selecteditem.Address;
            btnOK.Opacity = 1;
            btnOK.IsEnabled = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (view.Public_Addr_ROhms_List.Count() != 0)
            {
                var temptile = view.Register_Map_DataGrid_Address_Tile;
                Register_Map_DataGrid.ItemsSource = view.AddrVsROhmsList;
                view.Register_Map_DataGrid_Address_Tile = "Protocol";
                view.AddressSelected = null;
                btnOK.Opacity = 0.5;
                btnOK.IsEnabled = false;
                if (temptile == "Protocol") 
                {
                    Back = true;
                    Close();
                }

            }
            else
            {
                Back = true;
                Close();
            }
        }

        private void btnNoCOMOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        
    }
}
