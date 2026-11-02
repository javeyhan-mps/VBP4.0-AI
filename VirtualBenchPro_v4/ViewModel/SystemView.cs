using MPS.Common;
using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Instrument;
using MPS.VBP.PartFactory;
using MPS.VBP.AutoScan.Model;
using MPS.VBP.AutoScan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;

namespace VirtualBenchPro_v4.ViewModel
{

    public class SystemView : ObservableObject
    {
        readonly object _lockObj = new object();

        public SystemView(ProjectConfig pobj, DeviceView devView, LicenseInfo license_)
        {
            ProConfig = pobj;
            deviceViewModel = devView;
            license = license_;
        }
        public LicenseInfo license;

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

        public ObservableCollection<DataGridColumn> Columns { get; set; }

        public DeviceView deviceViewModel { get; set; }


        public ICommand ScanAddressCommand => new RelayCommand(ScanAddressMethod);

        public void ScanAddressMethod()
        {
            try
            {
                BusyIndicator.LoadingIndicator.LoadingUIRun();

                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                {
                    BusyIndicator.LoadingIndicator.LoadingUIStop();
                    var ui = new WinError("USB interface not found, please connect dongle and try again.");
                    ui.ShowDialog();
                    return;
                }

                ShowScanWindow();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ScanI2CMethod: " + ex.Message);
                BusyIndicator.LoadingIndicator.LoadingUIStop();
            }

        }

        private async void ShowScanWindow()
        {
            var scanView = new ScanView();
            var chipList = license != null && license._chipList_internal != null && license._chipList_internal.Count > 0 ?
                license._chipList_internal : license._chipList_public;
            var scanViewModel = new ScanViewModel(ProConfig, chipList.ToList());
            await scanViewModel.ParseAddressDevicePair();
            scanViewModel.AddSelectedDevicesToProjectEvent += ScanViewModel_AddSelectedDevicesToProjectEvent;
            scanView.DataContext = scanViewModel;

           
            BusyIndicator.LoadingIndicator.LoadingUIStop();

            if(scanViewModel.ScanData.Count == 0)
            {
                var ui = new WinError("No device found, please check the connection.");
                ui.ShowDialog();
                return;
            }

            scanView.ShowDialog();

        }

        private void ScanViewModel_AddSelectedDevicesToProjectEvent(object sender, List<ScanDataModel> e)
        {
            if (e != null)
            {
                e.ForEach(scanData =>
                {
                    if (scanData.IsSelected&&scanData.IsChipEnabled)
                    {
                        var newConfig = DataBaseHelper.LoadConfig(scanData.SelectedDevice, deviceViewModel.UserLicense.IsMPSAE);
                        deviceViewModel.AddDevices("", false, scanData.SelectedDevice, newConfig, scanData.Address, true,false);

                    }
                });

                if (ProConfig?.Chips?.Count > 0 && deviceViewModel.CurrentChip == null)
                    deviceViewModel.ChangeChipSelectionMethod(ProConfig.Chips.First());
            }
        }

        #region Useless Method(AutoScan)

        /*   public ICommand RefreshDeiveCommand => new RelayCommand(AutoScanSelectMethod, null);*/

        //public void RefreshDeiveMethod()
        //{
        //    try
        //    {
        //        BusyIndicator.LoadingIndicator.LoadingUIRun();
        //        deviceViewModel.ClearBindingData();
        //        //ClearBindingData();
        //        if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
        //            return;


        //        for (byte i = 0; i < 0x80; i++)
        //        {
        //            string partnumber = string.Empty;

        //            if (i == 81 || i == 0x0c)//|| i == 0
        //                continue;
        //            var flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x1);
        //            if (!flg)
        //            {
        //                flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x0);
        //            }
        //            if (!flg)
        //            {
        //                continue;
        //            }

        //            #region part confirm

        //            byte[] Rev = new byte[6];
        //            bool IsBlock = DeviceObject.Devices.PMBus.ReadBlock(i, 0x9d, ref Rev);
        //            if (IsBlock && Rev.Length > 2 && Rev[2] == 0x3a)
        //            {
        //                partnumber = EnumPartnumber.mEZDPD1620AS.ToString();
        //            }
        //            else if (IsBlock && Rev.Length > 2 && Rev[2] == 0x3b)
        //            {
        //                partnumber = EnumPartnumber.MPM82504.ToString();
        //            }
        //            else
        //            {
        //                #region pn confirm with 0x9b
        //                ushort mfrReversion;
        //                DeviceObject.Devices.PMBus.ReadWord(i, 0x9b, out mfrReversion);
        //                if (mfrReversion == 0x3001)
        //                {
        //                    partnumber = EnumPartnumber.MPQ8645P.ToString();
        //                }
        //                else if (mfrReversion == 0x3101)
        //                {
        //                    partnumber = EnumPartnumber.MPM3695_10.ToString();
        //                }
        //                else if (mfrReversion == 0x3201)
        //                {
        //                    partnumber = EnumPartnumber.MPM3695_25.ToString();
        //                }

