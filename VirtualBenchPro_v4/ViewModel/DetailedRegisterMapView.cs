using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Common.Sqlite;
using MPS.Instrument;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;

namespace VirtualBenchPro_v4.ViewModel
{
    public class DetailedRegisterMapView : ObservableObject
    {
        public DetailedRegisterMapView(Chip _currentChip, ObservableCollection<LogModel> _logData, WinDetailedRegisterMap _win)
        {
            var _currentRegisterMap = new List<Register>();
            foreach (var region in _currentChip.Regions.Where(o => o.RegionName != "Overview"))
            {
                foreach (var mode in region.ConfigurationModes)
                {
                    foreach (var page in mode.Pages)
                    {
                        foreach (var reg in page.AllRegisters)
                        {
                            if (!_currentRegisterMap.Contains(reg))
                            {
                                _currentRegisterMap.Add(reg);
                            }
                        }
                    }
                }
            }
            CurrentRegisterMap = new ObservableCollection<Register>();
            foreach (var reg in _currentRegisterMap.OrderBy(o => o.Address + (o.Address_H << 8)))
            {
                if (reg.Disabled)
                {
                    continue;
                }
                CurrentRegisterMap.Add(reg);
            }
            CurrentBitControlMap = new ObservableCollection<BitControl>();
            CurrentChip = _currentChip;
            ChipAddr = Convert.ToByte(CurrentChip.ChipAddr, 16);
            SelectedRegister = CurrentRegisterMap.FirstOrDefault();
            LogData = _logData;
            win = _win;
            if (sqlite == null)
            {
                sqlite = new CipherSqliteHelper();
            }
            Maximize_Restore_Icon = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/max@3x.png"));

        }

        #region Properties & Variables
        private ImageSource minimize_restore_icon;
        public ImageSource Maximize_Restore_Icon
        {
            get { return minimize_restore_icon; }
            set
            {
                minimize_restore_icon = value;
                RaisePropertyChanged("Maximize_Restore_Icon");
            }
        }

        private static CipherSqliteHelper sqlite;
        private byte ChipAddr;
        private WinDetailedRegisterMap win;


        private ObservableCollection<Register> _currentRegisterMap;
        public ObservableCollection<Register> CurrentRegisterMap
        {
            get { return _currentRegisterMap; }
            set
            {
                _currentRegisterMap = value;
                RaisePropertyChanged("CurrentRegisterMap");
            }
        }

        private Register _selectedRegister;
        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set
            {
                if (value != null)
                {
                    if (_selectedRegister != value)
                    {
                        _selectedRegister = value;
                        string addr_str;
                        if (_selectedRegister.Address_H > 0)
                        {
                            addr_str = ((_selectedRegister.Address_H << 8) + _selectedRegister.Address).ToString("X3");
                        }
                        else
                        {
                            addr_str = _selectedRegister.Address.ToString("X2");
                        }
                        RegisterTitle = string.Format("Page:{0} | {1} ({2}h)", _selectedRegister.Page, _selectedRegister.Name, addr_str);

                        if (!string.IsNullOrEmpty(_selectedRegister.ImageKey))
                        {
                            var img = sqlite.GetImagebyId(_selectedRegister.ImageKey);
                            Diagram = Views.CustomElements.Help_GroupBox.Bytes2BitmapImage(img);
                        }
                        else
                        {
                            Diagram = null;
                        }

                        CurrentBitControlMap.Clear();
                        foreach (var bt in SelectedRegister.BitControls)
                        {
                            bt.CurrentRawBitValue = bt.SourceRawBitValue;
                            if (CurrentBitControlMap.All(o => o.Position != bt.Position))
                            {
                                CurrentBitControlMap.Add(bt);
                            }
                        }
                    }
                }
                else
                {
                    CurrentBitControlMap.Clear();
                }

                RaisePropertyChanged("SelectedRegister");
            }
        }

        private ObservableCollection<BitControl> _currentBitControlMap;
        public ObservableCollection<BitControl> CurrentBitControlMap
        {
            get { return _currentBitControlMap; }
            set
            {
                _currentBitControlMap = value;
                RaisePropertyChanged("CurrentBitControlMap");
            }
        }

        private BitControl _selectedBitControl;
        public BitControl SelectedBitControl
        {
            get { return _selectedBitControl; }
            set
            {
                _selectedBitControl = value;
                RaisePropertyChanged("SelectedBitControl");
            }
        }

        private string _registerTitle;
        public string RegisterTitle
        {
            get { return _registerTitle; }
            set
            {
                _registerTitle = value;
                RaisePropertyChanged("RegisterTitle");
            }
        }

