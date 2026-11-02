using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VirtualBenchPro_v4.ViewModel;
using static VirtualBenchPro_v4.Views.Device;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for WinDetailedRegisterMap.xaml
    /// </summary>
    public partial class WinDetailedRegisterMap : Window
    {
        DetailedRegisterMapView view;
        public WinDetailedRegisterMap(Chip _currentChip, ObservableCollection<LogModel> _logData)
        {
            InitializeComponent();
            view = new DetailedRegisterMapView(_currentChip, _logData, this);
            DataContext = view;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            var viewmodel = DataContext as DetailedRegisterMapView;
            if (viewmodel.SearchRegisterMethod(SearchTextBox.Text.Trim()))
            {
                Register_Map_DataGrid.ScrollIntoView(viewmodel.SelectedRegister);
                SearchTextBox_Border.BorderBrush = Brushes.LightGray;
            }
            else
            {
                SearchTextBox_Border.BorderBrush = Brushes.IndianRed;
            }


        }

        private void Register_Map_DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Address")
            {
                ListCollectionView lview = (ListCollectionView)CollectionViewSource.GetDefaultView(Register_Map_DataGrid.ItemsSource);
                if (e.Column.SortDirection != ListSortDirection.Ascending) // Null or descending now
                {
                    lview.CustomSort = new RegisterAddressCompareAsc();
                    e.Column.SortDirection = ListSortDirection.Ascending;
                }
                else // Ascending now
                {
                    lview.CustomSort = new RegisterAddressCompareDesc();
                    e.Column.SortDirection = ListSortDirection.Descending;
                }
            }
        }

        private void Maximize_Button_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                view.Maximize_Restore_Icon = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/max@3x.png"));
            }
            else
            {
                //UpdateWindowMaxHeight(); //Adjust the Max height of the Window in case user change to a diffrent display.
                var handle = new WindowInteropHelper(this).Handle;
                var screen = System.Windows.Forms.Screen.FromHandle(handle);

                var PSH = SystemParameters.PrimaryScreenHeight;
                var PSBH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                var ratio = PSH / PSBH;

                MaxHeight = (screen.WorkingArea.Height + 10) * ratio;
                WindowState = WindowState.Maximized;

                view.Maximize_Restore_Icon = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/info-modal-inactive@2x.png"));
            }
        }
    }
}
