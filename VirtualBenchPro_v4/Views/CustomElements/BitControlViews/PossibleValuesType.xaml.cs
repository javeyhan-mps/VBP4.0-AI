using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for BitControl_View.xaml
    /// </summary>
    public partial class PossibleValuesType : UserControl, INotifyPropertyChanged
    {
        Regex _regex = new Regex(@"^(\-?)([0-9]+(\.?)([0-9]+)?)$");
        private short _itemSelected;

        public string ID { get; set; }
        public string BitName { get; set; }
        public int Position { get; set; }
        public int BitLength { get; set; }
        public short ItemSelected
        {
            get { return _itemSelected; }
            set
            {
                _itemSelected = value;
                if(PossibleValues.Count > 0)
                {
                    Value = Convert.ToInt32(PossibleValues.ElementAt(value).Value);
                    PossibleValuesType_ComboBox.ToolTip = PossibleValues.ElementAt(value).Key;
                }
            }
        }
        public int Value { get; set; }
        public double Lsb { get; set; }
        public Dictionary<string, int> PossibleValues { get; set; }


        //Type PossibleValuesType
        public PossibleValuesType(string id, string name, int position, int bitLength, Dictionary<string, int> possibleValues)
        {
            InitializeComponent();
            DataContext = this;

            ID = id;
            BitName = name;
            Position = position;
            BitLength = bitLength;
            PossibleValues = possibleValues; 
            
            List<string> temp = new List<string>();
            foreach(KeyValuePair<string, int> bit in PossibleValues)
            {
                temp.Add(bit.Key);
            }

            PossibleValuesType_ComboBox.ItemsSource = temp;
            PossibleValuesType_ComboBox.ToolTip = temp[0];
            Help_Icon_Control.Visibility = Visibility.Hidden;

        }

        public PossibleValuesType(string id, string name, int position, int bitLength, Dictionary<string, int> possibleValues, string info)
        {
            InitializeComponent();
            DataContext = this;

            ID = id;
            BitName = name;
            Position = position;
            BitLength = bitLength;
            PossibleValues = possibleValues;

            List<string> temp = new List<string>();
            foreach (KeyValuePair<string, int> bit in PossibleValues)
            {
                temp.Add(bit.Key);
            }

            PossibleValuesType_ComboBox.ItemsSource = temp;
            PossibleValuesType_ComboBox.ToolTip = temp[0];

            Help_Icon_Control.Children.Add(new Help_Icon(true, info));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