        private BitmapImage _diagram;
        public BitmapImage Diagram
        {
            get { return _diagram; }
            set
            {
                _diagram = value;
                RaisePropertyChanged("Diagram");
            }
        }

        private Chip _currentChip;
        public Chip CurrentChip
        {
            get { return _currentChip; }
            set
            {
                _currentChip = value;
                RaisePropertyChanged("CurrentChip");
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

        private ObservableCollection<LogModel> _logdata;
        public ObservableCollection<LogModel> LogData
        {
            get { return _logdata; }
            set
            {
                _logdata = value;
                RaisePropertyChanged("LogData");
            }
        }

        private string lastSearchText;
        private int selectedIndex;
        private List<Register> searchRegisters;

        Stopwatch sw = new Stopwatch();
        #endregion

        #region Commands & Methods

        public bool SearchRegisterMethod(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                searchRegisters.Clear();
                selectedIndex = 0;
                return false;
            }

            if (searchText != lastSearchText)  //first time to search
            {
                lastSearchText = searchText;
                searchRegisters = CurrentRegisterMap.Where(o => o.Name.ToLower().Contains(searchText.ToLower()) || (o.Address_H.ToString("x2").TrimStart('0') + o.Address.ToString("x2")).Contains(searchText.ToLower())).ToList();
                if (searchRegisters != null && searchRegisters.Count > 0)
                {
                    selectedIndex = 0;
                    SelectedRegister = searchRegisters[selectedIndex];
                    return true;
                }
                else
                {
                    selectedIndex = 0;
                    searchRegisters.Clear();
                    return false;
                }
            }
            else  //move to next
            {
                if (searchRegisters != null && searchRegisters.Count > 0)
                {
                    if (selectedIndex + 1 >= searchRegisters.Count)
                    {
                        selectedIndex = 0;
                    }
                    else
                    {
                        selectedIndex++;
                    }

                    SelectedRegister = searchRegisters[selectedIndex];
                    return true;
                }
                else
                {
                    selectedIndex = 0;
                    searchRegisters.Clear();
                    return false;
                }
            }

        }

        public ICommand ReadRegisterCommand => new RelayCommand(ReadRegisterMethod);
        private void ReadRegisterMethod()
        {
            bool isEnter = false;
            try
            {

                if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
                {
                    if (SelectedRegister == null || SelectedRegister.isVirtual || SelectedRegister.BitControls.Count() == 0)
                        return;

                    sw.Restart();
                    while (!DeviceObject.Devices.PMBus.SetStatus(true, "showRegisterMapDetailMethod"))
                    {
                        if (sw.ElapsedMilliseconds >= 5000)   //5s
                        {
                            sw.Stop();
                            throw new Exception("This operation has timed out. Please try again.");
                        }
                        //System.Threading.Thread.Sleep(100);
                    }
                    sw.Stop();

                    isEnter = true;
                    string regValue;
                    if (SelectedRegister.DataType == EnumCommandType.Word.ToString())
                    {
                        ushort wordValue;
                        DeviceObject.Devices.PMBus.ReadWord(ChipAddr, SelectedRegister.Address, out wordValue, false);
                        SelectedRegister.Value = wordValue;
                        regValue = wordValue.ToString("X4");
                    }
                    else
                    {
                        byte byteValue;
                        DeviceObject.Devices.PMBus.ReadByte(ChipAddr, SelectedRegister.Address, out byteValue, false);
                        SelectedRegister.Value = byteValue;
                        regValue = byteValue.ToString("X2");
                    }

                    foreach (BitControl bt in SelectedRegister.BitControls)
                    {
                        ReadBitcontrol(SelectedRegister, bt);
                    }

                    LogData.Add(new LogModel()
                    {
                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        RegisterName = "Read: " + SelectedRegister.Name,
                        RegisterAddress = SelectedRegister.Address.ToString("X2"),
                        RegisterValue = regValue
                    });

                    var ui = new WinDone("Read register done", 1000, 90, 210);
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                else
                {
                    throw new Exception("Device not found");

                }
            }
            catch (Exception ex)
            {
                UIEnabled = false;
                if (ex.Message == "Device not found")
                {
                    var ui = new WinDeviceDisconnected();
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                else
                {
                    var ui = new WinError(ex.Message);
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                UIEnabled = true;
            }
            finally
            {
                if (isEnter)
                {
                    DeviceObject.Devices.PMBus.SetStatus(false, "showRegisterMapDetailMethod");
                }
            }
        }
        private void ReadBitcontrol(Register reg, BitControl bit)
        {
            //MinMaxType
            if (string.IsNullOrEmpty(bit.PossibleValues))
            {
                StringBuilder strb = new StringBuilder();
                for (int i = bit.Position + bit.BitLength - 1; i >= bit.Position; i--)
                {
                    strb.Append((reg.Value >> i) & 1);
                }

                if (bit.Access == "R" && bit.BitName.ToUpper() == "RESERVED")
                {
                    bit.CurrentRawBitValue = bit.SourceRawBitValue = strb.ToString();
                }
                else
                {
                    int hex_digits = (int)Math.Ceiling(bit.BitLength / 4.0);
                    var rawval = Convert.ToUInt32(strb.ToString(), 2).ToString("X" + hex_digits);
                    bit.CurrentRawBitValue = bit.SourceRawBitValue = rawval;
                }
            }
            //PossibleValuesType
            else
            {
                StringBuilder strb = new StringBuilder();
                for (int i = bit.Position + bit.BitLength - 1; i >= bit.Position; i--)
                {
                    strb.Append((reg.Value >> i) & 1);
                }
                var pvs = bit.PossibleValues.Split(new char[] { ';' }).Select(o => o.Trim());

                foreach (var pv in pvs)
                {
                    var temps = pv.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temps.Length < 2)
                        continue;
                    if (temps.Any(o => o == string.Format("0b{0}", strb.ToString())))
                    {
                        bit.CurrentRawBitValue = bit.SourceRawBitValue = pv;
                        break;
                    }
                    else if (temps[1].StartsWith("0x"))
                    {
                        if (reg.DataType == EnumCommandType.Byte.ToString())
                        {
                            if (Convert.ToByte(temps[1], 16) == ((reg.Value >> bit.Position) & ((uint)Math.Pow(2, bit.BitLength) - 1)))
                            {
                                bit.CurrentRawBitValue = bit.SourceRawBitValue = pv;
                                break;
                            }
                        }
                        else
                        {
                            StringBuilder strb1 = new StringBuilder();
                            for (int i = bit.Position + bit.BitLength - 1; i >= bit.Position; i--)
                            {
                                strb1.Append((reg.Value >> i) & 1);
                            }
                            if (Convert.ToInt16(temps[1], 16) == Convert.ToInt16(strb1.ToString(), 2))
                            {
                                bit.CurrentRawBitValue = bit.SourceRawBitValue = pv;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public ICommand WriteRegisterCommand => new RelayCommand(WriteRegisterMethod);
        private void WriteRegisterMethod()
        {
            bool isEnter = false;
            try
            {
                if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
                {
                    if (SelectedRegister == null || SelectedRegister.isVirtual || SelectedRegister.BitControls.Count() == 0)
                        return;

                    sw.Restart();
                    while (!DeviceObject.Devices.PMBus.SetStatus(true, "showRegisterMapDetailMethod"))
                    {
                        if (sw.ElapsedMilliseconds >= 5000)   //5s
                        {
                            sw.Stop();
                            throw new Exception("This operation has timed out. Please try again.");
                        }
                        //System.Threading.Thread.Sleep(100);
                    }
                    sw.Stop();

                    isEnter = true;
                    bool isVirtual = CurrentChip.ChipAddr.Contains("v");

                    uint selectedValue = SelectedRegister.Value;
                    string temp;

                    foreach (var bt in CurrentBitControlMap)
                    {
                        if (!string.IsNullOrEmpty(bt.RawBitError))
                        {
                            throw new Exception(bt.RawBitError);
                        }

                        temp = Convert.ToString(selectedValue, 2);
                        if (SelectedRegister.DataType == EnumCommandType.Byte.ToString())
                        {
                            temp = temp.PadLeft(8, '0');
                        }
                        else if (SelectedRegister.DataType == EnumCommandType.Word.ToString())
                        {
                            temp = temp.PadLeft(16, '0');
                        }
                        else
                        {
                            throw new Exception($"Invalid register datatype: {SelectedRegister.DataType.ToString()}");
                        }

                        string binstr = "";
                        //MinMaxType
                        if (string.IsNullOrEmpty(bt.PossibleValues))
                        {
                            if (bt.Access == "R" && bt.BitName.ToUpper() == "RESERVED")
                            {
                                binstr = bt.CurrentRawBitValue.TrimStart('0').PadLeft(bt.BitLength, '0');
                                bt.CurrentRawBitValue = bt.SourceRawBitValue = binstr;
                            }
                            else
                            {
                                var val = Convert.ToUInt32(bt.CurrentRawBitValue, 16);
                                binstr = Convert.ToString(val, 2).PadLeft(bt.BitLength, '0');
                                bt.SourceRawBitValue = bt.CurrentRawBitValue;
                                selectedValue = Convert.ToUInt16(temp.Remove(temp.Count() - bt.Position - bt.BitLength, bt.BitLength).Insert(temp.Count() - bt.Position - bt.BitLength, binstr), 2);
                            }
                        }
                        //PossibleValueType
                        else
                        {
                            var btValue = bt.CurrentRawBitValue.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                            if (btValue.Length < 2)
                                continue;

                            if (btValue[1].StartsWith("0x"))
                            {
                                binstr = Convert.ToString(Convert.ToUInt16(btValue[1], 16), 2).TrimStart('0').PadLeft(bt.BitLength, '0');
                            }
                            else if (btValue[1].StartsWith("0b"))
                            {
                                binstr = btValue[1].Remove(0, 2);
                            }
                            bt.SourceRawBitValue = bt.CurrentRawBitValue;

                            selectedValue = Convert.ToUInt16(temp.Remove(temp.Count() - bt.Position - bt.BitLength, bt.BitLength).Insert(temp.Count() - bt.Position - bt.BitLength, binstr), 2);
                        }

                    }

                    string regValue;
                    if (SelectedRegister.DataType == EnumCommandType.Byte.ToString())
                    {
                        if (!isVirtual) DeviceObject.Devices.PMBus.WriteByte(ChipAddr, SelectedRegister.Address, (byte)selectedValue);
                        regValue = selectedValue.ToString("X2");
                    }
                    else
                    {
                        if (!isVirtual) DeviceObject.Devices.PMBus.WriteWord(ChipAddr, SelectedRegister.Address, (ushort)selectedValue);
                        regValue = selectedValue.ToString("X4");
                    }

                    SelectedRegister.Value = selectedValue;

                    LogData.Add(new LogModel()
                    {
                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        RegisterName = "Write: " + SelectedRegister.Name,
                        RegisterAddress = SelectedRegister.Address.ToString("X2"),
                        RegisterValue = regValue
                    });

                    var ui = new WinDone("Write register done", 1000, 90, 210);
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                else
                {
                    throw new Exception("Device not found");

                }
            }
            catch (Exception ex)
            {
                UIEnabled = false;
                if (ex.Message == "Device not found")
                {
                    var ui = new WinDeviceDisconnected();
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                else
                {
                    var ui = new WinError(ex.Message);
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                UIEnabled = true;
            }
            finally
            {
                if (isEnter)
                {
                    DeviceObject.Devices.PMBus.SetStatus(false, "showRegisterMapDetailMethod");
                }
            }
        }

        public ICommand SendByteCommand => new RelayCommand(SendByteMethod);
        private void SendByteMethod()
        {
            bool isEnter = false;
            try
            {
                if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
                {
                    if (SelectedRegister == null || SelectedRegister.isVirtual || SelectedBitControl == null)
                        return;

                    //while (!DeviceObject.Devices.PMBus.SetStatus(true, "showRegisterMapDetailMethod"))
                    //{
                    //}

                    sw.Restart();
                    while (!DeviceObject.Devices.PMBus.SetStatus(true, "showRegisterMapDetailMethod"))
                    {
                        if (sw.ElapsedMilliseconds >= 5000)   //5s
                        {
                            sw.Stop();
                            throw new Exception("This operation has timed out. Please try again.");
                        }
                        //System.Threading.Thread.Sleep(100);
                    }
                    sw.Stop();

                    isEnter = true;
                    DeviceObject.Devices.PMBus.SendCommand(ChipAddr, SelectedRegister.Address);

                    LogData.Add(new LogModel()
                    {
                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        RegisterName = "Send Byte: " + SelectedRegister.Name,
                        RegisterAddress = SelectedRegister.Address.ToString("X2"),
                        RegisterValue = "-"
                    });

                    var ui = new WinDone("Send register done", 1000, 90, 210);
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                else
                {
                    throw new Exception("Device not found");

                }
            }
            catch (Exception ex)
            {
                UIEnabled = false;
                if (ex.Message == "Device not found")
                {
                    var ui = new WinDeviceDisconnected();
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                else
                {
                    var ui = new WinError(ex.Message);
                    ui.Owner = win;
                    ui.ShowDialog();
                }
                UIEnabled = true;
            }
            finally
            {
                if (isEnter)
                {
                    DeviceObject.Devices.PMBus.SetStatus(false, "showRegisterMapDetailMethod");
                }
            }
        }

        #endregion
    }

}
