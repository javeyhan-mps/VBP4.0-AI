using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Instrument;
using MPS.Instrument.Enum;
using MPS.VBP.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VirtualBenchPro_v4.Views.CustomElements;

namespace VirtualBenchPro_v4.ViewModel
{
    class RegisterToolView : ObservableObject
    {
        private string _strOperation = "Byte";
        public string strOperation
        {
            get { return _strOperation; }
            set
            {
                this._strOperation = value;
                //if (strOperation != "Byte" && strOperation != "Word")
                //{
                //    this._strOperation = this._selectIndexProcotol.ToString();
                //}
                RaisePropertyChanged("strOperation");
            }
        }

        private bool _EnablePEC;
        public bool EnablePEC
        {
            get { return _EnablePEC; }
            set
            {
                this._EnablePEC = value;
                RaisePropertyChanged("EnablePEC");
            }
        }
        
        private bool _is2ByteAddress;
        public bool is2ByteAddress
        {
            get { return _is2ByteAddress; }
            set
            {
                this._is2ByteAddress = value;
                RaisePropertyChanged("is2ByteAddress");
            }
        }
        private string _slaveAddress;
        public string SlaveAddress
        {
            get
            {
                return this._slaveAddress;
            }

            set
            {
                this._slaveAddress = value;
                RaisePropertyChanged("SlaveAddress");
            }
        }

        private string _registerAddress;
        public string RegisterAddress
        {
            get
            {
                return this._registerAddress;
            }

            set
            {
                this._registerAddress = value;
                RaisePropertyChanged("RegisterAddress");
            }
        }

        private string _byteLength;
        public string ByteLength
        {
            get
            {
                return this._byteLength;
            }

            set
            {
                this._byteLength = value;
                RaisePropertyChanged("ByteLength");
            }
        }

        private string _data;
        public string Data
        {
            get
            {
                return this._data;
            }

            set
            {
                this._data = value;
                RaisePropertyChanged("Data");
            }
        }

        private string _Freqdata = "400";
        public string Freqdata
        {
            get
            {
                return this._Freqdata;
            }

            set
            {
                this._Freqdata = value;
                RaisePropertyChanged("Freqdata");
            }
        }

        private int _selectIndexIICClock = 2;
        public int SelectIndexIICClock
        {
            get
            {
                return this._selectIndexIICClock;
            }

            set
            {
                this._selectIndexIICClock = value;
                RaisePropertyChanged("SelectIndexIICClock");
            }
        }

        private int _selectIndexSPIClock = 0;
        public int SelectIndexSPIClock
        {
            get
            {
                return this._selectIndexSPIClock;
            }

            set
            {
                this._selectIndexSPIClock = value;
                RaisePropertyChanged("SelectIndexSPIClock");
            }
        }

        private int _selectIndexProcotol = 0;
        public int SelectIndexProcotol
        {
            get
            {
                return this._selectIndexProcotol;
            }

            set
            {
                this._selectIndexProcotol = value;
                RaisePropertyChanged("SelectIndexProcotol");
                this._strOperation = this._selectIndexProcotol.ToString();

                SpecialProtocolControlVisibiliyUpdate(true, value);
            }
        }

        private int _selectIndexMode = 0;
        public int SelectIndexMode
        {
            get
            {
                return this._selectIndexMode;
            }

            set
            {
                this._selectIndexMode = value;
                RaisePropertyChanged("SelectIndexMode");
            }
        }

        private bool _isSpecialProtocol = false;
        public bool IsSpecialProtocol
        {
            get { return _isSpecialProtocol; }
            set
            {
                this._isSpecialProtocol = value;
                RaisePropertyChanged("IsSpecialProtocol");
            }
        }

        #region Visibility
        private Visibility _slaveVisibility;
        public Visibility SlaveVisibility
        {
            get { return _slaveVisibility; } 
            set
            {
                _slaveVisibility = value;
                RaisePropertyChanged(nameof(SlaveVisibility));
            }
        }

        private Visibility _clockVisibility;
        public Visibility ClockVisibility
        {
            get { return _clockVisibility; }
            set
            {
                _clockVisibility = value;
                RaisePropertyChanged(nameof(ClockVisibility));
            }
        }

        private Visibility _setSPIVisibility;
        public Visibility SetSPIVisibility
        {
            get { return _setSPIVisibility; }
            set
            {
                _setSPIVisibility = value;
                RaisePropertyChanged(nameof(SetSPIVisibility));
            }
        }

