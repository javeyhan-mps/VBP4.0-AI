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

namespace VirtualBenchPro_v4.Views.CustomElements
{
    /// <summary>
    /// Interaction logic for Subcategory_GroupBox.xaml
    /// </summary>
    public partial class Subcategory_GroupBox : UserControl, INotifyPropertyChanged
    {

        private string _subCategoryName;
        private int _top_height;
        public string SubCategoryName
        {
            get { return _subCategoryName; }
            set
            {
                _subCategoryName = value;
                RaisePropertyChanged("SubCategoryName");
            }
        }
        public int Top_Height
        {
            get { return _top_height; }
            set
            {
                _top_height = value;
                RaisePropertyChanged("Top_Height");
            }
        }

        public bool HasInfo { get; set; }

        public Subcategory_GroupBox(string name)
        {
            InitializeComponent();
            DataContext = this;
            Top_Height = 20;

            Help_Icon_Control.Visibility = Visibility.Hidden;
            SubCategoryName = name;

            if (name == "None")
                Top_Height = 0;
        }

        public Subcategory_GroupBox(string name, string info)
        {
            InitializeComponent();
            DataContext = this;
            Top_Height = 20;

            Help_Icon_Control.Children.Add(new Help_Icon(false, info));

            SubCategoryName = name;

            if (name == "None")
                Top_Height = 0;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
