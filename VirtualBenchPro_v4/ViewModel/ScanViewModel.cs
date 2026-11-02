using MPS.Common;
using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Common.Sqlite;
using MPS.Instrument;
using MPS.Instrument.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.Models;
using VirtualBenchPro_v4.Views.CustomElements;

namespace VirtualBenchPro_v4.ViewModel
{
    public class ScanViewModel : ObservableObject
    {
        private ProjectConfig _projConfig;
        private List<string> _filteredChips;
        private List<ScanDataModel> _scanDataColl;

        public event EventHandler<List<ScanDataModel>> AddSelectedDevicesToProjectEvent;

        public bool AtLeastOneAddressChecked
        {
            get { return (bool)_scanDataColl?.Any(s => s.IsChipEnabled); }
        }

        public List<ScanDataModel> ScanData
        {
            get { return _scanDataColl; }
            set
            {
                _scanDataColl = value;
                RaisePropertyChanged(nameof(ScanData));
                RaisePropertyChanged(nameof(AtLeastOneAddressChecked));
            }
        }

        #region Commands
        public ICommand CancelCommand => new RelayCommand<object>(Cancel, null);

        public void Cancel(object currentWindow)
        {
            try
            {
                if (currentWindow is Window)
                {
                    var wind = (Window)currentWindow;
                    wind.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ChangeChipSelectionMethod: " + ex.Message);
            }
        }

        public ICommand AddCommand => new RelayCommand<object>(Add, null);

        public void Add(object currentWindow)
        {
            try
            {
                AddSelectedDevicesToProjectEvent?.Invoke(this, _scanDataColl);

                if (currentWindow is Window)
                {
                    var wind = (Window)currentWindow;
                    wind.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ChangeChipSelectionMethod: " + ex.Message);
            }
        }

        #endregion

        public ScanViewModel(ProjectConfig projConifg, List<string> filteredChips)
        {
            _projConfig = projConifg;
            _filteredChips = filteredChips;
            _scanDataColl = new List<ScanDataModel>();
        }

        private Task<List<ScanDataModel>> ScanAddresses()
        {
            return Task.Run(() =>
            {
                var ChipsList = new List<ScanDataModel>();
                try
                {
                    for (byte i = 0; i < 0x80; i++)
                    {

                        var flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x1, false);

                        if (!flg)
                        {
                            flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x0, true);
                        }
                        if (!flg)
                        {
                            flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0xdb, true);
                        }
                        if (!flg)
                        {
                            flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x78, false);
                        }
                        if (flg)
                        {
                            if (!string.IsNullOrEmpty(ConfirmPartnumber(i)))
                            {
                                ChipsList.Add(new ScanDataModel { Address = "0x" + i.ToString("X2"), SelectedDevice = ConfirmPartnumber(i), IsAutoScan = true, IsSelected = true });
                            }
                            else
                            {
                                ChipsList.Add(new ScanDataModel { Address = "0x" + i.ToString("X2") });
                            }

                        }
                    }

                 /*   ChipsList.Add(new ScanDataModel { Address = "0x12" });
                    ChipsList.Add(new ScanDataModel { Address = "0x62", SelectedDevice = "MP8859", IsAutoScan = true, IsSelected = true });
                    ChipsList.Add(new ScanDataModel { Address = "0x22" });
                    ChipsList.Add(new ScanDataModel { Address = "0x40", SelectedDevice = "MP2421B", IsAutoScan = true, IsSelected = true });
                    ChipsList.Add(new ScanDataModel { Address = "0x32" });
                    ChipsList.Add(new ScanDataModel { Address = "0x38", SelectedDevice = "MP2718", IsAutoScan = true, IsSelected = true });*/

                }
                catch (Exception e)
                {

                }

                return ChipsList;
            });
        }

        public async Task ParseAddressDevicePair()
        {
            if (_projConfig != null && _filteredChips != null)
            {
                var sqlite = new CipherSqliteHelper();
                var scannedAddresses = await ScanAddresses();
                ScanData = await FindDevices(sqlite, scannedAddresses);
            }
        }

        private Task<List<ScanDataModel>> FindDevices(CipherSqliteHelper sqlite, List<ScanDataModel> ChipsList)
        {
            return Task.Run(() =>
            {
                var temp = new List<ScanDataModel>();

                ChipsList?.ForEach(ChipList =>
                {
                    var devices = new List<string>();

                    foreach (var chip in _filteredChips)
                    {
                        var newConfig = MPS.VBP.PartFactory.DataBaseHelper.LoadConfig(chip);

                        newConfig.Chips.ToList().ForEach(cp =>
                        {
                            var addressConfigeration = sqlite.GetHelpStringId(cp.KeyAddressConfigeration);
                            var addresLst = addressConfigeration?.Split(';');
                            if (addresLst != null && addresLst.Any(ad => ad.Contains(ChipList.Address)))
                            {
                                devices.Add(cp.ChipName);
                            }
                        });
                    }
                    devices.Sort();

                    ChipList.IsChipEnabled = IsAddressCurrentlyUsed(ChipList.Address) ? false : true;

                    if (ChipList.IsChipEnabled && ChipList.IsAutoScan)
                    {
                        ChipList.IsSelected = true;
                    }
                    else if (ChipList.IsChipEnabled && !ChipList.IsAutoScan)
                    {
                        ChipList.IsSelected = false;
                    }
                    else if (!ChipList.IsChipEnabled)
                    {
                        ChipList.IsSelected = true;
                    }

                    if (devices.Count > 0)
                    {
                        if (ChipList.IsAutoScan)
                        {
                            ChipList.SelectedDevice = devices.Any(device => device.Contains(ChipList.SelectedDevice)) ? ChipList.SelectedDevice : devices.FirstOrDefault();
                            temp.Add(new ScanDataModel { Address = ChipList.Address, SelectedDevice = ChipList.SelectedDevice, IsSelected = ChipList.IsSelected, IsAutoScan = ChipList.IsAutoScan, IsChipEnabled = ChipList.IsChipEnabled, Devices = devices });
                        }
                        else
                        {
                            temp.Add(new ScanDataModel { Address = ChipList.Address, SelectedDevice = devices.FirstOrDefault(), IsSelected = ChipList.IsSelected, IsAutoScan = ChipList.IsAutoScan, IsChipEnabled = ChipList.IsChipEnabled, Devices = devices });
                        }

                    }

                });
                temp?.Sort((x, y) => x.Address.CompareTo(y.Address));
                return temp;
            });
        }

        private bool IsAddressCurrentlyUsed(string address)
        {
            return _projConfig != null && _projConfig.Chips.Any(chip => chip.ChipAddr.Contains(address));
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
            if (id4 == 0x53)
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

        #region Useless
        /*  private bool IsSPIAddress()
          {
              ushort data = 0;
              DeviceObject.Devices.PMBus.SetSPIFrequency(0x0B, EnumUPDOWNType.DOWN, EnumCSType.LOW);
              DeviceObject.Devices.PMBus.SPIWriteReadPEC(0x00, (byte)((0x01 << 1) + 1), ref data, true);
              if (data == 0 || data == 0xFFFF)
              {
                  DeviceObject.Devices.PMBus.SPIWriteReadPEC(0x00, (byte)((0x1B << 1) + 1), ref data, true);
                  if (data == 0 || data == 0xFFFF)
                  {
                      return false;
                  }
                  else
                  {
                      return true;
                  }
              }
              else
              {
                  return true;
              }
          }*/
        #endregion


    }
}