        private Visibility _setUARTVisibility;
        public Visibility SetUARTVisibility
        {
            get { return _setUARTVisibility; }
            set
            {
                _setUARTVisibility = value;
                RaisePropertyChanged(nameof(SetUARTVisibility));
            }
        }

        private Visibility _preEnablePECLineVisibility;
        public Visibility PreEnablePECLineVisibility
        {
            get { return _preEnablePECLineVisibility; }
            set
            {
                _preEnablePECLineVisibility = value;
                RaisePropertyChanged(nameof(PreEnablePECLineVisibility));
            }
        }

        private Visibility _enablePECVisibility;
        public Visibility EnablePECVisibility
        {
            get { return _enablePECVisibility; }
            set
            {
                _enablePECVisibility = value;
                RaisePropertyChanged(nameof(EnablePECVisibility));
            }
        }

        private Visibility _twoByteAddressVisibility;
        public Visibility TwoByteAddressVisibility
        {
            get { return _twoByteAddressVisibility; }
            set
            {
                _twoByteAddressVisibility = value;
                RaisePropertyChanged(nameof(TwoByteAddressVisibility));
            }
        }
        #endregion

        byte[] WriteSendDate = new byte[4];
        byte[] ReadSendDate = new byte[2];
        byte[] Outdata = new byte[4];

        public List<int> IICFreq = new List<int>() {  100, 200, 400, 500, 1000};
        //public List<int> SPIFreq = new List<int>() { 50, 100, 200, 400, 500, 1000, 2000, 4000 };

        private MainWindow main_window;
        public RegisterToolView(MainWindow mw)
        {
            main_window = mw;
        }

        private void SpecialProtocolControlVisibiliyUpdate(bool isSpecialProtocol, int specialProtocolIndex = 0)
        {
            if (isSpecialProtocol)
            {
                if (specialProtocolIndex == 10) //AMBS SPI
                {
                    SlaveVisibility = Visibility.Collapsed;
                    ClockVisibility = Visibility.Collapsed;
                    SetSPIVisibility = Visibility.Collapsed;
                    SetUARTVisibility = Visibility.Collapsed;
                    PreEnablePECLineVisibility = Visibility.Collapsed;
                    EnablePECVisibility = Visibility.Collapsed;
                    TwoByteAddressVisibility = Visibility.Collapsed;

                    try
                    {
                        Convert.ToByte(SlaveAddress, 16);
                    }
                    catch
                    {
                        SlaveAddress = null;  // When null, Convert.ToByte will return 0 but not throw exception
                    }
                }
                else
                {
                    SlaveVisibility = Visibility.Visible;
                    ClockVisibility = Visibility.Visible;
                    SetSPIVisibility = Visibility.Visible;
                    SetUARTVisibility = Visibility.Visible;
                    PreEnablePECLineVisibility = Visibility.Visible;
                    EnablePECVisibility = Visibility.Visible;
                    TwoByteAddressVisibility = Visibility.Visible;
                }
            }
            else
            {
                SlaveVisibility = Visibility.Visible;

                PreEnablePECLineVisibility = Visibility.Visible;
                EnablePECVisibility = Visibility.Visible;
                TwoByteAddressVisibility = Visibility.Visible;
            }
        }


        #region 之前的布局事件
        //public ICommand WriteCommand => new RelayCommand(BtnWriteMethod, null);

        //private void BtnWriteMethod()
        //{
        //    try
        //    {
        //        if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
        //        {
        //            main_window.UIEnabled = false;
        //            var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
        //            ui.ShowDialog();
        //            main_window.UIEnabled = true;
        //            return;

        //        }
        //        String[] strSplit = Data.Split(' ');
        //        DeviceObject.Devices.PMBus.WriteByte(Convert.ToByte(SlaveAddress, 16), Convert.ToByte(RegisterAddress, 16), Convert.ToByte(strSplit[0], 16), EnablePEC);


        //    }
        //    catch (Exception ex)
        //    {
        //        main_window.UIEnabled = false;
        //        var ui = new WinError(ex.Message);
        //        ui.ShowDialog();
        //        main_window.UIEnabled = true;
        //    }
        //}

        //public ICommand ReadCommand => new RelayCommand(BtnReadMethod, null);

