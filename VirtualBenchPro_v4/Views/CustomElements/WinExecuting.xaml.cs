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

namespace VirtualBenchPro_v4.Views.CustomElements
{
    /// <summary>
    /// Interaction logic for WinExecuting.xaml
    /// </summary>
    public partial class WinExecuting : Window
    {
        public WinExecuting(string param)
        {
            InitializeComponent();
            switch (param)
            {
                case "WriteROM":
                    xlabel.Content = "Writing Data...";
                    break;
                case "ReadROM":
                    xlabel.Content = "Restoring Data...";
                    break;
                case "WriteRAM":
                    xlabel.Content = "Saving Data...";
                    break;
                case "ReadRAM":
                    xlabel.Content = "Reading Data...";
                    break;
                case "ClearFault":
                    xlabel.Content = "Clearing Fault...";
                    break;
                case "Validate":
                    xlabel.Content = "Validating...";
                    break;
                case "Initial read":
                    xlabel.Content = "Initial Reading...";
                    break;
                case "Kit Detect":
                    xlabel.Content = "Kit Detecting...";
                    break;
                case "Kit Update":
                    xlabel.Content = "Kit Updating...";
                    break;
                case "Enable":
                    xlabel.Content = "Enabling...";
                    break;
                case "Disable":
                    xlabel.Content = "Disabling...";
                    break;
                case "SwitchPage":
                    xlabel.Content = "Switching Page...";
                    break;
                default:
                    xlabel.Content = "Executing...";
                    break;
            }
        }
    }
}
