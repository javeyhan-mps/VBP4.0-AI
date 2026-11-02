using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MB_SelectIC : Page, INotifyPropertyChanged
    {
        Window ParentWindow;

        private List<string> _modules;
        public List<string> Modules 
        {
            get { return _modules; }
            set
            {
                _modules = value;
                RaisePropertyChanged("Modules");
            }
        }

        public MB_SelectIC(Window parent)
        {
            InitializeComponent();
            DataContext = this;
            ParentWindow = parent;

            Modules = new List<string>{ "test", "test2", "test3", "test4", "test2", "test2", "test2", "test2", "test2", "test2", "test2", "test2" };
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Close();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            //Do something
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
