//using MPS.Common.AutoHelper;
using MPS.AutoUpdate.Interact;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for WinUpdate.xaml
    /// </summary>
    public partial class WinUpdate : Window
    {
        public WinUpdate(string ver)
        {
            InitializeComponent();
            content.Text = string.Format("A new version{0} is available.  Would you like to install it now?", ver);
            switch (ConstantModel.settings.UpdateServerLocation)
            {
                case "US":
                    Radio_US.IsChecked = true;
                    break;
                case "CN":
                    Radio_CN.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void btnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Radio_US.IsChecked != true && Radio_CN.IsChecked != true)
                {
                    UpdateErrorStatus("Please choose update sever");
                    return;
                }

                ConstantModel.settings.UpdateServerLocation = Radio_US.IsChecked == true ? "US" : "CN";
                ConstantModel.settings.SaveSettings();
                var configXmlPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "config.xml");
                if (File.Exists(configXmlPath))
                {
                    var configXmlContent = File.ReadAllText(configXmlPath);
                    var regex = new Regex("ServerUrl=\"([^\"]+)\"");  // Find {ServerUrl="......"}
                    var regexMatch = regex.Match(configXmlContent);
                    if (regexMatch.Success)
                    {
                        var matchContent = regexMatch.Groups[0].Value; // {ServerUrl="......"}
                        var updateServerLocationDomain = Settings.GetUpdateServerLocationDomain(ConstantModel.settings.UpdateServerLocation);
                        if (!matchContent.Contains($".{updateServerLocationDomain}/")) // If update server in file not current selected domain
                        {
                            var regex1 = new Regex(@"\.([^./]+)/");  // Find {.{Domain}/} in file
                            var regex1Match = regex1.Match(matchContent);
                            if (regex1Match.Success)
                            {
                                var macthContent1 = regex1Match.Groups[0].Value;  // {.{Domain}/} in file
                                var replacedMatchContent = matchContent.Replace(macthContent1, $".{updateServerLocationDomain}/"); // Replaced {ServerUrl="......"}

                                var newConfigXmlContent = configXmlContent.Replace(matchContent, replacedMatchContent);
                                File.WriteAllText(configXmlPath, newConfigXmlContent, Encoding.UTF8);
                            }
                        }
                    }
                }

                CheckUpdateStatus.DoUpdateProcess(ConstantModel.UPDATEAPPNAME, ConstantModel.APPNAME);
            }
            catch
            {
                //MessageBox.Show("Can't connect to MPS website");
                UpdateErrorStatus("Can't connect to MPS website");
            }
        }

        private void UpdateErrorStatus(string status)
        {
            TextBlock_Status.Foreground = Brushes.Red;
            TextBlock_Status.Text = status;
        }
    }
}