        //                else if (mfrReversion == 0x3301)
        //                {
        //                    partnumber = EnumPartnumber.mEZD81225A.ToString();
        //                }
        //                else if (mfrReversion == 0x3401)
        //                {
        //                    partnumber = EnumPartnumber.MPM3695A_10.ToString();
        //                }
        //                else if (mfrReversion == 0x3501)
        //                {
        //                    partnumber = EnumPartnumber.mEZDPD1620A.ToString();
        //                }
        //                else if (mfrReversion == 0x3601)
        //                {
        //                    partnumber = EnumPartnumber.MPM3695_100.ToString();
        //                }
        //                else if (mfrReversion == 0x3701)
        //                {
        //                    partnumber = EnumPartnumber.MP8796B.ToString();
        //                }
        //                else if (mfrReversion == 0x3801)
        //                {
        //                    partnumber = EnumPartnumber.mEZDPD1620AS.ToString();
        //                }
        //                else if (mfrReversion == 0x3901)
        //                {
        //                    partnumber = EnumPartnumber.MPM3695_7.ToString();
        //                }

        //                else
        //                {
        //                    byte id;
        //                    DeviceObject.Devices.PMBus.ReadByte(i, 0xdd, out id);
        //                    if (id == 0x06)
        //                    {
        //                        partnumber = EnumPartnumber.MP8855.ToString();
        //                    }
        //                    else if (id == 0x08)
        //                    {
        //                        partnumber = EnumPartnumber.MPQ8886.ToString();
        //                    }
        //                    else if (id == 0x09)
        //                    {
        //                        partnumber = EnumPartnumber.MPM3596.ToString();
        //                    }
        //                    else if (id == 0x0A)
        //                    {
        //                        partnumber = EnumPartnumber.mEZDPD4506AS.ToString();
        //                    }
        //                    else if (id == 0x0B)
        //                    {
        //                        partnumber = EnumPartnumber.MP8870.ToString();
        //                    }
        //                    else if (id == 0x0C)
        //                    {
        //                        partnumber = EnumPartnumber.MPQ8880.ToString();
        //                    }
        //                    else
        //                    {
        //                        DeviceObject.Devices.PMBus.ReadByte(i, 0xd6, out id);
        //                        if (id == 0x0D)
        //                        {
        //                            partnumber = EnumPartnumber.MPQ4272.ToString();
        //                        }
        //                        else
        //                        {
        //                            DeviceObject.Devices.PMBus.ReadByte(i, 0xf0, out id);
        //                            if (id == 0x0E)
        //                            {
        //                                partnumber = EnumPartnumber.mEZD81260A.ToString();
        //                            }
        //                        }

        //                    }
        //                }
        //                #endregion
        //            }


        //            partnumber = "MPQ8886";

        //            if (string.IsNullOrEmpty(partnumber))
        //                continue;

        //            if (ProConfig.Chips.Any(cp => cp.ChipName == partnumber))
        //                return;

        //            AddDeviceMethod( partnumber);
        //            deviceViewModel.ChangeAddressMethod(i);
        //            #endregion

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //    }
        //    finally
        //    {
        //        BusyIndicator.LoadingIndicator.LoadingUIStop();
        //    }


        //}

       /*  public void AutoScanSelectMethod()
         {
            var win = new Views.DeviceViews.AutoScanSelectWindow(license);
            var res = win.ShowDialog();
             if ((bool)res)
             {
                var view = win.DataContext as AutoScanSelectView;
                var chips = view.ChipList.Where(o => o.IsSelected == true);
                 if (chips.Count() > 0)
                 {
                     foreach (var item in chips)
                     {
                         deviceViewModel.AddDeviceMethod(item.ChipName + ' ' + item.ChipAddr, false);
                     }
                     deviceViewModel.ChangeChipSelectionMethod(ProConfig.Chips.LastOrDefault());
                 }
                 else   //not detected add manually
                 {
                     deviceViewModel.AddDeviceMethod();
                 }
             }
         }*/
 

        #endregion
    }
}
