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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    public partial class FloatingWindow : Window
    {
        private object _floatingWindowContent;
        public object FloatingWindowContent
        {
            get 
            { 
                return _floatingWindowContent; 
            }
            set
            {
                contentControl.Content = value;
                _floatingWindowContent = value;
            }
        }

        private string _windowTitle = "Floating Window";
        public string WindowTitle
        {
            get
            {
                return _windowTitle;
            }
            set
            {
                title_Label.Content = value;
                Title = value;
                _windowTitle = value;
            }
        }

        private const double maxContentWidth = 1100;
        private double _contentWidth;
        public double ContentWidth
        {
            get
            {
                return _contentWidth;
            }
            set
            {
                var caculatedWidth = value > maxContentWidth ? maxContentWidth : value;

                Width = caculatedWidth + 10 + 22; // ContentWidth + Margin + Space to hide scrollbar
                _contentWidth = caculatedWidth;
            }
        }


        private const double maxContentHeight = 520;
        private double _contentHeight;
        public double ContentHeight
        {
            get
            {
                return _contentHeight;
            }
            set
            {
                var caculateHeight = value > maxContentHeight ? maxContentHeight : value;

                Height = caculateHeight + 40 + 10 + 4; // ContentHeight + title Height + Margin + Space to hide scrollbar
                _contentHeight = caculateHeight;
            }
        }

        public FloatingWindow()
        {
            InitializeComponent();
            Maximize_Button.Content = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/max@3x.png"));
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                AdjustWindowSize();
            }
            else
            {
                DragMove();
            }
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Button_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AdjustWindowSize()
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
            {
                UpdateWindowMaxHeight(); //Adjust the Max height of the Window in case user change to a diffrent display.
                WindowState = WindowState.Maximized;
            }
        }

        private void UpdateWindowMaxHeight()
        {
            var handle = new WindowInteropHelper(this).Handle;
            var screen = System.Windows.Forms.Screen.FromHandle(handle);

            var PSH = SystemParameters.PrimaryScreenHeight;
            var PSBH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            var ratio = PSH / PSBH;

            MaxHeight = (screen.WorkingArea.Height + 10) * ratio;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                //BorderThickness = new Thickness(6, 6, 6, 0);
                Maximize_Button.Content = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/info-modal-inactive@2x.png"));
                WindowBorder.Margin = new Thickness(7, 7, 7, 0);
                WindowBorder.CornerRadius = new CornerRadius(0);
            }
            if (WindowState == WindowState.Normal)
            {
                //BorderThickness = new Thickness(0);
                Maximize_Button.Content = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/max@3x.png"));
                WindowBorder.Margin = new Thickness(5);
                WindowBorder.CornerRadius = new CornerRadius(5);
            }
        }
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            // to make sure the Window max height updated when eneter a diffrent moitor to make sure the Windows 10 drag
            // to change Applicaton window size function work normally
            UpdateWindowMaxHeight();
        }
    }
}