        //private void BtnReadMethod()
        //{
        //    try
        //    {
        //        if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
        //        {
        //            main_window.UIEnabled = false;
        //            var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
        //            ui.ShowDialog();
        //            main_window.UIEnabled = true;
        //            return;

        //        }
        //        Data = "";

        //        byte dat;
        //        DeviceObject.Devices.PMBus.ReadByte(Convert.ToByte(SlaveAddress, 16), Convert.ToByte(RegisterAddress, 16), out dat, EnablePEC);
        //        Data += Convert.ToString(dat, 16);

        //    }
        //    catch (Exception ex)
        //    {
        //        main_window.UIEnabled = false;
        //        var ui = new WinError(ex.Message);
        //        ui.ShowDialog();
        //        main_window.UIEnabled = true;
        //    }
        //}


        //暂时放开两个按钮点击事件， 防止出现绑定错误
        //如果不需要， 可以删除view中之前的布局。
        public ICommand ReadWordCommand => new RelayCommand(BtnReadWordMethod, null);

        private void BtnReadWordMethod()
        {
            try
            {
                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                    return;

                }
                Data = "";
                ushort dat;
                DeviceObject.Devices.PMBus.ReadWord(Convert.ToByte(SlaveAddress, 16), Convert.ToByte(RegisterAddress, 16), out dat, EnablePEC);
                Data += Convert.ToString(dat, 16);
            }
            catch (Exception ex)
            {
                main_window.UIEnabled = false;
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }


        public ICommand WriteWordCommand => new RelayCommand(BtnWriteWordMethod, null);

        private void BtnWriteWordMethod()
        {
            try
            {
                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                    return;
                }
                var strSplit = Data.Split(' ');
                var data = Convert.ToUInt16(strSplit[0], 16);
                DeviceObject.Devices.PMBus.WriteWord(Convert.ToByte(SlaveAddress, 16), Convert.ToByte(RegisterAddress, 16), data, EnablePEC);


            }
            catch (Exception ex)
            {
                main_window.UIEnabled = false;
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }
        #endregion

        public ICommand SelectModeCommand => new RelayCommand<object>(BtnSelecctModeMethod, null);

        private void BtnSelecctModeMethod(object obj)
        {
            string strBtn = obj as string;
            strOperation = strBtn;
            switch (strOperation)
            {
                case "Byte":
                case "Word":
                    EnablePEC = false;
                    IsSpecialProtocol = false;
                    SpecialProtocolControlVisibiliyUpdate(false);
                    break;
                default:
                    SelectIndexProcotol = 0;
                    EnablePEC = true;
                    IsSpecialProtocol = true;
                    break;
            }
        }


        public ICommand WriteCommand => new RelayCommand(BtnWriteMethod, null);

