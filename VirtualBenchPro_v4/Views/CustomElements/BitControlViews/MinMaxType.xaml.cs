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
    public partial class MinMaxType : UserControl, INotifyPropertyChanged
    {
        Regex _regex = new Regex(@"^(\-?)([0-9]+(\.?)([0-9]+)?)$");

        private double _value;
        private string _unit;

        public string ID { get; set; }
        public string BitName { get; set; }
        public int Position { get; set; }
        public int BitLength { get; set; }
        public double Value
        {
            get { return _value; }
            set
            {
             
                _value = value;
                
            }
        }
        public string Unit
        {
            get { return "[" + _unit + "]"; }
            set
            {
                _unit = value;
            }
        }
        public double Lsb { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        //Type MinMaxType
        public MinMaxType(string id, string name, int position, int bitLength, string unit, double lsb, double minValue, double maxValue)
        {
            InitializeComponent();
            DataContext = this;

            ID = id;
            BitName = name;
            Position = position;
            BitLength = bitLength;
            Unit = unit;
            Lsb = lsb;
            MinValue = minValue;
            MaxValue = maxValue;
            Help_Icon_Control.Visibility = Visibility.Hidden;
        }

        public MinMaxType(string id, string name, int position, int bitLength, string unit, double lsb, double minValue, double maxValue, string info)
        {
            InitializeComponent();
            DataContext = this;

            ID = id;
            BitName = name;
            Position = position;
            BitLength = bitLength;
            Unit = unit;
            Lsb = lsb;
            MinValue = minValue;
            MaxValue = maxValue;

            Help_Icon_Control.Children.Add(new Help_Icon(true, info));
        }

        //Only allows numbers
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Console.WriteLine(box.Text.Count(x => x == '.'));

            if (e.Text.EndsWith("."))
            {
                if (box.Text.Count(x => x == '.') < 1)
                {
                    e.Handled = true;
                    box.Text += ".";
                }
            }
            else
            {
                e.Handled = IsTextAllowed(e.Text);
            }

        }
        private bool IsTextAllowed(string text) => !_regex.IsMatch(text);

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
