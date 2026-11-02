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
using System.Windows.Threading;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    /// <summary>
    /// Interaction logic for WinDone.xaml
    /// </summary>
    public partial class WinDone : Window
    {
        private uint closeDelay;
        public WinDone(object content = null, uint preCloseDelayinMilliseconds = 1000, double windowHeight = 90, double windowWidth = 130)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(StartTimeTick);
            if (content != null)
            {
                xlabel.Content = content;
            }
            closeDelay = preCloseDelayinMilliseconds;
            Height = windowHeight;
            Width = windowWidth;
        }

        private void StartTimeTick(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(closeDelay);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            this.Close();
        }

    }
}
