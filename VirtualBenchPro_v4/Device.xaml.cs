using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using MPS.Common.Model;
using MPS.Common.Sqlite;
using MPS.Monitor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Xml;
using System.Xml.Linq;
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.ViewModel;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;
//using VirtualBenchPro_v4.Views.MonitorViews;

namespace VirtualBenchPro_v4.Views
{
    public partial class Device : System.Windows.Controls.Page, INotifyPropertyChanged
    {
        #region Message notification 
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        XDocument xmldoc;
        private ProjectConfig _projectConfig;
        public ProjectConfig ProConfig
        {
            get { return _projectConfig; }
            set
            {
                _projectConfig = value;
                RaisePropertyChanged("ProConfig");
            }
        }
        public Device(ProjectConfig pobj, MainWindow mw)
        {
            InitializeComponent();
            if (DataContext == null)
                DataContext = new DeviceView(pobj, mw);
            ProConfig = pobj;
            //Data_Table1.Items.Clear();

            var mapper = Mappers.Xy<DataModel>()
               .X(model => model.Second)     //use Second as X
               .Y(model => model.Value);     //use Value as Y

            //save the mapper globally
            Charting.For<DataModel>(mapper);
        }


        //private void LinesItemClick(object sender, EventArgs e)
        //{
        //    var sd = (ListBoxItem)sender;
        //    BrushConverter brushConverter = new BrushConverter();
        //    var brush1 = (Brush)brushConverter.ConvertFromString("#FF2B4CFE");
        //    sd.Background = sd.Background == Brushes.White ? brush1 : Brushes.White;
        //    sd.Foreground = sd.Foreground == Brushes.White ? brush1 : Brushes.White;
        //    var tempview = DataContext as DeviceView;
        //    tempview.LineCheckedMethod(((MeasureLine)sd.Content).Title);

        //}

        private void Scroll_Left_Button_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = GetScrollViewer(Module_list) as ScrollViewer;
            sv.LineLeft();
        }

