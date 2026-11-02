using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace VirtualBenchPro_v4
{
    /// <summary>
    /// Interaction logic for Module_Button.xaml
    /// </summary>
    public partial class Module_Button : UserControl
    {
        StackPanel ParentContainer;
        private readonly string CLOSE_ICON_HOVER_YES = "#FFFF0000";
        private readonly string CLOSE_ICON_HOVER_NO = "#FFB3B3B3";
        private readonly string CLOSE_ICON_HOVER_NO_ACTIVE = "#FF3F3F3F";

        private byte _led_status;
        public byte LED_Status
        {
            get { return _led_status; }
            set
            {
                _led_status = value;
                Status_LED_Icon.STATUS = value;
            }
        }

        private bool _ismoduleselected;
        public bool IsModuleSelected 
        {
            get { return _ismoduleselected; }
            set
            {
                _ismoduleselected = value;
                if (value)
                {
                    Button_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC9D5E4"));
                    Module_Name.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                }
                else
                {
                    Button_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                    Module_Name.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9EACBF"));
                }
            }
        }

        public Module_Button(string text, bool moduleSelected, byte status)
        {
            InitializeComponent();
            Module_Name.Text = text;
            IsModuleSelected = moduleSelected;
            Status_LED_Icon.STATUS = status;
            Close_Icon.Visibility = System.Windows.Visibility.Collapsed;
            Module_Name.FontSize = 11;
            Height = 20;
            MinWidth = 52;
            Margin = new System.Windows.Thickness(2,0,2,0);

            
        }
        public Module_Button(StackPanel parent, string text, bool moduleSelected, byte status)
        {
            InitializeComponent();
            Module_Name.Text = text;
            ParentContainer = parent;
            IsModuleSelected = moduleSelected;
            Status_LED_Icon.STATUS = status;

            if (IsModuleSelected)
            {
                Button_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC9D5E4"));
                Module_Name.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                Close_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(CLOSE_ICON_HOVER_NO_ACTIVE));
            }
        }

        private void Label_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ParentContainer.Children.Remove(this);
        }

        private void Close_Icon_MouseEnter(object sender, MouseEventArgs e)
        {
            Close_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(CLOSE_ICON_HOVER_YES));
        }

        private void Close_Icon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsModuleSelected)
                Close_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(CLOSE_ICON_HOVER_NO_ACTIVE));
            else
                Close_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(CLOSE_ICON_HOVER_NO));

        }

        
    }
}
