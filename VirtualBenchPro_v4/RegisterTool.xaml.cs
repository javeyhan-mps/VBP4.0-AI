using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using VirtualBenchPro_v4.ViewModel;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;
//using VirtualBenchPro_v4.Views.MonitorViews;

namespace VirtualBenchPro_v4.Views
{
    public partial class RegisterTool : Page
    {
        public RegisterTool(MainWindow mw)
        {
            InitializeComponent();
            this.DataContext = new RegisterToolView(mw);
        }

        private const string Pattern = @"^\d{0,3}$";
        private void FreqTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var change = new TextChange[e.Changes.Count];
                e.Changes.CopyTo(change, 0);
                int offset = change[0].Offset;
                if (change[0].AddedLength > 0)
                {
                    if (!Regex.IsMatch(textBox.Text, Pattern, RegexOptions.IgnoreCase))
                    {
                        // 不符合正则表达式时移除内容，设置光标位置
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }
                }
            }
        }

        private void FreqTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (UnitCmb.SelectedIndex == 0)
                {
                    if (int.Parse(textBox.Text) < 94)
                    {
                        textBox.Text = "94";
                    }
                }
                else
                {
                    if (int.Parse(textBox.Text) > 24)
                    {
                        textBox.Text = "24";
                    }
                    else if (int.Parse(textBox.Text) < 1)
                    {
                        textBox.Text = "1";
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var textBox = FreqTxt;
            if (textBox != null)
            {
                if (UnitCmb.SelectedIndex == 0)
                {
                    if (int.Parse(textBox.Text) < 94)
                    {
                        textBox.Text = "94";
                    }
                }
                else
                {
                    if (int.Parse(textBox.Text) > 24)
                    {
                        textBox.Text = "24";
                    }
                    else if (int.Parse(textBox.Text) < 1)
                    {
                        textBox.Text = "1";
                    }
                }
            }
        }
    }
}