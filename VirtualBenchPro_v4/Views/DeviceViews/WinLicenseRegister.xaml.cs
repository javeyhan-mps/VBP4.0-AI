using System;
using System.Collections.Generic;
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
using VirtualBenchPro_v4.ViewModel;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for WinLicenseRegister.xaml
    /// </summary>
    public partial class WinLicenseRegister : Window
    {
        //Regex regex = new Regex("^[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}$");
        Regex regex = new Regex("^[A-Za-z0-9]{1,}$");

        public static string code;
        public static string key;
        public WinLicenseRegister(string code_, string key_)
        {
            InitializeComponent();
            content_tb.Text = code_;
            content1_tb.Text = key_;
            code = null;
            key = null;
        }

        //private void Login_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    var obj = e.Parameter as LicenseInfo;

        //    if (obj == null)
        //    {
        //        e.CanExecute = false;
        //        return;
        //    }
        //    bool pass = false;
        //    if (obj.Code != null && obj.Key != null)
        //    {
        //        pass = (regex.IsMatch(obj.Code)) && (regex.IsMatch(obj.Key));
        //    }

        //    this.Register_Button.Opacity = pass ? 1 : 0.5;
        //    this.Register_Button.IsEnabled = pass;
        //    e.CanExecute = pass;

        //}

        //private void Login_Execute(object sender, ExecutedRoutedEventArgs e)
        //{
        //    DialogResult = true;
        //}

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void content_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var tb = sender as TextBox;
            //var len = 0;
            //for (int i = 0; i < tb.Text.Length; i++)
            //{
            //    if (tb.Text[i] != '-')
            //        len++;   
            //}

            //if ( (len > 4) && ((len - 1) % 4) == 0)
            //{
            //    var ch = tb.Text[tb.Text.Length - 2];
            //    if (ch != '-')
            //    {
            //        tb.Text = tb.Text.Insert(tb.Text.Length - 1, "-");
            //        tb.SelectionStart = tb.Text.Length;
            //    }
            //}
            var code = content_tb.Text;
            var key = content1_tb.Text;
            bool pass = false;
            if (code != null && key != null)
            {
                pass = (regex.IsMatch(code)) && (regex.IsMatch(key));
            }

            this.Register_Button.Opacity = pass ? 1 : 0.5;
            this.Register_Button.IsEnabled = pass;
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            code = content_tb.Text;
            key = content1_tb.Text;
            DialogResult = true;
            this.Close();
        }

        private void request_tb_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://www.monolithicpower.com/en/contact/vbprolicenserequest.html");
            switch (ConstantModel.mpsWebServer)
            {
                case "Production":
                    System.Diagnostics.Process.Start(ConstantModel.WSproductionURL + "en/contact/vbprolicenserequest.html");
                    break;
                case "Staging":
                    System.Diagnostics.Process.Start(ConstantModel.WSStagingURL + "en/contact/vbprolicenserequest.html");
                    break;
                default:
                    throw new ArgumentException("Invalid mpsWebServer value:" + ConstantModel.mpsWebServer);
            }
        }
    }
}
