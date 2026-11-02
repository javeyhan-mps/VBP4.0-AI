using Microsoft.Win32;
using MPS.Common;
using MPS.Common.Model;
using MPS.Instrument;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.ViewModel;
using VirtualBenchPro_v4.Views;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;
using MPS.Monitor;
using System.Collections.ObjectModel;
using VirtualBench_PC_MTP;
using MPS.Common.Log;
using MPS.Instrument.Enum;
using System.Windows.Interop;
using MPS.AutoUpdate.Interact;
using MPS.Communication;
using MPS.VBP.StateMachineDiagram;

namespace VirtualBenchPro_v4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Device device_view;
        System_View system_view;
        RegisterTool registertool_view;
        public MPS.VBP.StateMachineDiagram.FlowChart.FlowchartEditor ucsm_view { set; get; }
       


       Evaluation_View evaluation_view;
        //public readonly object _lockObj = new object();
        private List<string> _publicParts;
        private int p_AnimationFrom = 96;
        private int p_AnimationTo = 0;
        private int h_AnimationFrom = 96;
        private int h_AnimationTo = 0;
        private ImageSource minimize_restore_icon;
        private bool showRegisterToolWarningWindow = true;

        public SystemView SystemViewVM { get; set; }

        public int ProjectButton_AnimationFrom
        {
            get { return p_AnimationFrom; }
            set
            {
                p_AnimationFrom = value;
                RaisePropertyChanged("ProjectButton_AnimationFrom");
            }
        }
        public int ProjectButton_AnimationTo
        {
            get { return p_AnimationTo; }
            set
            {
                p_AnimationTo = value;
                RaisePropertyChanged("ProjectButton_AnimationTo");
            }
        }
        public int HelpButton_AnimationFrom
        {
            get { return h_AnimationFrom; }
            set
            {
                h_AnimationFrom = value;
                RaisePropertyChanged("HelpButton_AnimationFrom");
            }
        }
        public int HelpButton_AnimationTo
        {
            get { return h_AnimationTo; }
            set
            {
                h_AnimationTo = value;
                RaisePropertyChanged("HelpButton_AnimationTo");
            }
        }
        public ImageSource Maximize_Restore_Icon
        {
            get { return minimize_restore_icon; }
            set
            {
                minimize_restore_icon = value;
                RaisePropertyChanged("Maximize_Restore_Icon");
            }
        }

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

        private ProjectConfig _projectConfigs;
        public ProjectConfig ProConfigs
        {
            get { return _projectConfigs; }
            set
            {
                _projectConfigs = value;
                RaisePropertyChanged("ProConfigs");
            }
        }

        private ProjectConfig _configs;
        public ProjectConfig Configs
        {
            get { return _configs; }
            set
            {
                _configs = value;
                RaisePropertyChanged("Configs");
            }
        }

        private LicenseInfo _licenseInfo;
        public LicenseInfo LicenseInfo
        {
            get { return _licenseInfo; }
            set
            {
                _licenseInfo = value;
                RaisePropertyChanged("LicenseInfo");
            }
        }

     



        private bool _UIEnabled = true;

        public bool UIEnabled
        {
            get { return _UIEnabled; }
            set
            {
                _UIEnabled = value;
                RaisePropertyChanged("UIEnabled");
            }
        }



        private string _project_title;
        public string Project_Title
        {
            get { return _project_title; }
            set
            {
                _project_title = value;
                RaisePropertyChanged("Project_Title");
            }
        }
        private string _project_directory;
        public string Project_Directory
        {
            get { return _project_directory; }
            set
            {
                _project_directory = value;
                RaisePropertyChanged("Project_Directory");
            }
        }

      

        public MainWindow()
        {
            InitializeComponent();
            var errmsg = "";

            LicenseInfo = new LicenseInfo();  //Only need to create license object once in beginning!

            


            //try to deserialize th xml and populate licenseinfo
            errmsg = LicenseInfo.ParseLocalLicenseXmlFile();

            //issue while parsing xml file, just show error message
            if (string.IsNullOrEmpty(errmsg))
            {
                LicenseInfo.Login();
            }
            else if(errmsg != "XmlDoNotExist")
            {
                var ui = new WinError(errmsg);
                var res = ui.ShowDialog();
                if (res != null && (bool)res)
                {
                    LicenseInfo.Register();
                    if (!LicenseInfo.HasPMBusTool && Main_View.Content == registertool_view)
                    {
                        System_Button_Click(this, new RoutedEventArgs());
                    }
                }
            }


            ProConfig = new ProjectConfig() { Chips = new System.Collections.ObjectModel.ObservableCollection<MPS.Common.Model.Chip>() };
            
            DataContext = this;

            evaluation_view = new Evaluation_View(ProConfig);
            registertool_view = new RegisterTool(this);

            ucsm_view = new MPS.VBP.StateMachineDiagram.FlowChart.FlowchartEditor();
          



            device_view = new Device(ProConfig, this);
            var devView = device_view.DataContext as DeviceView;
            system_view = new System_View(ProConfig, devView, this);
            SystemViewVM = system_view.DataContext as SystemView;

            //MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            Application.Current.MainWindow.BorderThickness = new Thickness(6, 6, 6, 0);
            //Application.Current.MainWindow.BorderThickness = new Thickness(6);
            Maximize_Restore_Icon = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/restore_down_white_64x64.png"));

            Main_View.Content = system_view;
            //RefreshDeiveMethod();

            Project_Title = "";

            #region slave address monitor task
            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            if (ProConfig == null)
            //            {
            //                System.Threading.Thread.Sleep(1000);
            //                continue;
            //            }

            //            //lock (_lockObj)
            //            {
            //                foreach (var chip in ProConfig.Chips)
            //                {
            //                    try
            //                    {
            //                        while (!DeviceObject.Devices.PMBus.SetStatus(true, "addr responding"))
            //                        {
            //                            System.Threading.Thread.Sleep(1000);
            //                        }
            //                        //Console.WriteLine("--------------- address in ---------------");
            //                        UpdateAddressAvaliable(chip);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine(ex);
            //                        if (!chip.ChipAddr.Contains("v"))
            //                        {
            //                            chip.IsAddrAvalible = false;
            //                        }
            //                    }
            //                    finally
            //                    {
            //                        //Console.WriteLine("--------------- address out ---------------");
            //                        DeviceObject.Devices.PMBus.SetStatus(false, "-addr responding");

            //                    }
            //                }
            //            }
            //            System.Threading.Thread.Sleep(1000);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("Error occurs in AddressAvaliableLoop: " + ex.Message);
            //        }
            //    }

            //});

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        ConstantModel.FileStatus = ConstantModel.ProjectSaveStatus.GetStatus(ProConfig);
                        System.Threading.Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex);
                    }
                }
            });
            #endregion

            ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);

            // Subscribe Event to adjust window when Windows Display settings changed by user
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

        }

        //public void UpdateAddressAvaliable(Chip chip)
        //{
        //    if (chip.ChipAddr.Contains("v"))
        //    {
        //        chip.IsAddrAvalible = null;
        //        return;
        //    }
        //    if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
        //    {
        //        chip.IsAddrAvalible = false;
        //        return;
        //    }
        //    byte[] ReadSendDate = new byte[2];
        //    byte[] Outdata = new byte[4];

        //    if (chip.ChipName == "MPQ2026" || chip.ChipName == "MPQ2022" || chip.ChipName == "MPQ2024" || chip.ChipName == "MPQ2026A" || chip.ChipName == "MPQ2022A" || chip.ChipName == "MPQ2024A")
        //    {
        //        byte val = 0;
        //        DeviceObject.Devices.PMBus.ReadByte(Convert.ToByte(chip.ChipAddr, 16), 0xD8, out val, true);
        //        if (val == 0)                          //This judgment condition is not applicable to flash chip    --from lance
        //        {
        //            chip.IsAddrAvalible = false;
        //        }
        //        else
        //        {
        //            chip.IsAddrAvalible = true;
        //        }
        //    }
        //    //else if (chip.ChipName == "MPQ7225" || chip.ChipName == "MPQ7227" || chip.ChipName == "MPQ7228")    //0x02 will not be zero from Luke
        //    //{
        //    //    var Mode = chip.ChipName == "MPQ7225" || chip.ChipName == "MPQ7227" ? 1 : 2;
        //    //    ReadSendDate[0] = (byte)((Convert.ToByte(chip.ChipAddr, 16) << 4) + 5);
        //    //    ReadSendDate[1] = 0x02;
        //    //    DeviceObject.Devices.PMBus.WriteReadNBytes(ReadSendDate, ref Outdata, true, (byte)Mode);      //Read the remaining programming times
        //    //    var ReturnValue = Outdata[1] + (Outdata[2] << 8);
        //    //    if (ReturnValue == 0)
        //    //    {
        //    //        chip.IsAddrAvalible = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        chip.IsAddrAvalible = true;
        //    //    }
        //    //}
        //    #region  VBP4X-2993 
        //    /*            else if (chip.ChipName == "MPQ7210" || chip.ChipName == "MPQ7240" || chip.ChipName == "MPQ7210A")   //7210:0x01 will not be zero from Garden 7240 :0x1B
        //                {
        //                    ushort data = 0;
        //                    DeviceObject.Devices.PMBus.SetSPIFrequency(0x0B, EnumUPDOWNType.DOWN, EnumCSType.LOW);
        //                    DeviceObject.Devices.PMBus.SPIWriteReadPEC(0x00, (byte)((0x01 << 1) + 1), ref data, true);
        //                    if (data == 0 || data == 0xFFFF)
        //                    {
        //                        DeviceObject.Devices.PMBus.SPIWriteReadPEC(0x00, (byte)((0x1B << 1) + 1), ref data, true);
        //                        if (data == 0 || data == 0xFFFF)
        //                        {
        //                            chip.IsAddrAvalible = false;
        //                        }
        //                        else
        //                        {
        //                            chip.IsAddrAvalible = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        chip.IsAddrAvalible = true;
        //                    }
        //                }
        //                else if (chip.ChipName == "MPQ7241")   //0x1B will not be zero from Garden
        //                {
        //                    ReadSendDate[0] = (byte)((Convert.ToByte(chip.ChipAddr, 16) << 4) + 5);
        //                    ReadSendDate[1] = 0x1B;
        //                    DeviceObject.Devices.PMBus.WriteReadNBytes(ReadSendDate, ref Outdata, true);
        //                    var data = (Outdata[1] + (Outdata[2] << 8));
        //                    if (data == 0 || data == 0xFFFF)
        //                    {
        //                        chip.IsAddrAvalible = false;
        //                    }
        //                    else
        //                    {
        //                        chip.IsAddrAvalible = true;
        //                    }
        //                }
        //              */
        //    #endregion
        //    else if (chip.ChipName == "MPF32010" || chip.ChipName == "MPQ70331FS" || chip.ChipName == "MP5055A" || chip.ChipName == "MPQ7902" ||
        //      chip.ChipName == "MPQ7210" || chip.ChipName == "MPQ7240" || chip.ChipName == "MPQ7210A" || chip.ChipName == "MPQ7241"
        //      || chip.ChipName == "MPQ7225" || chip.ChipName == "MPQ7228" || chip.ChipName == "MPQ7227")
        //    {
        //        chip.IsAddrAvalible = true;
        //    }
        //    else if (chip.ChipName == "MPF52002-001B" || chip.ChipName == "MPF52002-0017")
        //    {
        //        //byte regID = 0x00;
        //        //if (chip.ChipName.Contains("001B"))
        //        //{
        //        //    regID = 0x10;   
        //        //}
        //        //byte val;
        //        //var flg = DeviceObject.Devices.PMBus.ReadByte(Convert.ToByte(chip.ChipAddr, 16), regID, out val, false);
        //        //chip.IsAddrAvalible = flg && (val == 0x02);
        //    }
        //    else
        //    {

        //        var flg = DeviceObject.Devices.PMBus.ReadResponse(Convert.ToByte(chip.ChipAddr, 16), 0x1, false);
        //        if (flg)
        //        {
        //            chip.IsAddrAvalible = true;
        //        }
        //        else
        //        {
        //            flg = DeviceObject.Devices.PMBus.ReadResponse(Convert.ToByte(chip.ChipAddr, 16), 0x0, true);
        //            if (flg)
        //            {
        //                chip.IsAddrAvalible = true;
        //            }
        //            else
        //            {
        //                flg = DeviceObject.Devices.PMBus.ReadResponse(Convert.ToByte(chip.ChipAddr, 16), 0xdb, true);
        //                if (flg)
        //                {
        //                    chip.IsAddrAvalible = true;
        //                }
        //                else
        //                {
        //                    chip.IsAddrAvalible = false;
        //                }
        //            }
        //        }
        //    }
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void ChangeProjectView(byte status)
        {
            if (system_view == null)
                return;

            if (status == 0) //when there is no project created/loaded
            {
                system_view.No_Project_View.Visibility = Visibility.Visible;

                system_view.lstData.Visibility = Visibility.Hidden;
                system_view.No_Devices_View.Visibility = Visibility.Hidden;

                system_view.System_View_Buttons.Opacity = 0.25;
                system_view.System_View_Buttons.IsEnabled = false;
                device_view.AddDeviceMenu.Opacity = 0.25;
                device_view.AddDeviceMenu.IsEnabled = false;
                ProjSave.Opacity = 0.5;
                ProjSave.IsEnabled = false;
                ProjSaveAs.Opacity = 0.5;
                ProjSaveAs.IsEnabled = false;

            }
            else if (status == 1) //when there is a project created/loaded but no devices
            {
                system_view.No_Devices_View.Visibility = Visibility.Visible;

                system_view.No_Project_View.Visibility = Visibility.Hidden;
                system_view.lstData.Visibility = Visibility.Hidden;

                system_view.System_View_Buttons.Opacity = 1;
                system_view.System_View_Buttons.IsEnabled = true;
                device_view.AddDeviceMenu.Opacity = 1;
                device_view.AddDeviceMenu.IsEnabled = true;
                ProjSave.Opacity = 1;
                ProjSave.IsEnabled = true;
                ProjSaveAs.Opacity = 1;
                ProjSaveAs.IsEnabled = true;
            }
            else if (status == 2) //when there is a project created/loaded and devices
            {
                system_view.lstData.Visibility = Visibility.Visible;

                system_view.No_Project_View.Visibility = Visibility.Hidden;
                system_view.No_Devices_View.Visibility = Visibility.Hidden;

                system_view.System_View_Buttons.Opacity = 1;
                system_view.System_View_Buttons.IsEnabled = true;
                device_view.AddDeviceMenu.Opacity = 1;
                device_view.AddDeviceMenu.IsEnabled = true;
                ProjSave.Opacity = 1;
                ProjSave.IsEnabled = true;
                ProjSaveAs.Opacity = 1;
                ProjSaveAs.IsEnabled = true;
            }
        }

        #region  Sub-Window Select Events

        private void System_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_View.Content = system_view;
           
            System_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF"));
            System_Button.Opacity = 1;

            Device_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Device_Button.Opacity = 0.5;

            Project_Tools_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Project_Tools_Button.Opacity = 0.5;

            UCSM_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            UCSM_Button.Opacity = 0.5;

            RegisterTool_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            RegisterTool_Button.Opacity = 0.5;

        }

        public void Device_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_View.Content = device_view;
            if ((device_view.DataContext as DeviceView).CurrentChip != null)
                (device_view.DataContext as DeviceView).loadWaveforms();
           
            Device_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF"));
            Device_Button.Opacity = 1;

            System_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            System_Button.Opacity = 0.5;

            Project_Tools_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Project_Tools_Button.Opacity = 0.5;

            UCSM_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            UCSM_Button.Opacity = 0.5;

            RegisterTool_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            RegisterTool_Button.Opacity = 0.5;
        }


        private void Evaluation_View_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_View.Content = evaluation_view; 
            Project_Tools_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF"));
            Project_Tools_Button.Opacity = 1;

            Device_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Device_Button.Opacity = 0.5;

            System_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            System_Button.Opacity = 0.5;

            UCSM_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            UCSM_Button.Opacity = 0.5;

            RegisterTool_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            RegisterTool_Button.Opacity = 0.5;

        }

        private void UCSM_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_View.Content = ucsm_view; 
           

            UCSM_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF"));
            UCSM_Button.Opacity = 1;

            Project_Tools_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Project_Tools_Button.Opacity = 0.5;

            Device_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Device_Button.Opacity = 0.5;

            System_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            System_Button.Opacity = 0.5;

            RegisterTool_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            RegisterTool_Button.Opacity = 0.5;

        }


        private void RegisterTool_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_View.Content = registertool_view;
           
            RegisterTool_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF"));
            RegisterTool_Button.Opacity = 1;

            Project_Tools_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Project_Tools_Button.Opacity = 0.5;

            Device_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            Device_Button.Opacity = 0.5;

            System_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            System_Button.Opacity = 0.5;

            UCSM_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            UCSM_Button.Opacity = 0.5;

            if (showRegisterToolWarningWindow)
            {
                var warningWindow = new WinWarning("Use Register Tool may affect the current chip configurations. Proceed with caution.", new Thickness(15, 0, 15, 0));
                warningWindow.ShowDialog();
                if(showRegisterToolWarningWindow != false)
                {
                    showRegisterToolWarningWindow = false;
                }
            }
        }


        private void SubButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Button subbutton = (Button)sender;
            subbutton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF818181"));
        }

        private void SubButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Button subbutton = (Button)sender;
            subbutton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        #endregion

        #region window adjust
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // When Window loaded make it the Maximized
            WindowState = WindowState.Normal;
            //Maximize Window
            AdjustWindowSize();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
            }
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
                Application.Current.MainWindow.BorderThickness = new Thickness(6, 6, 6, 0);
                Maximize_Restore_Icon = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/restore_down_white_64x64.png"));
            }
            if (WindowState == WindowState.Normal)
            {
                Application.Current.MainWindow.BorderThickness = new Thickness(0);
                Maximize_Restore_Icon = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/crop_square_white_48x48.png"));
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckProjectStatus())
                return;

            Application.Current.Shutdown();
        }

        private void Maximize_Button_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            // to make sure the Window max height updated when eneter a diffrent moitor to make sure the Windows 10 drag
            // to change Applicaton window size function work normally
            UpdateWindowMaxHeight();
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            //Adjust the window when user update the Display Solutions
            UpdateWindowMaxHeight();
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Minimized;
                WindowState = WindowState.Normal;
            }
        }

        #endregion

        #region Menu Events


        private void Proj_New_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckProjectStatus())
                return;

            var sysView = system_view.DataContext as SystemView;
            

            Project_Directory = "";
            Project_Title = "<untitled>";
            ChangeProjectView(1);
            var devView = device_view.DataContext as DeviceView;
            devView.AddDeviceMethod();
            if (Main_View.Content.GetType() == typeof(Device))
            {
                System_Button_Click(null, null);
            }
            ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
        }

        private void Proj_Open_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckProjectStatus())
                return;

            var devView = device_view.DataContext as DeviceView;
            devView.OpenConfigMethod();
            ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
            Configs = ProConfig;

        }

        private void Proj_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempConfig = new ProjectConfig();
                var res = ConfigCheck_Before_SaveProj(ProConfig, out tempConfig, false);
                if (!res) return;

                string title_ = Project_Title;
                string dir_ = Project_Directory;
                if (Project_Title == "<untitled>")
                {
                    var saveDiag = new SaveFileDialog
                    {
                        Filter = "(MPS Spec File)|*.Spec",
                        Title = "Save as"
                    };
                    var showDialog = saveDiag.ShowDialog();
                    var result = showDialog != null && (bool)showDialog;
                    if (!result) return;

                    title_ = saveDiag.SafeFileName.Split('.')[0];
                    dir_ = saveDiag.FileName;
                }

                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
                tempConfig.LastModifiedToolVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                tempConfig.LastModifiedTool = titleAttribute.Title;
                MPS.Common.Parser.SchemaParser.ModelToXml(tempConfig, dir_);

                Project_Title = title_;
                Project_Directory = dir_;
                ProConfig.LastModifiedTool = tempConfig.LastModifiedToolVersion;
                ProConfig.LastModifiedToolVersion = tempConfig.LastModifiedTool;

                ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                var errUI = new WinError(ex.Message);
                errUI.ShowDialog();
            }
        }

        private void Proj_Save_As_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempConfig = new ProjectConfig();
                var res = ConfigCheck_Before_SaveProj(ProConfig, out tempConfig, false);
                if (!res) return;

                var saveDiag = new SaveFileDialog
                {
                    Filter = "(MPS Spec File)|*.Spec",
                    Title = "Save as"
                };
                var showDialog = saveDiag.ShowDialog();
                var result = showDialog != null && (bool)showDialog;
                if (!result) return;

                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
                tempConfig.LastModifiedToolVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                tempConfig.LastModifiedTool = titleAttribute.Title;
                MPS.Common.Parser.SchemaParser.ModelToXml(tempConfig, saveDiag.FileName);

                Project_Title = saveDiag.SafeFileName.Split('.')[0]; ;
                Project_Directory = saveDiag.FileName;
                ProConfig.LastModifiedTool = tempConfig.LastModifiedToolVersion;
                ProConfig.LastModifiedToolVersion = tempConfig.LastModifiedTool;

                ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                var errUI = new WinError(ex.Message);
                errUI.ShowDialog();
            }

        }

        //private void ProjImportTxt_Click(object sender, RoutedEventArgs e)
        //{
        //    var defaultConfig = UtilityMTPFactory.ImportTxtMethod();
        //    if (defaultConfig == null)
        //        return;

        //    if (!CheckProjectStatus())
        //        return;

        //    Project_Directory = "";
        //    Project_Title = "<untitled>";

        //    var defaultchip = defaultConfig.Chips.FirstOrDefault();

        //    //To prevent add identical address
        //    if (defaultchip.ChipName == "MPF32010")
        //    {
        //        if (ProConfig.Chips.Any(o => o.ChipName == "MPF32010"))
        //        {
        //            MessageBox.Show("MPF32010" + " already exists.");
        //            return;
        //        }
        //        defaultchip.ChipAddr = "-";
        //    }
        //    else if (defaultchip.ChipName == "MP8878")
        //    {
        //        int addr = 0x01;
        //        if (ProConfig.Chips.Any(o => o.ChipAddr == defaultchip.ChipAddr))
        //        {
        //            defaultchip.ChipAddr = "0x" + addr.ToString("X2");

        //            while (ProConfig.Chips.Any(o => o.ChipAddr == defaultchip.ChipAddr))
        //            {
        //                addr = Convert.ToByte(defaultchip.ChipAddr, 16) + 1;
        //                defaultchip.ChipAddr = "0x" + addr.ToString("X2");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        while (ProConfig.Chips.Any(o => o.ChipAddr == defaultchip.ChipAddr))
        //        {
        //            var addr = Convert.ToByte(defaultchip.ChipAddr, 16) + 1;
        //            defaultchip.ChipAddr = "0x" + addr.ToString("X2");
        //        }
        //    }

        //    if (ProConfig.Chips.Count == 0)
        //    {
        //        ProConfig.Product = defaultConfig.Product;
        //        ProConfig.BaseProduct = defaultConfig.BaseProduct;
        //        ProConfig.CreatedDate = defaultConfig.CreatedDate;
        //        ProConfig.CreatedTool = defaultConfig.CreatedTool;
        //        ProConfig.CreatedToolVersion = defaultConfig.CreatedToolVersion;
        //        ProConfig.LastModifiedDate = defaultConfig.LastModifiedDate;
        //        ProConfig.LastModifiedTool = defaultConfig.LastModifiedTool;
        //        ProConfig.LastModifiedToolVersion = defaultConfig.LastModifiedToolVersion;
        //        ProConfig.SpecVersion = defaultConfig.SpecVersion;
        //        ChangeProjectView(2);

        //    }

        //    //Create Monitor
        //    defaultchip.Monitor = MonitorManager.CreateInstance(defaultchip, false);
        //    if (defaultchip.Monitor != null)
        //    {
        //        if (defaultchip.Monitor.LineGraphs.Count == 0)
        //        {
        //            defaultchip.Monitor.LoadMonitorValue();
        //        }
        //        defaultchip.Monitor.isRunning = true;
        //    }

        //    while (ProConfig.Chips.Any(chip => chip.ChipID == defaultchip.ChipID))
        //    {
        //        defaultchip.ChipID = defaultchip.ChipID + "*";
        //    }
        //    ProConfig.Chips.Add(defaultchip);

        //    var devView = device_view.DataContext as DeviceView;
        //    devView._logDataDic.Add(defaultchip.ChipAddr, new ObservableCollection<LogModel>());
        //    devView._isOpenFileDic.Add(defaultchip.ChipAddr, true);

        //    devView.ChangeChipSelectionMethod(ProConfig.Chips.LastOrDefault());
        //}



        private void ProjImportTxt_Click(object sender, RoutedEventArgs e)
        {
            var defaultConfig = UtilityMTPFactory.ImportTxtMethod(LicenseInfo.IsMPSAE);
            if (defaultConfig == null)
                return;
            else
            {
                ImportedPartFilter(LicenseInfo, defaultConfig);
                if (defaultConfig.Chips.Count == 0)
                    return;
            }

            if (!CheckProjectStatus())
                return;

            var devView = device_view.DataContext as DeviceView;

            Project_Directory = "";
            Project_Title = "<untitled>";

            foreach (var defaultchip in defaultConfig.Chips)
            {
                //To prevent add identical address
                if (defaultchip.ChipName == "MPF32010")
                {
                    if (ProConfig.Chips.Any(o => o.ChipName == "MPF32010"))
                    {
                        //MessageBox.Show("MPF32010" + " already exists.");
                        var errUI = new WinError("MPF32010" + " already exists.");
                        errUI.ShowDialog();
                        return;
                    }
                    defaultchip.ChipAddr = "-";
                }
                else if (defaultchip.ChipName == "MP8878")
                {
                    int addr = 0x01;
                    if (ProConfig.Chips.Any(o => o.ChipAddr == defaultchip.ChipAddr))
                    {
                        defaultchip.ChipAddr = "0x" + addr.ToString("X2");

                        while (ProConfig.Chips.Any(o => o.ChipAddr == defaultchip.ChipAddr))
                        {
                            addr = Convert.ToByte(defaultchip.ChipAddr, 16) + 1;
                            defaultchip.ChipAddr = "0x" + addr.ToString("X2");
                        }
                    }
                }
                else
                {
                    while (ProConfig.Chips.Any(o => o.ChipAddr == defaultchip.ChipAddr))
                    {
                        var addr = Convert.ToByte(defaultchip.ChipAddr, 16) + 1;
                        defaultchip.ChipAddr = "0x" + addr.ToString("X2");
                    }
                }

                if (ProConfig.Chips.Count == 0)
                {
                    ProConfig.Product = defaultConfig.Product;
                    ProConfig.BaseProduct = defaultConfig.BaseProduct;
                    ProConfig.CreatedDate = defaultConfig.CreatedDate;
                    ProConfig.CreatedTool = defaultConfig.CreatedTool;
                    ProConfig.CreatedToolVersion = defaultConfig.CreatedToolVersion;
                    ProConfig.LastModifiedDate = defaultConfig.LastModifiedDate;
                    ProConfig.LastModifiedTool = defaultConfig.LastModifiedTool;
                    ProConfig.LastModifiedToolVersion = defaultConfig.LastModifiedToolVersion;
                    ProConfig.SpecVersion = defaultConfig.SpecVersion;
                    ChangeProjectView(2);

                }

                //Create Monitor
                defaultchip.Monitor = MonitorManager.CreateInstance(defaultchip, false, LicenseInfo.IsMPSAE);
                if (defaultchip.Monitor != null)
                {
                    if (defaultchip.Monitor.LineGraphs.Count == 0)
                    {
                        defaultchip.Monitor.LoadMonitorValue();
                    }
                    defaultchip.Monitor.isRunning = true;
                   
                }

                while (ProConfig.Chips.Any(chip => chip.ChipID == defaultchip.ChipID))
                {
                    defaultchip.ChipID = defaultchip.ChipID + "*";
                }
                ProConfig.Chips.Add(defaultchip);

                devView._logDataDic.Add(defaultchip.ChipID, new ObservableCollection<LogModel>());
                devView._isOpenFileDic.Add(defaultchip.ChipID, true);
            }
         
            devView.ChangeChipSelectionMethod(ProConfig.Chips.LastOrDefault());
            Configs = ProConfig;
        }

        private void Proj_Close_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckProjectStatus())
                return;

            ChangeProjectView(0);
            Project_Title = "";
            Project_Directory = "";
            if (Main_View.Content.GetType() == typeof(Device))
            {
                System_Button_Click(null, null);
            }
            ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
        }

        private void Proj_Exit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckProjectStatus())
                return;

            Application.Current.Shutdown();
        }


        private void Help_User_Manual_Click(object sender, RoutedEventArgs e)
        {
            string dPath = @".\Document\VBP4.0 User Manual.pdf";
            System.Diagnostics.Process.Start(dPath);
        }

        private void Help_Update_Check_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUpdateStatus.CheckUpdate())
            {
                var ver = string.IsNullOrEmpty(CheckUpdateStatus.Version) ? "" : string.Format(" ({0})", CheckUpdateStatus.Version);
                var upWin = new WinUpdate(ver);
                upWin.ShowDialog();
            }
            else
            {
                WinVersionWarning winversion = new Views.DeviceViews.WinVersionWarning();
                winversion.ShowDialog();
            }

        }

        private void Help_Kit_Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIEnabled = false;
                var ui2 = new WinSaveProjectConfirm();
                if (ProConfig.Chips.Count > 0)
                {
                    ui2.ShowDialog();
                }
                else
                {
                    BusyIndicator.LoadingIndicator.LoadingUIRun1("Kit Detect");

                    if (DeviceObject.Devices.PMBus.IsDeviceAvailable())
                    {
                        var kit_ver = DeviceObject.Devices.PMBus.CheckKitVersion();
                        var fw_rev = DeviceObject.Devices.PMBus.CheckKitType();

                        BusyIndicator.LoadingIndicator.LoadingUIStop();
                        var upWin = new WinKitVersion(kit_ver, fw_rev);
                        upWin.ShowDialog();
                    }
                    else
                    {
                        throw new Exception("Kit not connected!");
                    }
                }
            }
            catch (Exception ex)
            {
                BusyIndicator.LoadingIndicator.LoadingUIStop();
                string error_content = ex.Message;
                if (ex.GetType() == typeof(NotImplementedException))
                {
                    error_content = "The current dongle does NOT support version checking.";
                }
                var winError = new WinError(error_content);
                winError.ShowDialog();
            }
            finally
            {
                UIEnabled = true;
            }
        }

        private void Help_About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new Views.DeviceViews.WinAbout();
            aboutWindow.ShowDialog();
        }

        private void License_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var devView = device_view.DataContext as DeviceView;

                UIEnabled = false;
                if (string.IsNullOrEmpty(LicenseInfo.Code))
                {
                    LicenseInfo.Register();
                }
                else
                {
                    var ui = new WinLicenseDetails(LicenseInfo);
                    var ischange = ui.ShowDialog();
                    var ui2 = new WinSaveProjectConfirm();

                    if (ui.RemoveFlg || (bool)ischange)
                    {
                        if (ProConfig.Chips.Count > 0)
                        {
                            ui2.ShowDialog();
                        }
                        else
                        {
                            if ((bool)ischange)
                            {
                                LicenseInfo.Register();
                            }
                            else
                            {
                                var winLicense = new WinLicenseRemove();
                                winLicense.ShowDialog();
                                if ((bool)winLicense.DialogResult)
                                {
                                    LicenseInfo.ClearLicenseInfo();
                                    ui.licenseInformation = null;
                                    ProConfig.Chips.Clear();
                                    devView.CurrentChip = null;
                                    //Main_View.Content = system_view;
                                    System_Button_Click(this, new RoutedEventArgs());
                                }
                                else { return; }
                            }
                            
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
            finally
            {
                UIEnabled = true;
            }
        }

        #endregion



        public bool CheckProjectStatus()
        {
            //ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
            if (ConstantModel.FileStatus)
            {
                //while (ProConfig.Chips.Count != 0)
                //    (device_view.DataContext as DeviceView).DeleteChipMethod(ProConfig.Chips[0]);

                (device_view.DataContext as DeviceView).ClearBindingData();
                return true;
            }
            else
            {
                CloseConfirmation cf = new CloseConfirmation();
                cf.ShowDialog();
                if (cf.CloseConfirm)
                {
                    //while (ProConfig.Chips.Count != 0)
                    //    (device_view.DataContext as DeviceView).DeleteChipMethod(ProConfig.Chips[0]);
                    (device_view.DataContext as DeviceView).ClearBindingData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ImportedPartFilter(LicenseInfo license, ProjectConfig pjconfig)
        {
            ObservableCollection<string> chipList;
            if (license._chipList_internal != null && license._chipList_internal.Count > 0)
            {
                chipList = license._chipList_internal;
            }
            else
            {
                chipList = license._chipList_public;
            }
            List<string> lst = new List<string>();
            foreach (var chip in pjconfig.Chips)
            {
                if (!chipList.Contains(chip.ChipName))
                {
                    lst.Add(chip.ChipName);
                }
            }

            if (lst.Count > 0)
            {
                foreach (var chipname in lst)
                {
                    pjconfig.Chips.Remove(pjconfig.Chips.FirstOrDefault(o => o.ChipName == chipname));
                }
                lst = lst.Where((x, i) => lst.FindIndex(o => o == x) == i).ToList();
                var str = "";
                foreach (var item in lst)
                {
                    str += item + "/";
                }

                UIEnabled = false;
                var winerror = new WinError(string.Format("Can not access to {0} without authorization. Please check your license information.", str.Trim('/')));
                winerror.ShowDialog();
                UIEnabled = true;
            }
        }

        public bool ConfigCheck_Before_SaveProj(ProjectConfig Config, out ProjectConfig tempConfig, bool isExportATE)
        {
            tempConfig = new ProjectConfig();

            if (Config == null || Config.Chips == null || Config.Chips.Count == 0)
                return false;

            bool hasUnstoredChanges = false;

            var dic = new Dictionary<string, bool>();
            foreach (var chip in Config.Chips)
            {
                if (isExportATE && !chip.IsSelected) 
                    continue;

                hasUnstoredChanges = false;
                if (chip.ChipName != "MP8850" && chip.ChipName != "MPM2896-20" && chip.ChipName != "MPM3898-50")
                {
                    foreach (var region in chip.Regions)
                    {
                        if (region.RegionName.Contains("Overview"))
                            continue;
                        foreach (var mode in region.ConfigurationModes)
                        {
                            foreach (var page in mode.Pages)
                            {
                                foreach (var reg in page.AllRegisters)
                                {
                                    if (reg.BitControls.Any(o => (o.Disabled == false) && !string.IsNullOrEmpty(o.Error)))
                                    {
                                        new WinError("There are configure errors exist. Please correct them before saving.").ShowDialog();
                                        return false;
                                    }
                                    if (reg.BitControls.Any(o => (o.Disabled == false) && (o.CurrentBitValue != o.SourceBitValue)))
                                    {
                                        hasUnstoredChanges = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var selectedmode = chip.Regions.FirstOrDefault().ConfigurationModes.FirstOrDefault(o => o.IsSelected);
                    foreach (var reg in selectedmode.Pages.FirstOrDefault().AllRegisters)
                    {
                        if (reg.BitControls.Any(o => (o.Disabled == false) && !string.IsNullOrEmpty(o.Error)))
                        {
                            new WinError("There are configure errors exist. Please correct them before saving.").ShowDialog();
                            return false;
                        }
                        if (reg.BitControls.Any(o => (o.Disabled == false) && (o.CurrentBitValue != o.SourceBitValue)))
                        {
                            hasUnstoredChanges = true;
                        }
                    }
                }
                
                dic.Add(chip.ChipID, hasUnstoredChanges);
            }
            
           
            if (dic.Any(o => o.Value == true))
            {
                var ui = new WinProceedwithUIChanges();
                var result = ui.ShowDialog();
                if (result == null || !(bool)result)
                    return false;
                tempConfig = DeepCopyProjectConfig(ProConfig, dic);
            }
            else
            {
                tempConfig = ProConfig;
            }

            return true;

        }

        private ProjectConfig DeepCopyProjectConfig(ProjectConfig config, Dictionary<string, bool> dic)
        {
            //deep copy -> copyConfig 
            var devView = (DeviceView)device_view.DataContext;
            Chip copychip;
            RegisterRegion copyregion;
            ConfigurationMode copymode;
            MPS.Common.Model.Page copypage;
            MPS.Common.Model.Register copyreg;
            MPS.Common.Model.BitControl copybt;
            ProjectConfig copyConfig = new ProjectConfig()
            {
                Product = config.Product,
                BaseProduct = config.BaseProduct,
                CreatedTool = config.CreatedTool,
                CreatedDate = config.CreatedDate,
                LastModifiedTool = config.LastModifiedTool,
                CreatedToolVersion = config.CreatedToolVersion,
                SpecVersion = config.SpecVersion,
                Chips = new ObservableCollection<Chip>()
            };
            foreach (var item in dic)
            {
                Chip chip = config.Chips.FirstOrDefault(o => o.ChipID == item.Key);
                if (item.Value == false)
                {
                    copyConfig.Chips.Add(chip);
                    continue;
                }

                copychip = MPS.VBP.PartFactory.DataBaseHelper.LoadConfig(chip.ChipName, LicenseInfo.IsMPSAE).Chips.FirstOrDefault();
                copychip.ChipAddr = chip.ChipAddr;
                copychip.ChipID = chip.ChipID;
                copychip.IsSelected = chip.IsSelected;
                foreach (var region in chip.Regions)
                {
                    if (region.RegionName.Contains("Overview"))
                        continue;
                    copyregion = copychip.Regions.FirstOrDefault(o => o.RegionName == region.RegionName);
                    copyregion.IsSelected = region.IsSelected;
                    foreach (var mode in region.ConfigurationModes)
                    {
                        copymode = copyregion.ConfigurationModes.FirstOrDefault(o => o.ModeName == mode.ModeName);
                        copymode.IsSelected = mode.IsSelected;
                        foreach (var page in mode.Pages)
                        {
                            copypage = copymode.Pages.FirstOrDefault(o => o.PageName == page.PageName);
                            copypage.isVisible = page.isVisible;
                            copypage.IsSelected = page.IsSelected;
                            foreach (var reg in page.AllRegisters)
                            {
                                copyreg = copypage.AllRegisters.FirstOrDefault(o => o.Name == reg.Name && o.Address == reg.Address);
                                copyreg.Value = reg.Value;
                                copyreg.Disabled = reg.Disabled;
                                for (int i = 0; i < reg.BitControls.Count(); i++)
                                {
                                    var bt = reg.BitControls[i];
                                    copybt = copyreg.BitControls[i];
                                    copybt.CurrentBitValue = bt.CurrentBitValue;
                                    copybt.SourceBitValue = bt.SourceBitValue;
                                    copybt.Disabled = bt.Disabled;
                                    copybt.isToggleOn = bt.isToggleOn;
                                    copybt.isEnable = bt.isEnable;
                                    copybt.MinValue = bt.MinValue;
                                    copybt.MaxValue = bt.MaxValue;
                                    copybt.Lsb = bt.Lsb;
                                    copybt.Offset = bt.Offset;
                                }

                            }
                        }
                    }
                }

                var comObj = CommunicationFactory.CreateInstance(copychip.ChipAddr, copychip.ChipName);
                comObj.VirtualWriteAllRegisters(copychip);
                copyConfig.Chips.Add(copychip);
            }

            return copyConfig;
        }

    }
}
