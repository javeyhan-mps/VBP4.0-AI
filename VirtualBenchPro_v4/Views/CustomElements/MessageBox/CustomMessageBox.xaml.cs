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

namespace VirtualBenchPro_v4
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
    
        private string _type;
        public string Type {
            get { return _type; }
            set
            {
                _type = value;

                switch (value)
                {
                    case "CheckDongle":
                    {
                        MessageBoxContent.Content = new MB_CheckDongle(this);
                        CenterWindowOnScreen();
                        break;
                    }
                    case "SaveConfiguration":
                    {
                        MessageBoxContent.Content = new MB_SaveConfiguration(this);
                        CenterWindowOnScreen();
                        break;
                    }
                    case "UsbUnavailable":
                    {
                        MessageBoxContent.Content = new MB_UsbUnavailable(this);
                        CenterWindowOnScreen();
                        break;
                    }
                    case "SelectIC":
                    {
                        MessageBoxContent.Content = new MB_SelectIC(this);
                        CenterWindowOnScreen();
                        break;
                    }
                }
            }
        }

        public CustomMessageBox(string type)
        {
            InitializeComponent();
            Type = type;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = RenderSize.Width;
            double windowHeight = RenderSize.Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();   
            }
        }
    }
}
