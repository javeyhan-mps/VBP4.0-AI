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
    /// Interaction logic for Module_Card.xaml
    /// </summary>
    public partial class Module_Card : UserControl, INotifyPropertyChanged
    {

        private double _temperature;
        private string _module_name;
        private byte _status;
        private WrapPanel ParentContainer;

        #region Properties
        public string Module_Name
        {
            get { return _module_name; }
            set
            {
                _module_name = value;
                RaisePropertyChanged("Module_Name");
            }
        }
        public double Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                _temperature = value;
                RaisePropertyChanged("Temperature");
            }
        }
        public byte Status
        {
            get { return _status; }
            set
            {
                _status = value;
                Status_LED_Icon.STATUS = value;
            }
        }

        public Dictionary<string, byte> Input_Parameters_Headers { get; set; }
        public Dictionary<string, byte> Channel_Parameters_Headers { get; set; }

        public Dictionary<string, ModuleValue[]> Input_Parameters_Data { get; set; }
        public Dictionary<string, ModuleValue[]> Channel_Parameters_Data { get; set; }

        public List<ColumnDefinition> Input_Columns { get; set; }
        public List<RowDefinition> Input_Rows { get; set; }
        public List<ColumnDefinition> Channel_Columns { get; set; }
        public List<RowDefinition> Channel_Rows { get; set; }
        #endregion

        public Module_Card(string name, bool mode, Dictionary<string, byte> input_parameters_headers, Dictionary<string, byte> channel_parameters_headers, Dictionary<string, ModuleValue[]> input_parameters_data, Dictionary<string, ModuleValue[]> channel_parameters_data, double temp, WrapPanel parent)
        {
            try
            {

                InitializeComponent();
                DataContext = this;
                ParentContainer = parent;

                Module_Name = name;
                Input_Parameters_Headers = input_parameters_headers;
                Channel_Parameters_Headers = channel_parameters_headers;
                Input_Parameters_Data = input_parameters_data;
                Channel_Parameters_Data = channel_parameters_data;

                Input_Columns = new List<ColumnDefinition>();
                Input_Rows = new List<RowDefinition>();
                Channel_Columns = new List<ColumnDefinition>();
                Channel_Rows = new List<RowDefinition>();

                Temperature = temp;

                if (mode)
                {
                    Top_Right_Section.Visibility = Visibility.Hidden;
                }
                InitTable();

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void InitTable()
        {
            InitColRow();

            //Put the data in the Tables
            int i = 1;
            foreach (KeyValuePair<string, byte> s in Input_Parameters_Headers)
            {
                Module_Button l = new Module_Button(s.Key, false, s.Value);
                l.MouseLeftButtonUp += ToggleColumn;
                Grid.SetRow(l, 0);
                Grid.SetColumn(l, i);
                Input_Table.Children.Add(l);
                i++;
            }

            i = 1;
            foreach (KeyValuePair<string, ModuleValue[]> kp in Input_Parameters_Data)
            {
                TextBlock l = new TextBlock
                {
                    Text = kp.Key,
                    Padding = new Thickness(0),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180")),                  
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                
                Grid.SetRow(l, i);
                Grid.SetColumn(l, 0);
                Input_Table.Children.Add(l);

                int column_counter = 1;
                foreach(ModuleValue value in kp.Value)
                {
                    Label v = new Label
                    {
                        Content = value.Value + " " + value.Unit,
                        Padding = new Thickness(0),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180")),
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    Grid.SetRow(v, i);
                    Grid.SetColumn(v, 0 + column_counter);
                    Input_Table.Children.Add(v);
                    column_counter++;
                }
                i++;
            }

            //channel column headers
            i = 1;
            foreach (KeyValuePair<string, byte> s in Channel_Parameters_Headers)
            {
                Module_Button l = new Module_Button(s.Key, false, s.Value);
                l.MouseLeftButtonUp += ToggleColumn;
                Grid.SetRow(l, 0);
                Grid.SetColumn(l, i);
                Channel_Table.Children.Add(l);
                i++;
            }

            //channel row data
            i = 1;
            foreach (KeyValuePair<string, ModuleValue[]> kp in Channel_Parameters_Data)
            {
                TextBlock l = new TextBlock
                {
                    Text = kp.Key,
                    Padding = new Thickness(0),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180")),                  
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetRow(l, i);
                Grid.SetColumn(l, 0);
                Channel_Table.Children.Add(l);

                int column_counter = 1;
                foreach (ModuleValue value in kp.Value)
                {
                    Label v = new Label
                    {
                        Content = value.Value + " " + value.Unit,
                        Padding = new Thickness(0),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180")),
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    Grid.SetRow(v, i);
                    Grid.SetColumn(v, 0 + column_counter);
                    Channel_Table.Children.Add(v);
                    column_counter++;
                }
                i++;
            }

        }

        private void InitColRow()
        {
            //Generate Columns/Rows
            for (int n = 0; n <= Input_Parameters_Headers.Count; n++)
            {
                Input_Columns.Add(new ColumnDefinition());
                if(n != 0)
                    Input_Columns[n].MinWidth = 43;
            }
            for (int n = 0; n <= Input_Parameters_Data.Count; n++)
            {
                Input_Rows.Add(new RowDefinition());
                Input_Rows[n].Height = new GridLength(25);
            }
            for (int n = 0; n <= Channel_Parameters_Headers.Count; n++)
            {
                Channel_Columns.Add(new ColumnDefinition());
                if (n != 0)
                    Channel_Columns[n].MinWidth = 43;
            }
            for (int n = 0; n <= Channel_Parameters_Data.Count; n++)
            {
                Channel_Rows.Add(new RowDefinition());
                Channel_Rows[n].Height = new GridLength(25);
            }

            //Add Column/Rows to Table(Grid)
            foreach (ColumnDefinition c in Input_Columns)
                Input_Table.ColumnDefinitions.Add(c);

            foreach (RowDefinition r in Input_Rows)
                Input_Table.RowDefinitions.Add(r);

            foreach (ColumnDefinition c in Channel_Columns)
                Channel_Table.ColumnDefinitions.Add(c);

            foreach (RowDefinition r in Channel_Rows)
                Channel_Table.RowDefinitions.Add(r);
        }

        public void Update(byte module_status, Dictionary<string, byte> input_parameters_headers, Dictionary<string, byte> channel_parameters_headers, Dictionary<string, ModuleValue[]> input_parameters_data, Dictionary<string, ModuleValue[]> channel_parameters_data, double temp)
        {
            Input_Parameters_Headers = input_parameters_headers;
            Channel_Parameters_Headers = channel_parameters_headers;
            Input_Parameters_Data = input_parameters_data;
            Channel_Parameters_Data = channel_parameters_data;
            Status = module_status;
            Temperature = temp;


            int i = 1;
            foreach (KeyValuePair<string, byte> s in Input_Parameters_Headers)
            {
                Module_Button item = Input_Table.Children.Cast<Module_Button>().First(e => Grid.GetRow(e) == 0 && Grid.GetColumn(e) == i);
                item.LED_Status = s.Value;
                i++;
            }

            i = 1;
            foreach (KeyValuePair<string, byte> s in Channel_Parameters_Headers)
            {
                Module_Button item = Channel_Table.Children.Cast<Module_Button>().First(e => Grid.GetRow(e) == 0 && Grid.GetColumn(e) == i);
                item.LED_Status = s.Value;
                i++;
            }

            i = 1;
            foreach (KeyValuePair<string, ModuleValue[]> s in Input_Parameters_Data)
            {
                int column_counter = 1;
                foreach (ModuleValue value in s.Value)
                {
                    try
                    {
                        Label item = Input_Table.Children.OfType<Label>().First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == column_counter);
                        item.Content = value.Value + " " + value.Unit;
                        column_counter++;
                    }
                    catch (Exception p)
                    {
                        Console.WriteLine(p);
                        continue;
                    }
                }
                i++;
            }

            i = 1;
            foreach (KeyValuePair<string, ModuleValue[]> s in Channel_Parameters_Data)
            {
                int column_counter = 1;
                foreach (ModuleValue value in s.Value)
                {
                    try
                    {

                        Label item = Channel_Table.Children.OfType<Label>().First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == column_counter);
                        item.Content = value.Value + " " + value.Unit;
                        column_counter++;
                    }
                    catch(Exception o)
                    {
                        Console.WriteLine(o);
                        continue;
                    }
                }
                i++;
            }
        }

        public void ToggleColumn(object sender, MouseButtonEventArgs e)
        {
            Module_Button m = (Module_Button)sender;
            m.IsModuleSelected = !m.IsModuleSelected;
            Console.WriteLine(Grid.GetColumn(m));
            Grid g = (Grid)m.Parent;
            if (g.Name == "Input_Table")
            {
                int i = 1;
                foreach (KeyValuePair<string, ModuleValue[]> s in Input_Parameters_Data)
                {
                   
                    Label item = g.Children.OfType<Label>().First(p => Grid.GetRow(p) == i && Grid.GetColumn(p) == Grid.GetColumn(m));
                    if (m.IsModuleSelected)
                    {
                        item.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF95AAB9"));
                    }
                    else
                    {
                        item.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180"));
                    }

                    i++;
                    
                }
            }
            else
            {
                int i = 1;
                foreach (KeyValuePair<string, ModuleValue[]> s in Channel_Parameters_Data)
                {
                   
                    Label item = g.Children.OfType<Label>().First(p => Grid.GetRow(p) == i && Grid.GetColumn(p) == Grid.GetColumn(m));
                    if (m.IsModuleSelected)
                    {
                        item.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF95AAB9"));
                    }
                    else
                    {
                        item.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180"));
                    }

                    i++;
                    
                }
            }
        }

        private void Close_Icon_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ParentContainer.Children.Remove(this);
        }
     
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