        private void Scroll_Right_Button_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = GetScrollViewer(Module_list) as ScrollViewer;
            sv.LineRight();


        }

        //private void Label_TargetUpdated(object sender, DataTransferEventArgs e)
        //{
        //    if ((sender as Label).Content != null)
        //        Export_Button.Visibility = Visibility.Visible;
        //    else
        //        Export_Button.Visibility = Visibility.Collapsed;

        //    //if (((System.Windows.Controls.ContentPresenter)((sender as Label).Content)).Content != null)
        //    //    Export_Button.Visibility = Visibility.Visible;
        //    //else
        //    //    Export_Button.Visibility = Visibility.Collapsed;
        //}

        private void Channel_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            foreach (var a in lb.SelectedItems)
            {
                if (a != lb.SelectedItem)
                    (a as CheckBox).IsChecked = false;
            }
        }

        //private void Register_Map_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (xmldoc == null)
        //    LoadXML();

        //    DataGrid test = (DataGrid)sender;
        //    XNamespace ns = "http://www.monolithicpower.com/XMLSchema";
        //    List<string> bit_controls = new List<string>();
        //    MPS.Common.Model.Register selected_register = test.SelectedItem as MPS.Common.Model.Register;
        //    if (selected_register != null)
        //    {
        //        Console.WriteLine(selected_register.Name);
        //        XElement register = xmldoc.Descendants(ns + "Register").Where(o => o.Attribute("Name").Value == selected_register.Name).FirstOrDefault();
        //        List<XElement> te = register.Descendants(ns + "BitControl").ToList();
        //        foreach (XElement element in te)
        //        {
        //            XElement register2 = xmldoc.Descendants(ns + "BitControl-Ref").Where(o => o.Attribute("Ref").Value == element.Attribute("Id").Value).FirstOrDefault();
        //            bit_controls.Add(register2.Parent.Attribute("Name").Value);
        //        }


        //        Register_Map_Tips.ItemsSource = bit_controls;

        //    }

        //}

        private void Register_Map_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid DG = (DataGrid)sender;
            List<string> bit_controls = new List<string>();

            //界面使用的虚拟寄存器是由两个寄存器组合来的，点击任意一个寄存器都指向虚拟寄存器的位置（spec定义的时候，单个寄存器的控件需要bc.Disabled）
            List<string> Expn = new List<string> { "MPQ7222", "MPQ76400FS", "MPQ76402FS", "MPQ7213", "MPQ7213C", "MPQ7212", "MPQ7212C", "MP2745" };

            MPS.Common.Model.Register selected_register = DG.SelectedItem as MPS.Common.Model.Register;
            if (selected_register != null)
            {               
                var pn = (DG.DataContext as DeviceView).CurrentChip.ChipName;
                foreach (var bc in selected_register.BitControls)
                {
                    if (string.IsNullOrEmpty(bc.ParamName))
                        continue;
                    if (bc.Disabled && !Expn.Contains(pn))
                        continue;

                    var path = FindBitPathMethod(selected_register, bc.BitID);
                    if (path.Split(new char[] { '/' }).Length < 2)
                        continue;

                    //只显示当前选择页面的bit信息
                    if (!path.Contains(_projectConfig.Chips.FirstOrDefault(p => p.IsSelected == true).Regions.FirstOrDefault(p => p.IsSelected == true).RegionName))
                    {
                        continue;
                    }

                    string BitInfo = "";
                    if (bc.BitLength > 1)
                    {
                        BitInfo = bc.Disabled ? string.Format("{0}({1}) {2}", bc.ParamName, bc.Unit, path) : string.Format("{0} D[{1}:{2}]  {3}", bc.ParamName, bc.Position + bc.BitLength - 1, bc.Position, path);
                    }
                    else
                    {
                        BitInfo = bc.Disabled ? string.Format("{0}({1}) {2}", bc.ParamName, bc.Unit, path) : string.Format("{0} D[{1}]  {2}", bc.ParamName, bc.Position, path);
                    }
                    bit_controls.Add(BitInfo);
                }
            }
            Register_Map_Tips.ItemsSource = bit_controls;

        }

        //private bool LoadXML()
        //{
        //    try
        //    {
        //        string working_directory = AppDomain.CurrentDomain.BaseDirectory;
        //        var specPath = working_directory + "/DataBase/" + (DataContext as DeviceView).CurrentChip.ChipName + "/DefaultDatasheet.Spec";
        //        xmldoc = XDocument.Load(specPath);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return false;
        //    }

        //}
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;


            if (cb.IsChecked == true)
            {
                foreach (var checkbox in FindVisualChildren<CheckBox>(Channel_lb))
                {
                    if (checkbox != cb)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }

        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    //if (child != null && (T)child != null)
                    if(child != null && child.GetType() == typeof(T))
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel lb = sender as StackPanel;
            var tempview = DataContext as DeviceView;
            //tempview.LineCheckedMethod((lb.Children[0] as TextBlock).Text);


            lb.Children[0].Opacity = lb.Children[0].Opacity == 1 ? 0.2 : 1;
            lb.Children[1].Opacity = lb.Children[1].Opacity == 1 ? 0.2 : 1;

        }

        public static DependencyObject GetScrollViewer(DependencyObject o)
        {
            // Return the DependencyObject if it is a ScrollViewer
            //if (o is ScrollViewer)
            var sv = o as ScrollViewer;
            if (sv != null)
            { return o; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }

        //private void Line_Checked(object sender, RoutedEventArgs e)
        //{
        //    TextBlock tb = FindVisualChildren<TextBlock>(sender as CheckBox).First();
        //    var tempview = DataContext as DeviceView;
        //    tempview.LineCheckedMethod(tb.Text);
        //}

        //private void Parameters_Channel_Scroll_Left_Click(object sender, RoutedEventArgs e)
        //{
        //    ScrollViewer sv = GetScrollViewer(lsbChannels) as ScrollViewer;
        //    if (sv == null)
        //    {
        //        return;
        //    }
        //    sv.LineLeft();
        //}

        //private void Parameters_Channel_Scroll_Right_Click(object sender, RoutedEventArgs e)
        //{
        //    ScrollViewer sv = GetScrollViewer(lsbChannels) as ScrollViewer;
        //    if (sv == null)
        //    {
        //        return;
        //    }
        //    sv.LineRight();
        //}

        private void Waveforms_Channel_Scroll_Left_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = GetScrollViewer(Channel_lb) as ScrollViewer;
            sv.LineLeft();
        }

        private void Waveforms_Channel_Scroll_Right_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = GetScrollViewer(Channel_lb) as ScrollViewer;
            sv.LineRight();
        }

        //private void Data_Table_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    FaultGrid.Visibility = Visibility.Visible;

        //    if (Data_Table.Columns.Count > 0)
        //    {
        //        Data_Table.Visibility = Visibility.Visible;
        //        Data_Table.Columns[0].CellStyle = FindResource("FirstColumn") as Style;
        //        Data_Table.Columns[0].HeaderStyle = FindResource("DGCHeaderStyle") as Style;
        //    }
        //    else
        //    {
        //        Data_Table.Visibility = Visibility.Collapsed;
        //    }

        //}


        //private void Data_Table_Unloaded(object sender, DataGridRowEventArgs e)
        //{

        //    Data_Table.Visibility = Visibility.Hidden;

        //}


        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image_Viewer iv = new Image_Viewer(sender as Image);
            iv.ShowDialog();
        }

        //private void Data_Table1_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    FaultGrid.Visibility = Visibility.Visible;

        //    if (Data_Table1.Columns.Count > 0)
        //    {
        //        Data_Table1.Columns[0].CellStyle = FindResource("FirstColumn") as Style;
        //        Data_Table1.Columns[0].HeaderStyle = FindResource("DGCHeaderStyle") as Style;
        //        Data_Table1.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        Data_Table1.Visibility = Visibility.Collapsed;
        //    }
        //}

        //private void Data_Table1_UnloadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    Data_Table1.Visibility = Visibility.Hidden;
        //}

        private void StatusGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (StatusGrid.Columns.Count > 0)
            {
                StatusGrid.Visibility = Visibility.Visible;
                DockPanel.SetDock(FaultGrid, Dock.Left);
            }
            else
            {
                StatusGrid.Visibility = Visibility.Collapsed;
                DockPanel.SetDock(FaultGrid, Dock.Top);
            }
        }

        private void StatusGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (StatusGrid.Items.Count == 0)
            {
                StatusGrid.Visibility = Visibility.Collapsed;
                DockPanel.SetDock(FaultGrid, Dock.Top);
            }
        }


        private void BlackBoxTabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BlackBox_DataGrid.Focus();
        }

        private void filtertb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BlackBoxTabItem.Focus();
            }
        }

        private void ParamTabItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If Selection Changed rasied by descendant Controls, do not execute PageSelectCommand function
            var orignalSourceName = ((FrameworkElement)e.OriginalSource).Name;
            var sourceName = ((FrameworkElement)e.Source).Name;
            if (!orignalSourceName.Equals(sourceName))
            {
                e.Handled = true;
            }
        }
        private string Pattern = @"^-?(\d+)?(\.)?(\d+)?$";

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
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
                    else if (textBox.Text.Length > 6)
                    {
                        // 长度超过10时移除内容，设置光标位置
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }

                }
            }
        }
        /// <summary>
        /// To find the location of the current bit
        /// </summary>
        /// <param name="address">Register address corresponding to current bit</param>
        /// <param name="id">ID of the current bit</param>
        /// <returns></returns>
        private string FindBitPathMethod(Register selectedreg, string id)
        {
            string path = "";
            var CurrentChip = ProConfig.Chips.FirstOrDefault(pt => pt.IsSelected);
            foreach (var title in CurrentChip.Regions)
            {
                var VM = (DeviceView)this.DataContext;
                if (title.RegionName == "Overview" || (title.RegionName == "Debug" && !VM.UserLicense.IsMPSAE))
                {
                    continue;
                }
                if (title.ConfigurationModes == null)
                    continue;
                if (title.Parameters != null)
                {
                    foreach (var bit in title.Parameters)
                    {
                        if (id == bit.BitID)
                        {
                            path = title.RegionName + "/" + bit.BitName;
                            return path;
                        }
                    }
                }
                foreach (var ConfigMode in title.ConfigurationModes)
                {
                    foreach (var page in ConfigMode.Pages)
                    {
                        int configCount = 0;
                        if (page.AllRegisters.FirstOrDefault(pt => pt.Address == selectedreg.Address && pt.Address_H == selectedreg.Address_H) != null)
                        {
                            foreach (var config in page.Configurations)
                            {
                                if (configCount > 1)      //Every Configurations only have two config(Basic and Advance)
                                    continue;
                                configCount++;

                                //var pathList = from cat in config.Categories
                                //               from sub in cat.Subcategories
                                //               from gp in sub.ParameterGroups
                                //               from bit in gp.BitControlMembers
                                //               where id == bit.BitID
                                //               select title.RegionName + (page.PageName == "" ? "" : title.RegionName == "" ? page.PageName : "/" + page.PageName) +
                                //                                (config.Name == "" ? "" : "/" + config.Name) +
                                //                                (cat.Name == "" ? "" : "/" + cat.Name) +
                                //                                (sub.Name == "" ? "" : "/" + sub.Name) +
                                //                                (gp.Name == "" ? "" : "/" + gp.Name);
                                //if (pathList.FirstOrDefault() != null)
                                //{
                                //    return pathList.FirstOrDefault();
                                //}

                                foreach (var cat in config.Categories)
                                {
                                    foreach (var sub in cat.Subcategories)
                                    {
                                        foreach (var group in sub.ParameterGroups)
                                        {
                                            foreach (var bit in group.BitControlMembers)
                                            {
                                                if (id == bit.BitID)
                                                {
                                                    path = title.RegionName +
                                                           (page.PageName == "" ? "" : title.RegionName == "" ? page.PageName : "/" + page.PageName) +
                                                           (config.Name == "" ? "" : "/" + config.Name) +
                                                           (cat.Name == "" ? "" : "/" + cat.Name) +
                                                           (sub.Name == "" ? "" : "/" + sub.Name) +
                                                           (group.Name == "" ? "" : "/" + group.Name);
                                                    return path;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }

        /*  private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)  // .cs file  need to add  PreviewMouseWheel="UIElement_OnPreviewMouseWheel"  ScrollViewer.CanContentScroll="True"
          {
              if (!e.Handled)                                                                 // can use the scroll wheel in the datagrid interface.= Helper:ScrollViewerHelper.IsPreviewMouseWheelEventMovetoNext="True"
              {
                  e.Handled = true;
                  var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                  eventArg.RoutedEvent = MouseWheelEvent;
                  eventArg.Source = sender;
                  var parent = ((Control)sender).Parent as UIElement;
                  parent?.RaiseEvent(eventArg);
              }
          }
        */

        private void Register_Map_DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Address")
            {
                ListCollectionView lview = (ListCollectionView)CollectionViewSource.GetDefaultView(Register_Map_DataGrid.ItemsSource);
                if (e.Column.SortDirection != ListSortDirection.Ascending) // Null or descending now
                {
                    lview.CustomSort = new RegisterAddressCompareAsc();
                    e.Column.SortDirection = ListSortDirection.Ascending;
                }
                else // Ascending now
                {
                    lview.CustomSort = new RegisterAddressCompareDesc();
                    e.Column.SortDirection = ListSortDirection.Descending;
                }
            }
        }

        public class RegisterAddressCompareAsc : IComparer
        {
            public int Compare(object x, object y)
            {
                var register1 = x as Register;
                var register2 = y as Register;
                var address1 = (register1.Address_H << 8) + register1.Address;
                var address2 = (register2.Address_H << 8) + register2.Address;

                return address1.CompareTo(address2);
            }
        }

        public class RegisterAddressCompareDesc : IComparer
        {
            public int Compare(object x, object y)
            {
                var register1 = x as Register;
                var register2 = y as Register;
                var address1 = (register1.Address_H << 8) + register1.Address;
                var address2 = (register2.Address_H << 8) + register2.Address;

                return address2.CompareTo(address1);
            }
        }
    }
}
