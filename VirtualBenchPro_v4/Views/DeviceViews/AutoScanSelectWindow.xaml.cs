using MPS.Common;
using MPS.Instrument;
using MPS.VBP.ScanMethodLibrary.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.ViewModel;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for AutoScanSelectWindow.xaml
    /// </summary>
    public partial class AutoScanSelectWindow : Window
    {
        public AutoScanSelectWindow(LicenseInfo license)
        {
            InitializeComponent();
            var chiplist = AutoScanMethod();
            var licensedList = (license._chipList_internal != null && license._chipList_internal.Count > 0) ? license._chipList_internal : license._chipList_public;
            var filterchiplist = new ObservableCollection<ChipItem>();
            foreach (var cp in chiplist)
            {
                if (licensedList.Contains(cp.ChipName))
                {
                    filterchiplist.Add(cp);
                }
            }
            if (filterchiplist.Count > 0)
            {
                Title_Label.Content = "Device detected";
                Info_TextBlock.Text = "Virtual Bench Pro has detected the following connected devices. Select the ones you would like to add to your system and hit \"Continue\".";
                Func_Button.Content = "Continue";
                chip_ListBox.ItemsSource = filterchiplist;
            }
            else
            {
                Title_Label.Content = "Device not detected";
                Info_TextBlock.Text = "Virtual Bench Pro could not find any connected devices. Would you like to add one manually?";
                Func_Button.Content = "Add Manually";
            }
            this.DataContext = new AutoScanSelectView(filterchiplist);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Login_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            var obj = e.Parameter as AutoScanSelectView;

            if (obj == null)
            {
                e.CanExecute = false;
                Func_Button.Opacity = 0.5;
                return;
            }
            if (Title_Label.Content.ToString().Contains("Device not detected"))
            {
                e.CanExecute = true;
                Func_Button.Opacity = 1;
                return;
            }

            if (obj.ChipList.Any(o => o.IsSelected == true))
            {
                e.CanExecute = true;
                Func_Button.Opacity = 1;
            }
            else
            {
                e.CanExecute = false;
                Func_Button.Opacity = 0.5;
            }
        }

        private void Login_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
        }

        public ObservableCollection<ChipItem> AutoScanMethod()
        {
            ObservableCollection<ChipItem> chiplist = new ObservableCollection<ChipItem>();
            try
            {
                var seconds = 0;
                TimeSpan timeSpan = new TimeSpan();
                BusyIndicator.LoadingIndicator.LoadingUIRun();

                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                    return chiplist;

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                for (byte i = 0; i < 0x80; i++)
                {

                    string partnumber = string.Empty;

                    //if (i == 81 || i == 0x0c)//|| i == 0
                    //    continue;
                    var flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x1, false);
                    if (!flg)
                    {
                        flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x0, false);
                    }
                    if (!flg)
                    {
                        flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x78, false);
                    }
                    if (!flg)
                    {
                        stopwatch.Stop();
                        timeSpan = stopwatch.Elapsed;
                        stopwatch.Reset();
                        seconds = seconds + timeSpan.Seconds;
                        if (seconds > 60)
                        {
                            break;
                        }
                        else
                        {
                            stopwatch.Start();
                            continue;
                        }
                    }

                    partnumber = ConfirmPartnumber(i);

                    if (string.IsNullOrEmpty(partnumber))
                        continue;

                    //chiplist.Add(new ChipItem(string.Format("{0} (0x{1})", partnumber, i.ToString("X2")), false));
                    chiplist.Add(new ChipItem(partnumber, "0x" + i.ToString("X2"), false));
                }

                if (chiplist.Count > 0)
                    chiplist[0].IsSelected = true;

                return chiplist;
            }
            catch (Exception)
            {
                return chiplist;
            }
            finally
            {
                BusyIndicator.LoadingIndicator.LoadingUIStop();
            }


        }


        private string ConfirmPartnumber(byte i)
        {
            string partnumber = string.Empty;

            #region 0x0D
            ushort add;
            DeviceObject.Devices.PMBus.ReadWord(i, 0x0D, out add);
            if (add == 0x3599)
            {
                return EnumPartnumber.MPM3599.ToString();
            }
            #endregion

            #region 0x9D

            ushort mfr;
            DeviceObject.Devices.PMBus.ReadWord(i, 0x9b, out mfr);
            byte[] Rev = new byte[6];
            bool IsBlock = DeviceObject.Devices.PMBus.ReadBlock(i, 0x9d, ref Rev);

            if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x39 && mfr == 0x3201)
            {
                return EnumPartnumber.MPM3895_25.ToString().Replace('_', '-');
            }
            if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x3a)
            {
                return EnumPartnumber.mEZDPD1620AS.ToString();
            }
            if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x3b)
            {
                return EnumPartnumber.MPM82504.ToString();
            }
            if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x3D)
            {
                return EnumPartnumber.MPM3690_50D.ToString().Replace('_', '-');
            }
            if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x3E)
            {
                return EnumPartnumber.CDM1680.ToString();
            }
            if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x3F && mfr == 0x3201)
            {
                return EnumPartnumber.MPM3695_20.ToString().Replace('_', '-');
            }
            #endregion

            #region 0x9A
            IsBlock = DeviceObject.Devices.PMBus.ReadBlock(i, 0x9A, ref Rev);
            if (IsBlock && Rev.Length >= 1 && Rev[0] == 0x98)
            {
                return EnumPartnumber.MPM3698.ToString();
            }

            byte[] MfrModel = new byte[7];
            bool IsBlock1 = DeviceObject.Devices.PMBus.ReadBlock(i, 0x9A, ref MfrModel);
            if (IsBlock1 && MfrModel.Length >= 7)
            {
                //MP8878
                //if Chip is in MPQ8646 Series
                if (MfrModel[0] == 0x36 && MfrModel[1] == 0x34 && MfrModel[2] == 0x36 && MfrModel[3] == 0x38 && MfrModel[4] == 0x51 && MfrModel[5] == 0x50 && MfrModel[6] == 0x4D)
                {
                    // if 0xC1[12] = 1, it is MP8878
                    ushort tempWord;
                    DeviceObject.Devices.PMBus.ReadWord(i, 0xC1, out tempWord);
                    if (((tempWord >> 12) & 1) == 1)
                    {
                        return EnumPartnumber.MP8878.ToString();
                    }
                }
            }
            else if (IsBlock1 && MfrModel.Length >= 6)
            {
                byte byteCount;
                DeviceObject.Devices.PMBus.ReadByte(i, 0x9A, out byteCount);

                //MP8797B 4D_50_38_37_39_37
                if (MfrModel[0] == 0x37 && MfrModel[1] == 0x39 && MfrModel[2] == 0x37 && MfrModel[3] == 0x38 && MfrModel[4] == 0x50 && MfrModel[5] == 0x4D && byteCount == 0x06)
                {
                    return EnumPartnumber.MP8797B.ToString();
                }
                //MPM3700  4D_50_33_37_30_30
                if (MfrModel[0] == 0x30 && MfrModel[1] == 0x30 && MfrModel[2] == 0x37 && MfrModel[3] == 0x33 && MfrModel[4] == 0x50 && MfrModel[5] == 0x4D && byteCount == 0x06)
                {
                    return EnumPartnumber.MPM3700.ToString();
                }
            }
            #endregion

            #region 0x9B
            ushort mfrReversion;
            DeviceObject.Devices.PMBus.ReadWord(i, 0x9b, out mfrReversion);
            if (mfrReversion == 0x3001)
            {
                return EnumPartnumber.MPQ8645P.ToString();
            }
            if (mfrReversion == 0x3101)
            {
                return EnumPartnumber.MPM3695_10.ToString().Replace('_', '-');
            }
            if (mfrReversion == 0x3201)
            {
                return EnumPartnumber.MPM3695_25.ToString().Replace('_', '-');
            }
            if (mfrReversion == 0x3301)
            {
                return EnumPartnumber.mEZD81225A.ToString();
            }
            if (mfrReversion == 0x3401)
            {
                return EnumPartnumber.MPM3695A_10.ToString().Replace('_', '-');
            }
            if (mfrReversion == 0x3501)
            {
                return EnumPartnumber.mEZDPD1620A.ToString();
            }
            if (mfrReversion == 0x3601)
            {
                return EnumPartnumber.MPM3695_100.ToString().Replace('_', '-');
            }
            if (mfrReversion == 0x3701)
            {
                Rev = new byte[6];
                IsBlock = DeviceObject.Devices.PMBus.ReadBlock(i, 0x9d, ref Rev);
                if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x32)
                {
                    return EnumPartnumber.MP8796B.ToString();
                }
                else if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x33)
                {
                    return EnumPartnumber.MP9795B.ToString();
                }
                else if (IsBlock && Rev.Length >= 3 && Rev[2] == 0x38)
                {
                    return EnumPartnumber.MP8796K.ToString();
                }
            }
            if (mfrReversion == 0x3801)
            {
                return EnumPartnumber.mEZDPD1620AS.ToString();
            }
            if (mfrReversion == 0x3901)
            {
                return EnumPartnumber.MPM3695_7.ToString().Replace('_', '-');
            }
            #endregion

            #region 0xDD 0xD6 0xF0 0x31 0xE5 0x61 0x60
            byte id;
            DeviceObject.Devices.PMBus.ReadByte(i, 0xdd, out id);
            byte id1;
            DeviceObject.Devices.PMBus.ReadByte(i, 0xd6, out id1);
            byte id2;
            DeviceObject.Devices.PMBus.ReadByte(i, 0xf0, out id2);
            byte id3;
            DeviceObject.Devices.PMBus.ReadByte(i, 0x31, out id3);
            byte id4;
            DeviceObject.Devices.PMBus.ReadByte(i, 0xE5, out id4);
            ushort id5;
            DeviceObject.Devices.PMBus.ReadWord(i, 0x61, out id5);
            ushort id7;
            DeviceObject.Devices.PMBus.ReadWord(i, 0x60, out id7);

            if (id4 == 0x32)
            {
                return EnumPartnumber.MPM54322.ToString();
            }
            if (id4 == 0x52)
            {
                return EnumPartnumber.MPM54522.ToString();
            }
            if (id4 ==0x53) 
            {
                return EnumPartnumber.MPM54532GPB.ToString();
            }
            if (id == 0x06 && id7 != 0x2946)
            {
                return EnumPartnumber.MP8855.ToString();
            }
            if (id == 0x08)
            {
                return EnumPartnumber.MPQ8886.ToString();
            }
            if (id == 0x09)
            {
                return EnumPartnumber.MPM3596.ToString();
            }
            if (id == 0x0A)
            {
                return EnumPartnumber.mEZDPD4506AS.ToString();
            }
            if (id == 0x70) //0x0B
            {
                return EnumPartnumber.MP8870.ToString();
            }
            if (id == 0x0C)
            {
                return EnumPartnumber.MPQ8880.ToString();
            }
            if (id1 == 0x0D)
            {
                return EnumPartnumber.MPQ4272.ToString();
            }

            if (id2 == 0x51 && i != 0)  //0x0E
            {
                return EnumPartnumber.mEZD81260A.ToString();
            }

            if (id3 == 0x13)
            {
                return EnumPartnumber.MPM54313.ToString();
            }
            if (id3 == 0x24)
            {
                return EnumPartnumber.MPM54524.ToString();
            }
            if (id5 == 0x2977)
            {
                return EnumPartnumber.MPQ2977.ToString();
            }
            if (id7 == 0x2946)
            {
                return EnumPartnumber.MPQ2946.ToString();
            }
            #endregion

            if (partnumber == "")
            {
                DeviceObject.Devices.PMBus.WriteByte(i, 0xFF, 0x01);
                byte id6;
                DeviceObject.Devices.PMBus.ReadByte(i, 0xB8, out id6);
                if (id6 == 0x67)
                {
                    return EnumPartnumber.MPQ2967.ToString();
                }
            }

            return partnumber;
        }

        #region  Useless

        //private string ConfirmPartnumber(byte i)
        //{
        //    //byte[] Rev = new byte[6];
        //bool IsBlock = DeviceObject.Devices.PMBus.ReadBlock(i, 0x9d, ref Rev, true);
        //if (IsBlock && Rev.Length > 2 && Rev[2] == 0x3a)
        //{
        //    partnumber = EnumPartnumber.mEZDPD1620AS.ToString();
        //}
        //else if (IsBlock && Rev.Length > 2 && Rev[2] == 0x3b)
        //{
        //    partnumber = EnumPartnumber.MPM82504.ToString();
        //}
        //else if (IsBlock && Rev.Length > 2 && Rev[2] == 0x3D)
        //{
        //    //partnumber = EnumPartnumber.MPM3690_50D.ToString();
        //    partnumber = "MPM3690-50D";
        //}
        //else
        //{
        //    #region pn confirm with 0x9b
        //    ushort mfrReversion;
        //    DeviceObject.Devices.PMBus.ReadWord(i, 0x9b, out mfrReversion);
        //    if (mfrReversion == 0x3001)
        //    {
        //        partnumber = EnumPartnumber.MPQ8645P.ToString();
        //    }
        //    else if (mfrReversion == 0x3101)
        //    {
        //        partnumber = EnumPartnumber.MPM3695_10.ToString();
        //    }
        //    else if (mfrReversion == 0x3201)
        //    {
        //        partnumber = EnumPartnumber.MPM3695_25.ToString();
        //    }
        //    else if (mfrReversion == 0x3301)
        //    {
        //        partnumber = EnumPartnumber.mEZD81225A.ToString();
        //    }
        //    else if (mfrReversion == 0x3401)
        //    {
        //        partnumber = EnumPartnumber.MPM3695A_10.ToString();
        //    }
        //    else if (mfrReversion == 0x3501)
        //    {
        //        partnumber = EnumPartnumber.mEZDPD1620A.ToString();
        //    }
        //    else if (mfrReversion == 0x3601)
        //    {
        //        partnumber = EnumPartnumber.MPM3695_100.ToString();
        //    }
        //    else if (mfrReversion == 0x3701)
        //    {
        //        partnumber = EnumPartnumber.MP8796B.ToString();
        //    }
        //    else if (mfrReversion == 0x3801)
        //    {
        //        partnumber = EnumPartnumber.mEZDPD1620AS.ToString();
        //    }
        //    else if (mfrReversion == 0x3901)
        //    {
        //        partnumber = EnumPartnumber.MPM3695_7.ToString();
        //    }
        //    else
        //    {
        //        byte id;
        //        DeviceObject.Devices.PMBus.ReadByte(i, 0xdd, out id, true);
        //        if (id == 0x06)
        //        {
        //            partnumber = EnumPartnumber.MP8855.ToString();
        //        }
        //        else if (id == 0x08)  
        //        {
        //            partnumber = EnumPartnumber.MPQ8886.ToString();
        //        }
        //        else if (id == 0x09)
        //        {
        //            partnumber = EnumPartnumber.MPM3596.ToString();
        //        }
        //        else if (id == 0x0A)
        //        {
        //            partnumber = EnumPartnumber.mEZDPD4506AS.ToString();
        //        }
        //        else if (id == 0x0B)
        //        {
        //            partnumber = EnumPartnumber.MP8870.ToString();
        //        }
        //        else if (id == 0x0C)
        //        {
        //            partnumber = EnumPartnumber.MPQ8880.ToString();
        //        }
        //        else
        //        {
        //            DeviceObject.Devices.PMBus.ReadByte(i, 0xd6, out id, true);
        //            if (id == 0x0D)
        //            {
        //                partnumber = EnumPartnumber.MPQ4272.ToString();
        //            }
        //            else
        //            {
        //                DeviceObject.Devices.PMBus.ReadByte(i, 0xf0, out id, true);
        //                if (id == 0x0E)
        //                {
        //                    partnumber = EnumPartnumber.mEZD81260A.ToString();
        //                }
        //                else
        //                {
        //                    DeviceObject.Devices.PMBus.ReadByte(i, 0x31, out id, true);
        //                    if (id == 0x13)
        //                    {
        //                        partnumber = EnumPartnumber.MPM54313.ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //}
        #endregion

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
