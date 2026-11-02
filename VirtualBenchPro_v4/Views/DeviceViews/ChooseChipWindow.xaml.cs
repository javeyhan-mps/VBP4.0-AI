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
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.ViewModel;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for ChooseChipWindow.xaml
    /// </summary>
    public partial class ChooseChipWindow : Window
    {
        public ChooseChipWindow(LicenseInfo license)
        {
            InitializeComponent();
            this.DataContext = new ChooseChipWindowViewModel(license);
        }

        private void Login_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var obj = e.Parameter as ChooseChipWindowViewModel;

            var isNextButtonEnable = obj?.SelectedChip != null;
            e.CanExecute = isNextButtonEnable;
            Next_Button.Opacity = isNextButtonEnable ? 1 : 0.5;
        }


        private void Login_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            //this.Visibility = Visibility.Hidden;
            //MatchAddressWindow mawin = new DeviceViews.MatchAddressWindow();

            //if ((bool)mawin.ShowDialog())
            //{
            //    var chs = this.DataContext as ChooseChipWindowViewModel;


            //}

            DialogResult = true;
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

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (e.Key == Key.Enter)
            {
                tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                var dc = DataContext as ChooseChipWindowViewModel;
                if (dc != null)
                {
                    dc.FilterChipMethod();
                }
            }
        }

        //private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((sender as ListBox).SelectedItem != null)
        //    {
        //        Next_Button.Opacity = 1;
        //        Next_Button.IsEnabled = true;
        //    }
        //}
    }
}