        private void BtnWriteMethod()
        {
            try
            {
                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                    return;
                }
                String[] strSplit = Data.Split(' ');
                var data = Convert.ToUInt16(strSplit[0], 16);
                ushort Reg2ByteAddr = 0;
                WriteSendDate[0] = (byte)((Convert.ToByte(SlaveAddress, 16) << 4) + 1);
                if(is2ByteAddress)
                    Reg2ByteAddr = Convert.ToUInt16(RegisterAddress, 16);
                else
                    WriteSendDate[1] = Convert.ToByte(RegisterAddress, 16);
                WriteSendDate[2] = (byte)(data & 0xFF);
                WriteSendDate[3] = (byte)((data >> 8) & 0xFF);
                switch (strOperation)
                {
                    case "Byte":
                        if (is2ByteAddress)
                        {
                            byte addr_H = (byte)((Reg2ByteAddr >> 8) & 0xFF);
                            byte addr_L = (byte)(Reg2ByteAddr & 0x00FF);
                            byte[] buffer = new byte[2] { addr_H, WriteSendDate[2] };
                            DeviceObject.Devices.PMBus.WriteSequence(Convert.ToByte(SlaveAddress, 16), addr_L, buffer);
                        }
                        else
                        {
                            DeviceObject.Devices.PMBus.WriteByte(Convert.ToByte(SlaveAddress, 16), WriteSendDate[1], WriteSendDate[2], EnablePEC);
                        }

                        break;
                    case "Word":
                        if (is2ByteAddress)
                        {
                            byte addr_H = (byte)((Reg2ByteAddr >> 8) & 0xFF);
                            byte addr_L = (byte)(Reg2ByteAddr & 0x00FF);
                            byte[] buffer = new byte[3] { addr_H, WriteSendDate[2], WriteSendDate[3] };
                            DeviceObject.Devices.PMBus.WriteSequence(Convert.ToByte(SlaveAddress, 16), addr_L, buffer);
                        }
                        else
                        {
                            DeviceObject.Devices.PMBus.WriteWord(Convert.ToByte(SlaveAddress, 16), WriteSendDate[1], data, EnablePEC);
                        }
                        break;
                    case "0":
                    case "Other":
                        DeviceObject.Devices.PMBus.SPIWriteReadPEC(Convert.ToByte(SlaveAddress, 16), (byte)(WriteSendDate[1] << 1), ref data, EnablePEC);
                        break;
                    case "1":
                        DeviceObject.Devices.PMBus.SPIWriteReadPEC(Convert.ToByte(SlaveAddress, 16), (byte)(WriteSendDate[1] << 2), ref data, EnablePEC);
                        break;
                    case "2":
                        DeviceObject.Devices.PMBus.WriteReadNBytes(WriteSendDate, ref Outdata, EnablePEC);
                        break;
                    case "3":
                        ushort Tempdata = (ushort)(((WriteSendDate[1] << 9) & 0xFE00) + (0 << 8) + data);
                        //ushort Tempdata = (ushort)((WriteSendDate[1] << 8) +data);
                        DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)(WriteSendDate[1] << 1), Tempdata, EnablePEC);
                        break;
                    case "4":
                        DeviceObject.Devices.PMBus.WriteReadNBytes(WriteSendDate, ref Outdata, EnablePEC, 2);
                        break;
                    case "5":
                        DeviceObject.Devices.PMBus.WriteWordCommand(Convert.ToByte(SlaveAddress, 16), WriteSendDate[1], data, EnablePEC, true);
                        break;
                    case "6":
                        DeviceObject.Devices.PMBus.WriteWordCommand(Convert.ToByte(SlaveAddress, 16), WriteSendDate[1], data, EnablePEC, false);
                        break;
                    case "7":
                        WriteByte(WriteSendDate[1], data);
                        break;
                    case "8":
                        var dataList = new byte[] { WriteSendDate[2] };
                        DeviceObject.Devices.PMBus.WriteReadNBytes_MPF61000(0, Convert.ToByte(SlaveAddress, 16), WriteSendDate[1], dataList, EnablePEC, 1);
                        break;
                    case "9":
                        byte[] SendDate = new byte[3];
                        SendDate[0] = (byte)((1 << 7) + Convert.ToByte(SlaveAddress, 16));
                        SendDate[1] = WriteSendDate[1];
                        SendDate[2] = WriteSendDate[2];
                        DeviceObject.Devices.PMBus.WriteReadNBytes(SendDate, ref Outdata, EnablePEC, 3);
                        break;
                    case "10":
                        DeviceObject.Devices.PMBus.ReadResponse(0x00, 0x00); // Without this page 0, 0x7F might failed to opereate
                        DeviceObject.Devices.PMBus.WriteWord_MPQ78000FS(WriteSendDate[1], data);
                        break;
                    case "11":
                        byte[] bytes = StringToHexByte(Data);
                        DeviceObject.Devices.PMBus.SpiWriteSequence(Convert.ToByte(SlaveAddress, 16), WriteSendDate[1], bytes);
                        break;
                    case "12":
                        var PAR = GetParMethod((1 << 15) + ((WriteSendDate[1] & 0x3F) << 8) + WriteSendDate[2]);
                        var ActAdress = (1 << 7) + (PAR << 6) + (WriteSendDate[1] & 0x3F);
                        DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)ActAdress, (ushort)((ActAdress << 8) + WriteSendDate[2]));
                        break;
                    case "13":
                        ushort uselessdata = 0;
                        DeviceObject.Devices.PMBus.SPI_MP5499(WriteSendDate[0], WriteSendDate[1], data, out uselessdata, 0);
                        break;
                    case "14":
                        PAR = GetParMethod((1 << 15) + ((WriteSendDate[1] & 0x3F) << 8) + WriteSendDate[2], 1);
                        var Address = (1 << 7) + (PAR << 6) + (WriteSendDate[1] & 0x3F);
                        DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)Address, (ushort)((Address << 8) + WriteSendDate[2]));
                        break;
                }
            }
            catch (Exception ex)
            {
                main_window.UIEnabled = false;
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }
        public ICommand ReadCommand => new RelayCommand(BtnReadMethod, null);

        private void BtnReadMethod()
        {
            try
            {
                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                    return;
                }
                Data = "";
                int PAR = 0;
                byte data;
                ushort data1 = 0;
                ushort Reg2ByteAddr = 0;
                ReadSendDate[0] = (byte)((Convert.ToByte(SlaveAddress, 16) << 4) + 5);
                if(is2ByteAddress)
                    Reg2ByteAddr = Convert.ToUInt16(RegisterAddress, 16);
                else
                    ReadSendDate[1] = Convert.ToByte(RegisterAddress, 16);
                switch (strOperation)
                {
                    case "Byte":
                        if (is2ByteAddress)
                        {
                            byte[] buffer = new byte[1];
                            byte addr_H = (byte)((Reg2ByteAddr >> 8) & 0xFF);
                            byte addr_L = (byte)(Reg2ByteAddr & 0x00FF);
                            DeviceObject.Devices.PMBus.SpecialRead(Convert.ToByte(SlaveAddress, 16), addr_H, addr_L, 1, ref buffer, EnablePEC);
                            Data = buffer[0].ToString("X2");
                        }
                        else
                        { 
                            DeviceObject.Devices.PMBus.ReadByte(Convert.ToByte(SlaveAddress, 16), ReadSendDate[1], out data, EnablePEC);
                            Data = data.ToString("X2");
                        } 
                        break;
                    case "Word":
                        if (is2ByteAddress)
                        {
                            byte[] buffer = new byte[2];
                            byte addr_H = (byte)((Reg2ByteAddr >> 8) & 0xFF);
                            byte addr_L = (byte)(Reg2ByteAddr & 0x00FF);
                            DeviceObject.Devices.PMBus.SpecialRead(Convert.ToByte(SlaveAddress, 16), addr_H, addr_L, 2, ref buffer, EnablePEC);
                            Data = ((buffer[1] << 8) + buffer[0]).ToString("X4");
                        }
                        else
                        {
                            DeviceObject.Devices.PMBus.ReadWord(Convert.ToByte(SlaveAddress, 16), ReadSendDate[1], out data1, EnablePEC);
                            Data = data1.ToString("X4"); 
                        }
                        break;
                    case "0":
                    case "Other":
                        DeviceObject.Devices.PMBus.SPIWriteReadPEC(Convert.ToByte(SlaveAddress, 16), (byte)((ReadSendDate[1] << 1) + 1), ref data1, EnablePEC);
                        Data = data1 > 0xFF ? data1.ToString("X4") : data1.ToString("X2");
                        break;
                    case "1":
                        DeviceObject.Devices.PMBus.SPIWriteReadPEC(Convert.ToByte(SlaveAddress, 16), (byte)((ReadSendDate[1] << 2) + 1), ref data1, EnablePEC);
                        Data = data1 > 0xFF ? data1.ToString("X4") : data1.ToString("X2");
                        break;
                    case "2":
                        DeviceObject.Devices.PMBus.WriteReadNBytes(ReadSendDate, ref Outdata, EnablePEC);
                        Data = (Outdata[1] + (Outdata[2] << 8)).ToString("X4");
                        break;
                    case "3":
                        ushort Tempdata = (ushort)(((ReadSendDate[1] << 9) & 0xFE00) + (1 << 8) + 0x00);
                        var wR1Data = DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)((ReadSendDate[1] << 1) + 1), Tempdata, EnablePEC);
                        //ushort Tempdata = (ushort)(((ReadSendDate[1] << 1) & 0xFE00) + (1 << 8) + 0x00);
                        //var wR1Data = DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)((ReadSendDate[1] << 1) + 1), 0x00, EnablePEC);
                        if (wR1Data != null)
                        {
                            Data = (wR1Data.Data & 0xFF).ToString("X2");
                        }
                        else
                        {
                            Data = "00";
                        }
                        break;
                    case "4":
                        DeviceObject.Devices.PMBus.WriteReadNBytes(ReadSendDate, ref Outdata, EnablePEC, 2);
                        Data = (Outdata[1] + (Outdata[2] << 8)).ToString("X4");
                        break;
                    case "5":
                        Data = DeviceObject.Devices.PMBus.ReadWordCommand(Convert.ToByte(SlaveAddress, 16), ReadSendDate[1], EnablePEC, true).ToString("X4");
                        break;
                    case "6":
                        Data = DeviceObject.Devices.PMBus.ReadWordCommand(Convert.ToByte(SlaveAddress, 16), ReadSendDate[1],EnablePEC, false).ToString("X4");
                        break;
                    case "7":
                        Data = ((byte)ReadByte(ReadSendDate[1])).ToString("X2");
                        break;
                    case "8":
                        byte[] dataList = new byte[] { };
                        var outdata = DeviceObject.Devices.PMBus.WriteReadNBytes_MPF61000(1, Convert.ToByte(SlaveAddress, 16), ReadSendDate[1], dataList, EnablePEC, 1);
                        outdata = outdata == null ? new WR1Data() : outdata;
                        Data = (outdata.Data & 0xFF).ToString("X2");
                        break;
                    case "9":
                        ReadSendDate[0] = Convert.ToByte(SlaveAddress, 16);
                        DeviceObject.Devices.PMBus.WriteReadNBytes(ReadSendDate, ref Outdata, EnablePEC, 3);
                        Data = Outdata[1].ToString("X2");
                        break;
                    case "10":
                        DeviceObject.Devices.PMBus.ReadResponse(0x00, 0x00); // Without this page 0, 0x7F might failed to opereate
                        DeviceObject.Devices.PMBus.ReadWord_MPQ78000FS(ReadSendDate[1], out data1);
                        Data = data1.ToString("X4");
                        break;
                    case "11":
                        byte[] value = DeviceObject.Devices.PMBus.SpiReadSequence(Convert.ToByte(SlaveAddress, 16), ReadSendDate[1], 1);
                        Data = value[value.Length - 1].ToString("X2");
                        break;
                    case "12":
                        PAR = GetParMethod((ReadSendDate[1] & 0x3F) << 8);
                        var ActAdress = (PAR << 6) + (ReadSendDate[1] & 0x3F);
                        var Rdata = DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)ActAdress, (ushort)(ActAdress << 8));
                        if (Rdata != null)
                            Data = (Rdata.Data & 0xFF).ToString("X2");
                        else
                            Data = "Communication Error";
                        break;
                    case "13":
                        ushort read_data = 0;
                        bool IsBlock = DeviceObject.Devices.PMBus.SPI_MP5499(Convert.ToByte(SlaveAddress, 16), ReadSendDate[1], read_data, out read_data, 1);
                        Data = read_data.ToString("X4");
                        break;
                    case "14":
                        PAR = GetParMethod((ReadSendDate[1] & 0x3F) << 8, 1);
                        var Adress = (PAR << 6) + (ReadSendDate[1] & 0x3F);
                        var RData = DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)Adress, (ushort)(Adress << 8));
                        if (RData != null)
                            Data = (RData.Data & 0xFF).ToString("X2");
                        else
                            Data = "Communication Error";
                        break;
                }
            }
            catch (Exception ex)
            {
                main_window.UIEnabled = false;
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }

        public ICommand ScanCommand => new RelayCommand(ScanSlaveMethod, null);

        private void ScanSlaveMethod()
        {
            try
            {
                //string KitVersion = DeviceObject.Devices.PMBus.CheckKitVersion(); 暂时不需要判断版本， 比较耗时
                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())  //|| (KitVersion == null)
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                    return;
                }

                if (DeviceObject.Devices.PMBus.DeviceName.ToUpper().Contains("COM"))// SCAN function is no need for uart
                {
                    return;
                }
                var flg = false;
                for (byte i = 0; i < 0x80; i++)
                {
                   /* if (i == 81 || i == 0x0C)
                        continue;*/
                    flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x1, false);
                    if (!flg)
                    {
                        flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0x0, true);
                    }
                    if (!flg)
                    {
                        flg = DeviceObject.Devices.PMBus.ReadResponse(i, 0xdb, true);

                    }
                    if (flg)
                    {
                        SlaveAddress = Convert.ToString(i, 16);
                        break;
                    }
                }
                if (!flg)
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                }
            }
            catch (Exception ex)
            {
                main_window.UIEnabled = false;
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }

        public ICommand SendCommand => new RelayCommand(SendMethod, null);

        private void SendMethod()
        {
            try
            {
                if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                    return;
                }
                DeviceObject.Devices.PMBus.SendCommand(Convert.ToByte(SlaveAddress, 16), Convert.ToByte(RegisterAddress, 16));
            }
            catch (Exception ex)
            {
                main_window.UIEnabled = false;
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }

        public ICommand SetFrequencyCommand => new RelayCommand<object>(SetFrequencyMethod, null);

        private void SetFrequencyMethod(object obj)
        {
            if (!DeviceObject.Devices.PMBus.IsDeviceAvailable())
            {
                main_window.UIEnabled = false;
                var ui = new WinError("VB Pro cannot find any device connected. Please check your setup.");
                ui.ShowDialog();
                main_window.UIEnabled = true;
                return;
            }
            byte wdata = 0;
            var CommunicationType = obj.ToString();
            var Gain = SelectIndexSPIClock == 0 ? 1 : 1000;
            switch (CommunicationType)
            {
                case "IIC":
                    wdata = (byte)(256 - (16000 / IICFreq[SelectIndexIICClock]));
                    DeviceObject.Devices.PMBus.SetIICFrequency(wdata);
                    break;
                case "SPI":
                    wdata = (byte)((24000 / (int.Parse(Freqdata) * Gain)) - 1);
                    DeviceObject.Devices.PMBus.SetSPIFrequency(wdata, EnumUPDOWNType.DOWN, EnumCSType.LOW);
                    break;
                case "UART":
                    wdata = (byte)(256 - 24000 / (int.Parse(Freqdata) * Gain));
                    DeviceObject.Devices.PMBus.SetIICFrequency(wdata);
                    break;
                default:
                    wdata = (byte)(256 - (16000 / IICFreq[SelectIndexIICClock]));
                    DeviceObject.Devices.PMBus.SetIICFrequency(wdata);
                    break;
            }
        }
        
        public ICommand I2CScriptCommand => new RelayCommand<object>(I2CScriptMethod, null);

        private void I2CScriptMethod(object obj)
        {
            try
            {
                while (!DeviceObject.Devices.PMBus.SetStatus(true, "I2C COMM Script Tool"))
                {
                    System.Threading.Thread.Sleep(10);
                }

                I2C_Script_Window win = new I2C_Script_Window();
                win.ShowDialog();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                DeviceObject.Devices.PMBus.SetStatus(false, "I2C COMM Script Tool");
            }
        }


        public int ReadByte(byte offset)
        {
            byte value = 0;
            var withPEC = PECStatus();
            WR1Data wR1Data = DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)((offset << 1) + 1), (ushort)(((offset << 1) + 1) << 8), withPEC);
            if (wR1Data != null)
                value = (byte)(wR1Data.Data & 0xFF);

            return value;
        }
        public void WriteByte(byte offset, int data)
        {
            var withPEC = PECStatus();
            DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)(offset << 1), (ushort)(((offset << 9) & 0xFE00) + data), withPEC);
        }
        private bool PECStatus()
        {
            byte RealValue;
            WR1Data wR1Data = DeviceObject.Devices.PMBus.SPIWriteRead16Bits(Convert.ToByte(SlaveAddress, 16), (byte)((0x29 << 1) + 1), (ushort)(((0x29 << 1) + 1) << 8), false);
            RealValue = (wR1Data != null) ? (byte)(wR1Data.Data & 0xFF) : (byte)0;
            return ((RealValue >> 1) & 0x01) == 1 ? true : false;
        }
        public byte[] StringToHexByte(string str)
        {
            try
            {
                string hex = ConverttoString(str);
                //清除所有空格
                hex = hex.Replace(" ", "");
                hex = hex.Replace("\r\n", "");
                //若字符个数为奇数，补一个0
                hex += hex.Length % 2 != 0 ? "0" : "";

                byte[] result = new byte[hex.Length / 2];
                for (int i = 0, c = result.Length; i < c; i++)
                {
                    result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }

                return result;
            }
            catch
            {


                Byte[] result = new Byte[] { };

                return result;
            }

        }
        public static string ConverttoString(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c <= 0 || c >= 127)
                {
                    s.Append(c.ToString("X4"));
                }
                else
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }

        private int GetParMethod(int value, int index = 0)
        {
            for (int i = 0; i < 16; i++)
            {
                index = index + ((value >> i) & 0x01);
            }

            return index % 2;
        }
    }
}
