using MPS.Common.Sqlite;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VirtualBenchPro_v4.Views.CustomElements;

namespace VirtualBenchPro_v4.Views
{
    /// <summary>
    /// Interaction logic for Help_Window.xaml
    /// </summary>
    public partial class Help_Window : Window
    {
        private Page index_view;
        private List<HelpGroup> Help_Content { get; set; }
        public bool isShow = false;
        private Point point = new Point(0,0);

        public Help_Window(string title, List<HelpGroup> help_content)
        {
            InitializeComponent();
            DataContext = this;
            Help_Title_Label.Content = title;
            Help_Content = help_content;
            index_view = new Page
            {
                Content = InitContent(help_content)
            };

            Help_View.Content = index_view;
            this.WindowState = WindowState.Normal;
            LocationMiddle();
        }

        private WrapPanel InitContent(List<HelpGroup> content)
        {
            WrapPanel wp = new WrapPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 24, 0, 0)
            };
            if (content != null)
            {
                if(content.Count == 1 && content[0].HelpInfoList.Count == 1)
                {
                    Help_GroupBox hgb = new Help_GroupBox(content[0], Help_View, this);
                    wp = hgb.GetImageView(wp);  
                }
                else
                {
                    foreach (HelpGroup hp in content)
                    {
                        wp.Children.Add(new Help_GroupBox(hp, Help_View, this));
                    }
                }
            }
            return wp;
        }

        public void GoHome()
        {
            index_view = new Page
            {
                Content = InitContent(Help_Content)
            };
            while (Help_View.CanGoBack)
                Help_View.RemoveBackEntry();
            
            Help_View.Content = index_view;


        }

        #region Events 
        private void Minimize_Button_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Close_Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
            if (e.ClickCount == 2)
            {
                this.Width = 405;
                LocationMiddle();
            }
        }
        #endregion
        
        private void Max_Button_Click(object sender, RoutedEventArgs e)
        {
            //if (Help_Content.Count == 1 && Help_Content[0].HelpInfoList.Count == 1)
            //{
            //    this.Width = this.Width == 405 ? this.Width * 3 : 405;

            //}
            //else if (isShow)
            //{
            //    this.Width = this.Width == 405 ? this.Width * 3 : 405;
            //}
            //LocationMiddle();

            if (this.WindowState == WindowState.Normal)
            {
                var handle = new WindowInteropHelper(this).Handle;
                var screen = System.Windows.Forms.Screen.FromHandle(handle);

                var PSH = SystemParameters.PrimaryScreenHeight;
                var PSBH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                var ratio = PSH / PSBH;
                MaxHeight = screen.WorkingArea.Height * ratio;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }

        }
        private void LocationMiddle()
        {
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHigh = SystemParameters.WorkArea.Height;
            this.Top = (screenHigh - this.Height) / 2;
            this.Left = (screenWidth - this.Width) / 2;
        }
    }

}
