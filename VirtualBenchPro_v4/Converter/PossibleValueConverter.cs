using MPS.Common;
using MPS.Common.Model;
using MPS.Common.Sqlite;
using MPS.Monitor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VirtualBenchPro_v4.Classes;


namespace VirtualBenchPro_v4.Converter
{


    public class PossibleValueConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            var lst = (value as string).Split(new char[] { ';' }).Select(o => o.Trim());
            return lst;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ColorConvertor : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //     private readonly SolidColorBrush newBrush = new SolidColorBrush(Colors.LimeGreen);
            //private readonly SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
            if (value == null)
            {
                return new SolidColorBrush(Colors.Gray);
            }
            else if (value != null && (bool)value)
                return new SolidColorBrush(Colors.LawnGreen);
            else
                return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class IndicatorBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Gray;
            if ((bool)value)
                return Brushes.IndianRed;
            else
                return Brushes.LightGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SelectColorConvertor : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //     private readonly SolidColorBrush newBrush = new SolidColorBrush(Colors.LimeGreen);
            //private readonly SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
            if (value != null && (bool)value)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class IndicatorColorConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Orange);
            // return new SolidColorBrush(Color.FromRgb(145, 22, 252));

            if ((bool)value)
                return new SolidColorBrush(Colors.LightGreen);
            else
                return new SolidColorBrush(Colors.Orange);
            // return new SolidColorBrush(Colors.IndianRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ScanChipOpcityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0.6;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FaultColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Gray);
            if (!(bool)value)
                return new SolidColorBrush(Colors.LightGreen);
            else
                return new SolidColorBrush(Colors.IndianRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitorInputParameterVisibility : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //     private readonly SolidColorBrush newBrush = new SolidColorBrush(Colors.LimeGreen);
            //private readonly SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
            if (value == null)
                return System.Windows.Visibility.Collapsed;
            else
                return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BitNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bitname = (string)values[0];
            var position = (byte)values[1];

            var str = string.Format("{0}(D{1})", bitname, position);
            return str;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HelpInfoImageVisibility : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //     private readonly SolidColorBrush newBrush = new SolidColorBrush(Colors.LimeGreen);
            //private readonly SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return System.Windows.Visibility.Collapsed;
            else
                return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class WaveformVisbleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                if ((bool)value)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                return ConvertBack(value, targetType, parameter, culture);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((Visibility)value == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return Convert(value, targetType, parameter, culture);
            }
        }
    }

    public class DecimalHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var x = System.Convert.ToUInt32(value);
            //return x.ToString("X2");
            if (x > 0xFF)
            {
                return x.ToString("X3");
            }
            else
            {
                return x.ToString("X2");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MultiRegisterAddressHexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte addrL = System.Convert.ToByte(values[0]);
            byte addrH = System.Convert.ToByte(values[1]);
            return addrH.ToString("X2").TrimStart('0') + addrL.ToString("X2");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            if (value == null || str == "-")
                return string.Empty;
            return string.Format(" ({0})", str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FriendlyNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var friendlyName = (string)value;
            if (string.IsNullOrEmpty(friendlyName))
            {
                return string.Empty;
            }
            else
            {
                return $" ({friendlyName})";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DSVersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dsversion = (string)value;
            if (string.IsNullOrEmpty(dsversion))
            {
                return string.Empty;
            }
            else
            {
                return $" (Datasheet Rev. {dsversion})";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderNameInfoConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var p = parameter as string;

            if (p == "Name")
            {
                var name = values[0] as string;
                return name;
            }
            else if (p == "Info")
            {
                var info = values[1] as string;
                return info;
            }
            else
                return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class stringInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as string;
            //SqliteHelper sqlite = new SqliteHelper();
            //CipherSqliteHelper sqlite = new CipherSqliteHelper();
            if ((string)parameter == "Tip")
            {
                var str = ConstantModel.sqlite.GetHelpStringId(key);
                return str;
            }
            else
            {
                var img = ConstantModel.sqlite.GetImagebyId(key);
                var source = Views.CustomElements.Help_GroupBox.Bytes2BitmapImage(img);
                return source;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class ChannelIndicatorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //var ch0fault = values[0] as bool?;
            //var ch1fault = values[1] as bool?;
            //var ch = values[2] as string;

            //bool? res = null;
            //if (ch == "CH0")
            //{
            //    res = ch0fault;
            //}
            //else if (ch == "CH1")
            //{
            //    res = ch1fault;
            //}

            //if (res == null)
            //{
            //    return Brushes.Gray;
            //}
            //else
            //{
            //    if ((bool)res)
            //    {
            //        return Brushes.IndianRed;
            //    }
            //    else
            //    {
            //        return Brushes.LightGreen;
            //    }
            //}
            string type = parameter as string;
            var PageName = values[0] as string;

            if (PageName == "CH 0") PageName = "CH0";
            if (PageName == "CH 1") PageName = "CH1";
            if (PageName == "CH 2") PageName = "CH2";
            if (PageName == "CH 3") PageName = "CH3";


            if (type == "Indicator")
            {
                bool? res = null;

                for (int i = 1; i < 10; i++)
                {
                    var it = values[i] as MonitorFault;
                    if (it != null && it.FaultName == PageName)
                    {
                        res = it.IsFault;
                        break;
                    }
                }

                if (res == null)
                {
                    return Brushes.Gray;
                }
                else
                {
                    if ((bool)res)
                    {
                        return Brushes.IndianRed;
                    }
                    else
                    {
                        return Brushes.LightGreen;
                    }
                }
            }
            else   //visibility
            {
                MonitorFault targetFault = null;
                for (int i = 1; i < 10; i++)
                {
                    var it = values[i] as MonitorFault;
                    if (it != null && it.FaultName == PageName)
                    {
                        targetFault = it;
                        break;
                    }
                }

                if (targetFault == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ParamIndicator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //var ch0fault = values[0] as bool?;
            //var ch1fault = values[1] as bool?;
            ////return Brushes.ForestGreen;

            //if (ch0fault == null || ch1fault == null)
            //{
            //    return Brushes.Gray;
            //}
            //else
            //{
            //    var res = (bool)ch0fault || (bool)ch1fault;
            //    if (res)
            //    {
            //        return Brushes.IndianRed;
            //    }
            //    else
            //    {
            //        return Brushes.LightGreen;
            //    }
            //}

            bool allFaultsareNull = true;
            foreach (var item in values)
            {
                var fault = item as MonitorFault;
                if (fault != null && fault.FaultName != string.Empty)
                {
                    if (fault.IsFault == null)
                    {
                        return Brushes.Gray;
                    }
                    else if ((bool)fault.IsFault)
                    {
                        return Brushes.IndianRed;
                    }
                    allFaultsareNull = false;
                }
            }
            if (allFaultsareNull)
            {
                return Brushes.Gray;
            }
            else
            {
                return Brushes.LightGreen;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitorVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as string).Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MP5470B:
                case EnumPartnumber.MP5470:
                case EnumPartnumber.MPM54304:
                case EnumPartnumber.MPQ8883:
                case EnumPartnumber.MPM3593:
                case EnumPartnumber.mEZDPD3603A:
                case EnumPartnumber.mEZDPD3603AS:
                case EnumPartnumber.MPQ8875A:
                case EnumPartnumber.MPQ8872:
                case EnumPartnumber.MPQ8873:
                case EnumPartnumber.MPQ8874:
                case EnumPartnumber.MP8843:
                case EnumPartnumber.MPQ8856:
                case EnumPartnumber.MP8856GQE:
                case EnumPartnumber.MP8845:
                case EnumPartnumber.MPQ7928:
                case EnumPartnumber.MPQ7929:
                case EnumPartnumber.MPQ70241FS:
                case EnumPartnumber.MPQ70242FS:
                case EnumPartnumber.MPQ70244FS:
                case EnumPartnumber.MPQ70245FS:
                case EnumPartnumber.MPQ4210:
                case EnumPartnumber.MP28167_A:
                case EnumPartnumber.MP28167_B:
                case EnumPartnumber.MPM4730:
                case EnumPartnumber.MP4255:
                case EnumPartnumber.MPQ7241:
                case EnumPartnumber.MPQ70331FS:
                case EnumPartnumber.MPQ70332FS:
                case EnumPartnumber.MPQ70430FS:
                case EnumPartnumber.MPQ70333FS:
                case EnumPartnumber.MPQ7902:
                case EnumPartnumber.MPQ2283:
                case EnumPartnumber.MPQ2284:
                case EnumPartnumber.MPQ2285:
                case EnumPartnumber.MPQ2286:
                case EnumPartnumber.MPQ2287:
                case EnumPartnumber.MPQ2288:
                case EnumPartnumber.MPQ7222:
                case EnumPartnumber.MPQ7225:
                case EnumPartnumber.MPQ7227:
                case EnumPartnumber.MPQ7228:
                case EnumPartnumber.MPQ7225B:
                case EnumPartnumber.MP5479:
                case EnumPartnumber.MPQ7930:
                case EnumPartnumber.MPQ7931:
                case EnumPartnumber.MPQ7934:
                case EnumPartnumber.MPQ7240:
                case EnumPartnumber.MPQ79700FS:
                case EnumPartnumber.MPQ79750FS:
                case EnumPartnumber.MPQ7960:
                case EnumPartnumber.MPQ4263:
                case EnumPartnumber.MPQ5858:
                case EnumPartnumber.MPQ5856H:
                case EnumPartnumber.MPQ5857:
                case EnumPartnumber.MPQ70160FS:
                case EnumPartnumber.MPQ70161FS:
                case EnumPartnumber.MPQ70165FS:
                case EnumPartnumber.MPQ70166FS:
                case EnumPartnumber.MPQ7932:
                case EnumPartnumber.MPQ4262A:
                case EnumPartnumber.MP8870:
                case EnumPartnumber.MPQ7920:
                case EnumPartnumber.MPQ7936:
                case EnumPartnumber.MP5513:
                case EnumPartnumber.MPF52000:
                case EnumPartnumber.MPF52000_A:
                case EnumPartnumber.MPQ4214:
                case EnumPartnumber.MPQ4284:
                case EnumPartnumber.MPQ70240FS:
                case EnumPartnumber.MPQ70340FS:
                case EnumPartnumber.MPQ76250FS_A:
                case EnumPartnumber.MPQ70341FS:
                case EnumPartnumber.MPQ70342FS:
                case EnumPartnumber.MP5413:
                case EnumPartnumber.MP5417:
                case EnumPartnumber.MP5417C:
                case EnumPartnumber.MPQ74180FS:
                case EnumPartnumber.MPQ4282:
                case EnumPartnumber.MPQ7795:
                case EnumPartnumber.MPQ7795A:
                case EnumPartnumber.MPQ7795U:
                case EnumPartnumber.MP5424:
                case EnumPartnumber.MP8128:
                case EnumPartnumber.MP8859:
                // case EnumPartnumber.MP8021:
                case EnumPartnumber.MP8747B:
                case EnumPartnumber.MPQ4232:
                case EnumPartnumber.MPQ9232:
                case EnumPartnumber.MPQ4232A:
                case EnumPartnumber.MPQ9232A:
                case EnumPartnumber.MPQ4233:
                case EnumPartnumber.MPQ4233A:
                case EnumPartnumber.MP4235:
                case EnumPartnumber.MPQ4234:
                case EnumPartnumber.MP4246:
                case EnumPartnumber.CD4246:
                case EnumPartnumber.MPQ8847A:
                case EnumPartnumber.MPQ4272:
                case EnumPartnumber.MP5475:
                case EnumPartnumber.MPQ5856:
                case EnumPartnumber.MP3439:
                case EnumPartnumber.MP9795B:
                case EnumPartnumber.MP2984:
                case EnumPartnumber.MPQ8857:
                case EnumPartnumber.MP8857:
                case EnumPartnumber.MP5416:
                case EnumPartnumber.MP5415:
                case EnumPartnumber.MP5496:
                case EnumPartnumber.MP2980:
                case EnumPartnumber.MP2718:
                case EnumPartnumber.MP2718A:
                case EnumPartnumber.MP2719:
                case EnumPartnumber.MP5491:
                case EnumPartnumber.MPQ7970:
                case EnumPartnumber.MPQ70700FS:
                case EnumPartnumber.MPQ70701FS:
                case EnumPartnumber.MP2658:
                case EnumPartnumber.MPQ7794T:
                case EnumPartnumber.MP8846:
                case EnumPartnumber.MP8847:
                case EnumPartnumber.MP8868:
                case EnumPartnumber.MP8867:
                case EnumPartnumber.MPQ70150FS:
                case EnumPartnumber.MPQ70152FS:
                case EnumPartnumber.MPQ70153FS:
                case EnumPartnumber.MPQ70150FS_M:
                case EnumPartnumber.MPQ70372FS:
                case EnumPartnumber.MPQ7961:
                case EnumPartnumber.MPQ7962:
                case EnumPartnumber.MPQ7963:
                case EnumPartnumber.MPQ7964:
                case EnumPartnumber.MPQ7965:
                case EnumPartnumber.MPQ7966:
                case EnumPartnumber.MPQ7967:
                case EnumPartnumber.MPQ79720FS:
                case EnumPartnumber.MPQ79730FS:
                case EnumPartnumber.MP8865:
                case EnumPartnumber.MP8864:
                case EnumPartnumber.MPQ8861:
                case EnumPartnumber.MP8891:
                case EnumPartnumber.MP8892:
                case EnumPartnumber.MP4248:
                case EnumPartnumber.MPQ8887:
                case EnumPartnumber.MPQ8887A:
                case EnumPartnumber.MP8877:
                case EnumPartnumber.MP8887:
                case EnumPartnumber.MP8899:
                case EnumPartnumber.MP5456:
                case EnumPartnumber.MPQ3445:
                case EnumPartnumber.MPQ3446:
                case EnumPartnumber.MP3446U:
                case EnumPartnumber.MPQ3447:
                case EnumPartnumber.MPQ7935:
                case EnumPartnumber.M5CKD02GBZ_Z:
                case EnumPartnumber.MPQ7260:
                case EnumPartnumber.MPQ2294:
                case EnumPartnumber.MPQ71020FS:
                case EnumPartnumber.MP9187:
                case EnumPartnumber.MPQ71018FS:
                case EnumPartnumber.MPQ71411FS:
                case EnumPartnumber.MPQ78000FS:
                case EnumPartnumber.MP5468:
                case EnumPartnumber.MPQ8835A:
                case EnumPartnumber.MPQ8837:
                case EnumPartnumber.MPQ4236:
                case EnumPartnumber.MP4236A:
                case EnumPartnumber.MP3710:
                case EnumPartnumber.MPQ4249:
                case EnumPartnumber.MPQ5450:
                case EnumPartnumber.MPQ77743FS:
                case EnumPartnumber.MPQ5843:
                case EnumPartnumber.MPQ75000FS:
                case EnumPartnumber.MPQ8878:
                case EnumPartnumber.MPQ70354FS:
                case EnumPartnumber.MPQ76200FS:
                case EnumPartnumber.MPQ76201FS:
                case EnumPartnumber.MPQ76252FS_B:
                case EnumPartnumber.MP28165:
                case EnumPartnumber.MPQ8889:
                case EnumPartnumber.MP8741B:
                case EnumPartnumber.MPQ4237GUTE:
                case EnumPartnumber.MP8860:
                    return Visibility.Collapsed;
                case EnumPartnumber.MP5514:
                case EnumPartnumber.MP5515:
                case EnumPartnumber.MP5477:
                case EnumPartnumber.MP9477B:
                case EnumPartnumber.MP5055A:
                case EnumPartnumber.MP5451:
                case EnumPartnumber.MPF52002_0017:
                case EnumPartnumber.MPF52002_0001:
                case EnumPartnumber.MPF52002_0004:
                case EnumPartnumber.MPF52002_001C:
                case EnumPartnumber.MPF52002_0027:
                case EnumPartnumber.MPF52002_001B:
                case EnumPartnumber.MPF52002_0003:
                case EnumPartnumber.MPF52002_0002:
                case EnumPartnumber.MPF52002_0033:
                case EnumPartnumber.MPQ76300FS:
                case EnumPartnumber.MPQ8897GX:
                case EnumPartnumber.MPQ76400FS:
                case EnumPartnumber.MPQ76402FS:
                //case EnumPartnumber.MP2798:
                //case EnumPartnumber.MP3716:
                case EnumPartnumber.MP8850:
                case EnumPartnumber.MPM2896_20:
                case EnumPartnumber.MPM3898_50:
                case EnumPartnumber.MP2737:
                case EnumPartnumber.MP2745:
                case EnumPartnumber.MPQ7212:
                case EnumPartnumber.MPQ7212C:
                case EnumPartnumber.MPQ7213:
                case EnumPartnumber.MPQ7213C:
                case EnumPartnumber.MPQ3448:
                    return (string)parameter == "Monitoring" ? Visibility.Visible : Visibility.Collapsed;
                case EnumPartnumber.MPQ5476:
                case EnumPartnumber.MPF32010:
                case EnumPartnumber.MP5440:
                case EnumPartnumber.M5SPD0GX:
                    return (string)parameter == "Waveform" ? Visibility.Collapsed : Visibility.Visible;
                case EnumPartnumber.MPQ70250FS:
                case EnumPartnumber.MPQ70350FS:
                case EnumPartnumber.MPQ76252FS_A:
                    return (string)parameter == "Waveform" ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FaultVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as string).Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MPQ8883:
                case EnumPartnumber.MPM3593:
                case EnumPartnumber.mEZDPD3603A:
                case EnumPartnumber.mEZDPD3603AS:
                case EnumPartnumber.MP8843:
                case EnumPartnumber.MP8845:
                case EnumPartnumber.MP5479:
                case EnumPartnumber.MP5514:
                case EnumPartnumber.MP5515:
                case EnumPartnumber.MP5424:
                case EnumPartnumber.MPF52000:
                case EnumPartnumber.MPF52000_A:
                case EnumPartnumber.MP3439:
                case EnumPartnumber.MP2658:
                case EnumPartnumber.MPQ8896:
                case EnumPartnumber.MPQ76300FS:
                case EnumPartnumber.MP28165:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RegisterVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as string).Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MPF52000:
                case EnumPartnumber.MPF52000_A:
                    //case EnumPartnumber.MP2658:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as Visibility?;
            if ((Visibility)text == Visibility.Visible)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d04437"));
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e7ef"));

            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class BorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as Visibility?;
            if ((Visibility)text == Visibility.Visible)
            {
                return 1.5;
            }
            else
            {
                return 1;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BorderBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue ||
                values[2] == DependencyProperty.UnsetValue || values[0] == null || values[1] == null || values[2] == null)
            {
                return Brushes.Transparent;
            }
            var isEnable = (bool)(values[0]);
            var bitbackcolor = values[1] as SolidColorBrush;
            var text = values[2] as string;

            if (isEnable)
            {
                if (text == "")
                {
                    return Brushes.Transparent;
                }
                else
                {
                    return bitbackcolor;
                }
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e7ef"));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterMarkVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isFocus = values[0] as bool?;
            var text = values[1] as string;

            if ((string)parameter == "license")
            {
                if (text != string.Empty)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    if ((bool)isFocus)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }

            }

            var hasError = values[2] as bool?;
            if (isFocus == null)
            {
                return Visibility.Collapsed;
            }
            if ((bool)isFocus)
            {
                if (text == string.Empty)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                if (hasError != null && (bool)hasError)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChangeAddrMenuItemVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var chipname = values[0] as string;
            var addr = values[1] as string;

            if ((chipname == null && addr == null))
            {
                return Visibility.Collapsed;
            }
            else
            {
                if (chipname == "MPQ73350FS" || chipname == "MPQ7212" || chipname == "MPQ7212C" || chipname == "MPQ7213"
                    || chipname == "MPQ7213C" || addr.Contains("SPI") || addr.Contains("UART"))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DeviceAddrToToolTipTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var addr = value as string;
            if (addr.Contains("v") || addr.Contains("-") || addr.Contains("SPI"))
            {
                return "Right mouse click to set friendly name";
            }
            else
            {
                return "Right mouse click to change address or set friendly name";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class bitVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            if (val)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitorInputParamsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OperationEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var chip = values[0] as Chip;
            var license = values[1] as LicenseInfo;
            if (chip == null || license == null)
            {
                return false;
            }

            var type = parameter as string;
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chip.ChipName.Replace('-', '_'));

            //virtual chip has been removed, this condition will never be satisfied.
            if (chip.ChipAddr.Contains("v"))
            {
                if (type == "WriteRAM")
                {
                    switch (pn)
                    {
                        case EnumPartnumber.MPF52000:
                        case EnumPartnumber.MPF52000_A:
                        case EnumPartnumber.MP2658:
                            return false;
                        default:
                            return true;
                    }
                }
                else if (type == "Export")
                {
                    switch (pn)
                    {
                        case EnumPartnumber.MP2718:
                        case EnumPartnumber.MP2718A:
                        case EnumPartnumber.MP2719:
                        case EnumPartnumber.MP2798:
                        case EnumPartnumber.MP3716:
                        case EnumPartnumber.MPQ3716:
                        case EnumPartnumber.MP3712:
                        case EnumPartnumber.MPQ3712:
                        case EnumPartnumber.MP3713:
                        case EnumPartnumber.MP3714:
                        case EnumPartnumber.MP3718:
                        case EnumPartnumber.MP3728:
                        case EnumPartnumber.MP2764:
                        case EnumPartnumber.MP2765:
                        case EnumPartnumber.MP2764A:
                        case EnumPartnumber.MP2733B:
                        case EnumPartnumber.MP2770A:
                        case EnumPartnumber.MPQ78232FS:
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (type == "ReadROM")
            {
                switch (pn)
                {
                    case EnumPartnumber.ZCU_REF_GEN2:
                    case EnumPartnumber.MPQ7212:
                    case EnumPartnumber.MPQ7213:
                    case EnumPartnumber.MPQ7212C:
                    case EnumPartnumber.MPQ7213C:
                    case EnumPartnumber.MPQ70245FS:
                    case EnumPartnumber.MP5470B:
                    case EnumPartnumber.MP5470:
                    case EnumPartnumber.MPM54304:
                    case EnumPartnumber.MP8853:
                    case EnumPartnumber.MP8854:
                    case EnumPartnumber.MPQ8883:
                    case EnumPartnumber.MPM3593:
                    case EnumPartnumber.mEZDPD3603A:
                    // case EnumPartnumber.mEZDPD3603AS:
                    case EnumPartnumber.MP8861:
                    case EnumPartnumber.MPQ8861:
                    case EnumPartnumber.MP8869:
                    case EnumPartnumber.MP8869S:
                    case EnumPartnumber.MP8869W:
                    case EnumPartnumber.MPQ8875A:
                    case EnumPartnumber.MPQ8872:
                    case EnumPartnumber.MPQ8873:
                    case EnumPartnumber.MPQ8874:
                    case EnumPartnumber.MP8843:
                    case EnumPartnumber.MPQ8856:
                    case EnumPartnumber.MP8856GQE:
                    case EnumPartnumber.MP8845:
                    case EnumPartnumber.MPM54313:
                    case EnumPartnumber.MPQ4210:
                    case EnumPartnumber.MP28167_A:
                    case EnumPartnumber.MP28167_B:
                    case EnumPartnumber.MPM4730:
                    case EnumPartnumber.MPQ5031:
                    case EnumPartnumber.MPQ2026:
                    case EnumPartnumber.MPQ2026A:
                    case EnumPartnumber.MPQ2024:
                    case EnumPartnumber.MPQ2024A:
                    case EnumPartnumber.MPQ2022:
                    case EnumPartnumber.MPQ2022A:
                    case EnumPartnumber.MPF32010:
                    case EnumPartnumber.MPQ7241:
                    case EnumPartnumber.MP5511:
                    case EnumPartnumber.MP5511A:
                    case EnumPartnumber.MPQ7210:
                    case EnumPartnumber.MPQ7210A:
                    case EnumPartnumber.MPQ70331FS:
                    case EnumPartnumber.MPQ70332FS:
                    case EnumPartnumber.MPQ70333FS:
                    case EnumPartnumber.MPQ70430FS:
                    case EnumPartnumber.MPQ7902:
                    case EnumPartnumber.MP2772:
                    case EnumPartnumber.MPM54322:
                    case EnumPartnumber.MPM54522:
                    case EnumPartnumber.MPQ5472:
                    case EnumPartnumber.MPM54532GPB:
                    case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MPQ7225:
                    case EnumPartnumber.MPQ7227:
                    case EnumPartnumber.MPQ7228:
                    case EnumPartnumber.MPQ7225B:
                    case EnumPartnumber.MPM54524:
                    case EnumPartnumber.MP5479:
                    case EnumPartnumber.MPQ7240:
                    case EnumPartnumber.MP5516:
                    case EnumPartnumber.MP5516N:
                    case EnumPartnumber.MP5519:
                    case EnumPartnumber.CD5519:
                    case EnumPartnumber.MP5514:
                    case EnumPartnumber.MP5515:
                    case EnumPartnumber.MPQ79700FS:
                    case EnumPartnumber.MPQ79750FS:
                    case EnumPartnumber.MPQ7960:
                    case EnumPartnumber.MPQ79500FS:
                    case EnumPartnumber.MPQ79550FS:
                    case EnumPartnumber.MPQ79501FS:
                    case EnumPartnumber.MPQ79500AFS:
                    case EnumPartnumber.MPQ79505FS:
                    case EnumPartnumber.MPQ79530FS:
                    case EnumPartnumber.MP5473:
                    case EnumPartnumber.MPQ4263:
                    case EnumPartnumber.MPQ5858:
                    case EnumPartnumber.MPQ5856H:
                    case EnumPartnumber.MPQ5857:
                    case EnumPartnumber.MPQ7920:
                    case EnumPartnumber.MPQ7936:
                    case EnumPartnumber.MPQ4262A:
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MPQ4214:
                    case EnumPartnumber.MPQ4284:
                    case EnumPartnumber.MP3924:
                    case EnumPartnumber.MP3921:
                    case EnumPartnumber.MP3928:
                    case EnumPartnumber.CD3029:
                    case EnumPartnumber.MTS1312:
                    case EnumPartnumber.MP8826:
                    case EnumPartnumber.MP5413:
                    case EnumPartnumber.MP5417:
                    case EnumPartnumber.MP5417C:
                    case EnumPartnumber.MPQ71000FS:
                    case EnumPartnumber.MPQ2023:
                    case EnumPartnumber.MPQ2023A:
                    case EnumPartnumber.MPQ74180FS:
                    case EnumPartnumber.MPQ4282:
                    case EnumPartnumber.MPQ7795:
                    case EnumPartnumber.MPQ7795A:
                    case EnumPartnumber.MPQ7795U:
                    case EnumPartnumber.MP5424:
                    case EnumPartnumber.MP8128:
                    case EnumPartnumber.MP8859:
                    case EnumPartnumber.MP8021:
                    case EnumPartnumber.MP8031:
                    case EnumPartnumber.MP8747B:
                    case EnumPartnumber.MPQ4232:
                    case EnumPartnumber.MPQ9232:
                    case EnumPartnumber.MPQ4232A:
                    case EnumPartnumber.MPQ9232A:
                    case EnumPartnumber.MPQ4233:
                    case EnumPartnumber.MPQ4233A:
                    case EnumPartnumber.MP4235:
                    case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ70171FS:
                    case EnumPartnumber.MPQ4234:
                    case EnumPartnumber.MP4246:
                    case EnumPartnumber.CD4246:
                    case EnumPartnumber.MPQ8847A:
                    case EnumPartnumber.MPQ4272:
                    case EnumPartnumber.MP5518:
                    case EnumPartnumber.MP5475:
                    case EnumPartnumber.MPQ5856:
                    case EnumPartnumber.MP5520:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MPQ5864:
                    case EnumPartnumber.MPQ77240FS:
                    case EnumPartnumber.MPQ77220FS:
                    case EnumPartnumber.MPQ5862:
                    case EnumPartnumber.MP8833A:
                    case EnumPartnumber.MP8833B:
                    case EnumPartnumber.MP9107:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP3439:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MP2733B:
                    case EnumPartnumber.MP2770A:
                    case EnumPartnumber.MPM3599:
                    case EnumPartnumber.MPM38852:
                    case EnumPartnumber.MP2984:
                    case EnumPartnumber.MPQ8857:
                    case EnumPartnumber.MP8857:
                    case EnumPartnumber.MP5431:
                    case EnumPartnumber.MPM24622:
                    case EnumPartnumber.MPQ5476:
                    case EnumPartnumber.MP5416:
                    case EnumPartnumber.MP5415:
                    case EnumPartnumber.MP5496:
                    case EnumPartnumber.MPQ4242:
                    case EnumPartnumber.MPQ4242B:
                    case EnumPartnumber.MPQ4240:
                    case EnumPartnumber.MP2980:
                    case EnumPartnumber.MPQ74201FS:
                    case EnumPartnumber.MPQ74202FS:
                    case EnumPartnumber.MP5446:
                    case EnumPartnumber.MP5446A:
                    case EnumPartnumber.MPQ7926:
                    case EnumPartnumber.MPQ7921:
                    case EnumPartnumber.MPQ7922:
                    case EnumPartnumber.MP5491:
                    case EnumPartnumber.MPQ7970:
                    case EnumPartnumber.MPQ70700FS:
                    case EnumPartnumber.MPQ70701FS:
                    case EnumPartnumber.MP5472:
                    case EnumPartnumber.MP5471:
                    case EnumPartnumber.MP2658:
                    case EnumPartnumber.MP8846:
                    case EnumPartnumber.MP8847:
                    case EnumPartnumber.MPQ7794T:
                    case EnumPartnumber.MP8868:
                    case EnumPartnumber.MP8867:
                    case EnumPartnumber.MP2718:
                    case EnumPartnumber.MP2718A:
                    case EnumPartnumber.MP2719:
                    case EnumPartnumber.MPF52002_001B:
                    case EnumPartnumber.MPF52002_0002:
                    case EnumPartnumber.MP2798:
                    case EnumPartnumber.MP3710:
                    case EnumPartnumber.MP3712:
                    case EnumPartnumber.MPQ3712:
                    case EnumPartnumber.MP3713:
                    case EnumPartnumber.MP3714:
                    case EnumPartnumber.MP3716:
                    case EnumPartnumber.MPQ3716:
                    case EnumPartnumber.MP3718:
                    case EnumPartnumber.MP3728:
                    case EnumPartnumber.MPQ7961:
                    case EnumPartnumber.MPQ7962:
                    case EnumPartnumber.MPQ7963:
                    case EnumPartnumber.MPQ7964:
                    case EnumPartnumber.MPQ7965:
                    case EnumPartnumber.MPQ7966:
                    case EnumPartnumber.MPQ7967:
                    case EnumPartnumber.MPQ79720FS:
                    case EnumPartnumber.MPQ79730FS:
                    case EnumPartnumber.MPQ70150FS:
                    case EnumPartnumber.MPQ70152FS:
                    case EnumPartnumber.MPQ70153FS:
                    case EnumPartnumber.MPQ70372FS:
                    case EnumPartnumber.MPQ70150FS_M:
                    case EnumPartnumber.MP8865:
                    case EnumPartnumber.MP8864:
                    case EnumPartnumber.MP5440:
                    case EnumPartnumber.MPQ70342FS:
                    case EnumPartnumber.MPQ73350FS:
                    case EnumPartnumber.MPQ76350FS:
                    case EnumPartnumber.MP8891:
                    case EnumPartnumber.MP8892:
                    case EnumPartnumber.MP4248:
                    case EnumPartnumber.MPF61000:
                    case EnumPartnumber.MPQ3445:
                    case EnumPartnumber.MPQ3446:
                    case EnumPartnumber.MP3446U:
                    case EnumPartnumber.MPQ3447:
                    case EnumPartnumber.MPQ3448:
                    case EnumPartnumber.MP5432:
                    case EnumPartnumber.MPQ70335FS:
                    case EnumPartnumber.MPQ70336FS:
                    case EnumPartnumber.MP5436:
                    case EnumPartnumber.MP5434:
                    case EnumPartnumber.MPF52100_MP3928:
                    case EnumPartnumber.MPQ7935:
                    case EnumPartnumber.MPQ8895:
                    case EnumPartnumber.MPQ8896:
                    case EnumPartnumber.MPQ7794:
                    case EnumPartnumber.MPQ7794A:
                    case EnumPartnumber.MPQ7794_0001:
                    case EnumPartnumber.MPQ76300FS:
                    case EnumPartnumber.M5CKD02GBZ_Z:
                    case EnumPartnumber.MPQ7260:
                    case EnumPartnumber.MPQ76230FS:
                    case EnumPartnumber.MPQ76233FS:
                    case EnumPartnumber.MPQ76250FS_B:
                    case EnumPartnumber.MPQ8897GX:
                    case EnumPartnumber.MPQ4243:
                    case EnumPartnumber.MPQ4244:
                    case EnumPartnumber.MP2769:
                    case EnumPartnumber.MPQ2294:
                    case EnumPartnumber.MPQ71020FS:
                    case EnumPartnumber.MP9187:
                    case EnumPartnumber.MPQ71018FS:
                    case EnumPartnumber.MPQ71411FS:
                    case EnumPartnumber.MPQ76400FS:
                    case EnumPartnumber.MPQ76402FS:
                    case EnumPartnumber.MP8850:
                    case EnumPartnumber.MPM2896_20:
                    case EnumPartnumber.MPM3898_50:
                    case EnumPartnumber.MPQ8835A:
                    case EnumPartnumber.MPQ8837:
                    case EnumPartnumber.MPQ4236:
                    case EnumPartnumber.MP4236A:
                    case EnumPartnumber.MP4241:
                    case EnumPartnumber.MPQ4241:
                    case EnumPartnumber.MP5494:
                    case EnumPartnumber.MP8899:
                    case EnumPartnumber.MP5456:
                    case EnumPartnumber.MPM26100:
                    case EnumPartnumber.MPQ2282:
                    case EnumPartnumber.MPQ2281:
                    case EnumPartnumber.MPQ2275:
                    case EnumPartnumber.MPQ4249:
                    case EnumPartnumber.GCS_48V_V02:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ77743FS:
                    case EnumPartnumber.MPQ5843:
                    case EnumPartnumber.MPQ75000FS:
                    case EnumPartnumber.MP5701:
                    case EnumPartnumber.MP5498:
                    case EnumPartnumber.MPQ8878:
                    case EnumPartnumber.MPQ5885:
                    case EnumPartnumber.MPQ5886D:
                    case EnumPartnumber.MPQ5888D:
                    case EnumPartnumber.MPQ77415FS:
                    case EnumPartnumber.MPQ77416FS:
                    case EnumPartnumber.MPQ77418FS:
                    case EnumPartnumber.MPQ77426FS:
                    case EnumPartnumber.MPQ77428FS:
                    case EnumPartnumber.MP5499:
                    case EnumPartnumber.MPQ4221:
                    case EnumPartnumber.MP28165:
                    case EnumPartnumber.MP2737:
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MPQ8889:
                    case EnumPartnumber.MPQ76200FS:
                    case EnumPartnumber.MPQ76201FS:
                    case EnumPartnumber.MPQ76252FS_B:
                    case EnumPartnumber.MP8741B:
                    case EnumPartnumber.MP8860:
                        return false;
                    case EnumPartnumber.MPM36100GPL:
                    case EnumPartnumber.MPM3895_25:
                        return license.IsMPSAE;
                    default:
                        return true;
                }

            }
            else if (type == "WriteROM")
            {
                switch (pn)
                {
                    case EnumPartnumber.ZCU_REF_GEN2:
                    case EnumPartnumber.MPQ70245FS:
                    case EnumPartnumber.MP8853:
                    case EnumPartnumber.MP8854:
                    case EnumPartnumber.MP8861:
                    case EnumPartnumber.MPQ8861:
                    case EnumPartnumber.MP8869:
                    case EnumPartnumber.MP8869S:
                    case EnumPartnumber.MP8869W:
                    case EnumPartnumber.MP8843:
                    case EnumPartnumber.MPQ8856:
                    case EnumPartnumber.MP8856GQE:
                    case EnumPartnumber.MP8845:
                    case EnumPartnumber.MPQ4210:
                    case EnumPartnumber.MP28167_A:
                    case EnumPartnumber.MP28167_B:
                    case EnumPartnumber.MPM4730:
                    case EnumPartnumber.MP4255:
                    case EnumPartnumber.MPQ5031:
                    case EnumPartnumber.MPF32010:
                    case EnumPartnumber.MPQ70333FS:
                    case EnumPartnumber.MPQ7902:
                    case EnumPartnumber.MP2772:
                    case EnumPartnumber.MP5514:
                    case EnumPartnumber.MP5515:
                    case EnumPartnumber.MPQ4263:
                    case EnumPartnumber.MPQ4262A:
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MPQ4214:
                    case EnumPartnumber.MPQ4284:
                    case EnumPartnumber.MP3924:
                    case EnumPartnumber.MP3921:
                    case EnumPartnumber.MP3928:
                    case EnumPartnumber.CD3029:
                    case EnumPartnumber.MTS1312:
                    case EnumPartnumber.MP5413:
                    case EnumPartnumber.MP5417:
                    case EnumPartnumber.MP5417C:
                    case EnumPartnumber.MPQ74180FS:
                    case EnumPartnumber.MPQ4282:
                    case EnumPartnumber.MPQ7795:
                    case EnumPartnumber.MPQ7795A:
                    case EnumPartnumber.MPQ7795U:
                    case EnumPartnumber.MP8128:
                    case EnumPartnumber.MP8859:
                    case EnumPartnumber.MP8021:
                    case EnumPartnumber.MP8031:
                    case EnumPartnumber.MP8747B:
                    case EnumPartnumber.MPQ4232:
                    case EnumPartnumber.MPQ9232:
                    case EnumPartnumber.MPQ4232A:
                    case EnumPartnumber.MPQ9232A:
                    case EnumPartnumber.MPQ4233:
                    case EnumPartnumber.MPQ4233A:
                    case EnumPartnumber.MP4235:
                    // case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ4234:
                    case EnumPartnumber.MP4246:
                    case EnumPartnumber.CD4246:
                    case EnumPartnumber.MPQ8847A:
                    case EnumPartnumber.MPQ4272:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MPQ5864:
                    case EnumPartnumber.MPQ77240FS:
                    case EnumPartnumber.MPQ77220FS:
                    case EnumPartnumber.MPQ5862:
                    case EnumPartnumber.MP8833A:
                    case EnumPartnumber.MP8833B:
                    case EnumPartnumber.MP9107:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP3439:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MP2733B:
                    case EnumPartnumber.MP2770A:
                    case EnumPartnumber.MPM3599:
                    case EnumPartnumber.MP2984:
                    case EnumPartnumber.MPQ8857:
                    case EnumPartnumber.MP8857:
                    case EnumPartnumber.MP2980:
                    case EnumPartnumber.MP5491:
                    //case EnumPartnumber.MPQ7970:
                    //case EnumPartnumber.MPQ70700FS:
                    //case EnumPartnumber.MPQ70701FS:
                    case EnumPartnumber.MP2658:
                    case EnumPartnumber.MP8846:
                    case EnumPartnumber.MP8847:
                    case EnumPartnumber.MPQ7794T:
                    case EnumPartnumber.MP8868:
                    case EnumPartnumber.MP8867:
                    case EnumPartnumber.MP2718:
                    case EnumPartnumber.MP2718A:
                    case EnumPartnumber.MP2719:
                    case EnumPartnumber.MPF52002_001B:
                    case EnumPartnumber.MPF52002_0002:
                    //case EnumPartnumber.MP2798:
                    //case EnumPartnumber.MP3716:
                    //case EnumPartnumber.MP3712:
                    //case EnumPartnumber.MP3713:
                    //case EnumPartnumber.MP3714:
                  //  case EnumPartnumber.MPQ8896:
                    case EnumPartnumber.MP8865:
                    case EnumPartnumber.MP8864:
                    case EnumPartnumber.MPQ70342FS:
                    case EnumPartnumber.MP4248:
                    case EnumPartnumber.MPF61000:
                    //case EnumPartnumber.MPQ73350FS:
                    case EnumPartnumber.MPQ76350FS:
                    case EnumPartnumber.MPQ7794:
                    case EnumPartnumber.MPQ7794A:
                    case EnumPartnumber.MPQ7794_0001:
                    case EnumPartnumber.M5CKD02GBZ_Z:
                    case EnumPartnumber.MPQ7260:
                    //case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MP2769:
                    case EnumPartnumber.MPQ8835A:
                    case EnumPartnumber.MPQ8837:
                    case EnumPartnumber.MPQ4236:
                    case EnumPartnumber.MP4236A:
                    case EnumPartnumber.MP5494:
                    case EnumPartnumber.MPQ4249:
                    //case EnumPartnumber.MPQ76400FS:
                    //case EnumPartnumber.MPQ76402FS:
                    case EnumPartnumber.GCS_48V_V02:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ75000FS:
                    case EnumPartnumber.MPQ8878:
                    case EnumPartnumber.MPQ5885:
                    case EnumPartnumber.MPQ5886D:
                    case EnumPartnumber.MPQ5888D:
                    case EnumPartnumber.MPQ77415FS:
                    case EnumPartnumber.MPQ77416FS:
                    case EnumPartnumber.MPQ77418FS:
                    case EnumPartnumber.MPQ77426FS:
                    case EnumPartnumber.MPQ77428FS:
                    case EnumPartnumber.MP5499:
                    case EnumPartnumber.MP28165:
                    case EnumPartnumber.MP2737:
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MPQ8889:
                    case EnumPartnumber.MP8741B:
                    case EnumPartnumber.MP8860:
                        return false;
                    case EnumPartnumber.MPM3895_25:
                    case EnumPartnumber.MP8850:
                    case EnumPartnumber.MPM2896_20:
                    case EnumPartnumber.MPM3898_50:
                    case EnumPartnumber.MPM36100GPL:
                        return license.IsMPSAE;
                    default:
                        return true;
                }

            }
            else if (type == "ReadRAM")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return false;
                    default:
                        return true;
                }
            }
            else if (type == "WriteRAM")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return false;
                    default:
                        return true;
                }
            }
            else if (type == "Readback")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return false;
                    default:
                        return true;
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OperationStrokeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
            var chip = values[0] as Chip;
            var license = values[1] as LicenseInfo;
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chip.ChipName.Replace('-', '_'));
            var type = parameter as string;
            if (chip.ChipAddr.Contains("v") && type != "WriteRAM")
            {
                return Brushes.LightGray;
            }

            if (type == "ReadROM")
            {
                switch (pn)
                {
                    case EnumPartnumber.ZCU_REF_GEN2:
                    case EnumPartnumber.MPQ7212:
                    case EnumPartnumber.MPQ7213:
                    case EnumPartnumber.MPQ7212C:
                    case EnumPartnumber.MPQ7213C:
                    case EnumPartnumber.MPQ76200FS:
                    case EnumPartnumber.MPQ76201FS:
                    case EnumPartnumber.MPQ76252FS_B:
                    case EnumPartnumber.MPQ70245FS:
                    case EnumPartnumber.MP5470B:
                    case EnumPartnumber.MP5470:
                    case EnumPartnumber.MPM54304:
                    case EnumPartnumber.MP8853:
                    case EnumPartnumber.MP8854:
                    case EnumPartnumber.MPQ8883:
                    case EnumPartnumber.MPM3593:
                    case EnumPartnumber.mEZDPD3603A:
                    // case EnumPartnumber.mEZDPD3603AS:
                    case EnumPartnumber.MP8861:
                    case EnumPartnumber.MPQ8861:
                    case EnumPartnumber.MP8869:
                    case EnumPartnumber.MP8869S:
                    case EnumPartnumber.MP8869W:
                    case EnumPartnumber.MPQ8875A:
                    case EnumPartnumber.MPQ8872:
                    case EnumPartnumber.MPQ8873:
                    case EnumPartnumber.MPQ8874:
                    case EnumPartnumber.MP8843:
                    case EnumPartnumber.MPQ8856:
                    case EnumPartnumber.MP8856GQE:
                    case EnumPartnumber.MP8845:
                    case EnumPartnumber.MPM54313:
                    case EnumPartnumber.MPQ4210:
                    case EnumPartnumber.MP28167_A:
                    case EnumPartnumber.MP28167_B:
                    case EnumPartnumber.MPM4730:
                    case EnumPartnumber.MPQ5031:
                    case EnumPartnumber.MPQ2026:
                    case EnumPartnumber.MPQ2026A:
                    case EnumPartnumber.MPQ2024:
                    case EnumPartnumber.MPQ2024A:
                    case EnumPartnumber.MPQ2022:
                    case EnumPartnumber.MPQ2022A:
                    case EnumPartnumber.MPF32010:
                    case EnumPartnumber.MPQ7241:
                    case EnumPartnumber.MPQ7240:
                    case EnumPartnumber.MP5511:
                    case EnumPartnumber.MP5511A:
                    case EnumPartnumber.MPQ7210:
                    case EnumPartnumber.MPQ7210A:
                    case EnumPartnumber.MPQ70331FS:
                    case EnumPartnumber.MPQ70332FS:
                    case EnumPartnumber.MPQ70333FS:
                    case EnumPartnumber.MPQ70430FS:
                    case EnumPartnumber.MPQ7902:
                    case EnumPartnumber.MP2772:
                    case EnumPartnumber.MPM54322:
                    case EnumPartnumber.MPM54522:
                    case EnumPartnumber.MPQ5472:
                    case EnumPartnumber.MPM54532GPB:
                    case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MPQ7225:
                    case EnumPartnumber.MPQ7227:
                    case EnumPartnumber.MPQ7228:
                    case EnumPartnumber.MPQ7225B:
                    case EnumPartnumber.MPM54524:
                    case EnumPartnumber.MP5479:
                    case EnumPartnumber.MP5516:
                    case EnumPartnumber.MP5516N:
                    case EnumPartnumber.MP5519:
                    case EnumPartnumber.CD5519:
                    case EnumPartnumber.MP5514:
                    case EnumPartnumber.MP5515:
                    case EnumPartnumber.MPQ79700FS:
                    case EnumPartnumber.MPQ79750FS:
                    case EnumPartnumber.MPQ7960:
                    case EnumPartnumber.MPQ79500FS:
                    case EnumPartnumber.MPQ79501FS:
                    case EnumPartnumber.MPQ79500AFS:
                    case EnumPartnumber.MPQ79550FS:
                    case EnumPartnumber.MPQ79505FS:
                    case EnumPartnumber.MPQ79530FS:
                    case EnumPartnumber.MP5473:
                    case EnumPartnumber.MPQ4263:
                    case EnumPartnumber.MPQ5858:
                    case EnumPartnumber.MPQ5856H:
                    case EnumPartnumber.MPQ5857:
                    case EnumPartnumber.MPQ7920:
                    case EnumPartnumber.MPQ7936:
                    case EnumPartnumber.MPQ4262A:
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MPQ4214:
                    case EnumPartnumber.MPQ4284:
                    case EnumPartnumber.MP3924:
                    case EnumPartnumber.MP3921:
                    case EnumPartnumber.MP3928:
                    case EnumPartnumber.CD3029:
                    case EnumPartnumber.MTS1312:
                    case EnumPartnumber.MP8826:
                    case EnumPartnumber.MP5413:
                    case EnumPartnumber.MP5417:
                    case EnumPartnumber.MP5417C:
                    case EnumPartnumber.MPQ71000FS:
                    case EnumPartnumber.MPQ2023:
                    case EnumPartnumber.MPQ2023A:
                    case EnumPartnumber.MPQ74180FS:
                    case EnumPartnumber.MPQ4282:
                    case EnumPartnumber.MPQ7795:
                    case EnumPartnumber.MPQ7795A:
                    case EnumPartnumber.MPQ7795U:
                    case EnumPartnumber.MP5424:
                    case EnumPartnumber.MP8128:
                    case EnumPartnumber.MP8859:
                    case EnumPartnumber.MP8021:
                    case EnumPartnumber.MP8031:
                    case EnumPartnumber.MP8747B:
                    case EnumPartnumber.MPQ4232:
                    case EnumPartnumber.MPQ9232:
                    case EnumPartnumber.MPQ4232A:
                    case EnumPartnumber.MPQ9232A:
                    case EnumPartnumber.MPQ4233:
                    case EnumPartnumber.MPQ4233A:
                    case EnumPartnumber.MP4235:
                    case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ70171FS:
                    case EnumPartnumber.MPQ4234:
                    case EnumPartnumber.MP4246:
                    case EnumPartnumber.CD4246:
                    case EnumPartnumber.MPQ8847A:
                    case EnumPartnumber.MPQ4272:
                    case EnumPartnumber.MP5518:
                    case EnumPartnumber.MP5475:
                    case EnumPartnumber.MPQ5856:
                    case EnumPartnumber.MP5520:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MPQ5864:
                    case EnumPartnumber.MPQ77240FS:
                    case EnumPartnumber.MPQ77220FS:
                    case EnumPartnumber.MPQ5862:
                    case EnumPartnumber.MP8833A:
                    case EnumPartnumber.MP8833B:
                    case EnumPartnumber.MP9107:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP3439:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MP2733B:
                    case EnumPartnumber.MP2770A:
                    case EnumPartnumber.MPM3599:
                    case EnumPartnumber.MPM38852:
                    case EnumPartnumber.MP2984:
                    case EnumPartnumber.MPQ8857:
                    case EnumPartnumber.MP8857:
                    case EnumPartnumber.MP5431:
                    case EnumPartnumber.MPM24622:
                    case EnumPartnumber.MPQ5476:
                    case EnumPartnumber.MP5416:
                    case EnumPartnumber.MP5496:
                    case EnumPartnumber.MP5415:
                    case EnumPartnumber.MPQ4242:
                    case EnumPartnumber.MPQ4242B:
                    case EnumPartnumber.MPQ4240:
                    case EnumPartnumber.MP2980:
                    case EnumPartnumber.MPQ74201FS:
                    case EnumPartnumber.MPQ74202FS:
                    case EnumPartnumber.MP5446:
                    case EnumPartnumber.MP5446A:
                    case EnumPartnumber.MPQ7926:
                    case EnumPartnumber.MPQ7921:
                    case EnumPartnumber.MPQ7922:
                    case EnumPartnumber.MP5491:
                    case EnumPartnumber.MPQ7970:
                    case EnumPartnumber.MPQ70700FS:
                    case EnumPartnumber.MPQ70701FS:
                    case EnumPartnumber.MP5472:
                    case EnumPartnumber.MP5471:
                    case EnumPartnumber.MP2658:
                    case EnumPartnumber.MP8846:
                    case EnumPartnumber.MP8847:
                    case EnumPartnumber.MPQ7794T:
                    case EnumPartnumber.MP8868:
                    case EnumPartnumber.MP8867:
                    case EnumPartnumber.MP2718:
                    case EnumPartnumber.MP2718A:
                    case EnumPartnumber.MP2719:
                    case EnumPartnumber.MPF52002_001B:
                    case EnumPartnumber.MPF52002_0002:
                    case EnumPartnumber.MP2798:
                    case EnumPartnumber.MP3710:
                    case EnumPartnumber.MP3712:
                    case EnumPartnumber.MPQ3712:
                    case EnumPartnumber.MP3713:
                    case EnumPartnumber.MP3714:
                    case EnumPartnumber.MP3716:
                    case EnumPartnumber.MPQ3716:
                    case EnumPartnumber.MP3718:
                    case EnumPartnumber.MP3728:
                    case EnumPartnumber.MPQ7961:
                    case EnumPartnumber.MPQ7962:
                    case EnumPartnumber.MPQ7963:
                    case EnumPartnumber.MPQ7964:
                    case EnumPartnumber.MPQ7965:
                    case EnumPartnumber.MPQ7966:
                    case EnumPartnumber.MPQ7967:
                    case EnumPartnumber.MPQ79720FS:
                    case EnumPartnumber.MPQ79730FS:
                    case EnumPartnumber.MPQ70150FS:
                    case EnumPartnumber.MPQ70152FS:
                    case EnumPartnumber.MPQ70153FS:
                    case EnumPartnumber.MPQ70372FS:
                    case EnumPartnumber.MPQ70150FS_M:
                    case EnumPartnumber.MP8865:
                    case EnumPartnumber.MP8864:
                    case EnumPartnumber.MP5440:
                    case EnumPartnumber.MPQ70342FS:
                    case EnumPartnumber.MP8891:
                    case EnumPartnumber.MP8892:
                    case EnumPartnumber.MP4248:
                    case EnumPartnumber.MPQ73350FS:
                    case EnumPartnumber.MPQ76350FS:
                    case EnumPartnumber.MPF61000:
                    case EnumPartnumber.MPQ3445:
                    case EnumPartnumber.MPQ3446:
                    case EnumPartnumber.MP3446U:
                    case EnumPartnumber.MPQ3447:
                    case EnumPartnumber.MPQ3448:
                    case EnumPartnumber.MP5432:
                    case EnumPartnumber.MPQ70335FS:
                    case EnumPartnumber.MPQ70336FS:
                    case EnumPartnumber.MP5436:
                    case EnumPartnumber.MP5434:
                    case EnumPartnumber.MPF52100_MP3928:
                    case EnumPartnumber.MPQ7935:
                    case EnumPartnumber.MPQ8895:
                    case EnumPartnumber.MPQ8896:
                    case EnumPartnumber.MPQ7794:
                    case EnumPartnumber.MPQ7794A:
                    case EnumPartnumber.MPQ7794_0001:
                    case EnumPartnumber.MPQ76300FS:
                    case EnumPartnumber.M5CKD02GBZ_Z:
                    case EnumPartnumber.MPQ7260:
                    case EnumPartnumber.MPQ76230FS:
                    case EnumPartnumber.MPQ76233FS:
                    case EnumPartnumber.MPQ76250FS_B:
                    case EnumPartnumber.MPQ8897GX:
                    case EnumPartnumber.MPQ4243:
                    case EnumPartnumber.MPQ4244:
                    case EnumPartnumber.MP2769:
                    case EnumPartnumber.MPQ2294:
                    case EnumPartnumber.MPQ71020FS:
                    case EnumPartnumber.MP9187:
                    case EnumPartnumber.MPQ71018FS:
                    case EnumPartnumber.MPQ71411FS:
                    case EnumPartnumber.MPQ76400FS:
                    case EnumPartnumber.MPQ76402FS:
                    case EnumPartnumber.MP8850:
                    case EnumPartnumber.MPM2896_20:
                    case EnumPartnumber.MPM3898_50:
                    case EnumPartnumber.MPQ8835A:
                    case EnumPartnumber.MPQ8837:
                    case EnumPartnumber.MPQ4236:
                    case EnumPartnumber.MP4236A:
                    case EnumPartnumber.MP4241:
                    case EnumPartnumber.MPQ4241:
                    case EnumPartnumber.MP5494:
                    case EnumPartnumber.MP8899:
                    case EnumPartnumber.MP5456:
                    case EnumPartnumber.MPM26100:
                    case EnumPartnumber.MPQ2282:
                    case EnumPartnumber.MPQ2281:
                    case EnumPartnumber.MPQ2275:
                    case EnumPartnumber.MPQ4249:
                    case EnumPartnumber.GCS_48V_V02:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ77743FS:
                    case EnumPartnumber.MPQ5843:
                    case EnumPartnumber.MPQ75000FS:
                    case EnumPartnumber.MP5701:
                    case EnumPartnumber.MP5498:
                    case EnumPartnumber.MPQ8878:
                    case EnumPartnumber.MPQ5885:
                    case EnumPartnumber.MPQ5886D:
                    case EnumPartnumber.MPQ5888D:
                    case EnumPartnumber.MPQ77415FS:
                    case EnumPartnumber.MPQ77416FS:
                    case EnumPartnumber.MPQ77418FS:
                    case EnumPartnumber.MPQ77426FS:
                    case EnumPartnumber.MPQ77428FS:
                    case EnumPartnumber.MP5499:
                    case EnumPartnumber.MP28165:
                    case EnumPartnumber.MP2737:
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MPQ8889:
                    case EnumPartnumber.MP8741B:
                    case EnumPartnumber.MP8860:
                        return Brushes.LightGray;
                    case EnumPartnumber.MPM3895_25:
                    case EnumPartnumber.MPM36100GPL:
                        return license.IsMPSAE ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F")) : Brushes.LightGray;
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
                }
            }
            else if (type == "WriteROM")
            {
                switch (pn)
                {
                    case EnumPartnumber.ZCU_REF_GEN2:
                    case EnumPartnumber.MPQ70245FS:
                    case EnumPartnumber.MP8853:
                    case EnumPartnumber.MP8854:
                    case EnumPartnumber.MP8861:
                    case EnumPartnumber.MPQ8861:
                    case EnumPartnumber.MP8869:
                    case EnumPartnumber.MP8869S:
                    case EnumPartnumber.MP8869W:
                    case EnumPartnumber.MP8843:
                    case EnumPartnumber.MPQ8856:
                    case EnumPartnumber.MP8856GQE:
                    case EnumPartnumber.MP8845:
                    case EnumPartnumber.MPQ4210:
                    case EnumPartnumber.MP28167_A:
                    case EnumPartnumber.MP28167_B:
                    case EnumPartnumber.MPM4730:
                    case EnumPartnumber.MP4255:
                    case EnumPartnumber.MPQ5031:
                    case EnumPartnumber.MPF32010:
                    case EnumPartnumber.MPQ70333FS:
                    case EnumPartnumber.MPQ7902:
                    case EnumPartnumber.MP2772:
                    case EnumPartnumber.MP5514:
                    case EnumPartnumber.MP5515:
                    case EnumPartnumber.MPQ4263:
                    case EnumPartnumber.MPQ4262A:
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MPQ4214:
                    case EnumPartnumber.MPQ4284:
                    case EnumPartnumber.MP3924:
                    case EnumPartnumber.MP3921:
                    case EnumPartnumber.MP3928:
                    case EnumPartnumber.CD3029:
                    case EnumPartnumber.MTS1312:
                    case EnumPartnumber.MP5413:
                    case EnumPartnumber.MP5417:
                    case EnumPartnumber.MP5417C:
                    case EnumPartnumber.MPQ74180FS:
                    case EnumPartnumber.MPQ4282:
                    case EnumPartnumber.MPQ7795:
                    case EnumPartnumber.MPQ7795A:
                    case EnumPartnumber.MPQ7795U:
                    case EnumPartnumber.MP8128:
                    case EnumPartnumber.MP8859:
                    case EnumPartnumber.MP8021:
                    case EnumPartnumber.MP8031:
                    case EnumPartnumber.MP8747B:
                    case EnumPartnumber.MPQ4232:
                    case EnumPartnumber.MPQ9232:
                    case EnumPartnumber.MPQ4232A:
                    case EnumPartnumber.MPQ9232A:
                    case EnumPartnumber.MPQ4233:
                    case EnumPartnumber.MPQ4233A:
                    case EnumPartnumber.MP4235:
                    // case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ4234:
                    case EnumPartnumber.CD4246:
                    case EnumPartnumber.MP4246:
                    case EnumPartnumber.MPQ8847A:
                    case EnumPartnumber.MPQ4272:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MPQ5864:
                    case EnumPartnumber.MPQ77240FS:
                    case EnumPartnumber.MPQ77220FS:
                    case EnumPartnumber.MPQ5862:
                    case EnumPartnumber.MP8833A:
                    case EnumPartnumber.MP8833B:
                    case EnumPartnumber.MP9107:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP3439:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MP2733B:
                    case EnumPartnumber.MP2770A:
                    case EnumPartnumber.MPM3599:
                    case EnumPartnumber.MP2984:
                    case EnumPartnumber.MPQ8857:
                    case EnumPartnumber.MP8857:
                    case EnumPartnumber.MP2980:
                    case EnumPartnumber.MP5491:
                    //case EnumPartnumber.MPQ7970:
                    //case EnumPartnumber.MPQ70700FS:
                    //case EnumPartnumber.MPQ70701FS:
                    case EnumPartnumber.MP2658:
                    case EnumPartnumber.MP8846:
                    case EnumPartnumber.MP8847:
                    case EnumPartnumber.MPQ7794T:
                    case EnumPartnumber.MP8868:
                    case EnumPartnumber.MP8867:
                    case EnumPartnumber.MP2718:
                    case EnumPartnumber.MP2718A:
                    case EnumPartnumber.MP2719:
                    case EnumPartnumber.MPF52002_001B:
                    case EnumPartnumber.MPF52002_0002:
                    //case EnumPartnumber.MP2798:
                    //case EnumPartnumber.MP3716:
                    //case EnumPartnumber.MP3712:
                    //case EnumPartnumber.MP3713:
                    //case EnumPartnumber.MP3714:              
                    case EnumPartnumber.MP8865:
                    case EnumPartnumber.MP8864:
                    case EnumPartnumber.MPQ70342FS:
                    case EnumPartnumber.MP4248:
                    case EnumPartnumber.MPF61000:
                    //case EnumPartnumber.MPQ73350FS:
                    case EnumPartnumber.MPQ76350FS:
                    case EnumPartnumber.MPQ7794:
                    case EnumPartnumber.MPQ7794A:
                    case EnumPartnumber.MPQ7794_0001:
                    case EnumPartnumber.M5CKD02GBZ_Z:
                    case EnumPartnumber.MPQ7260:
                    //case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MP2769:
                    case EnumPartnumber.MPQ8835A:
                    case EnumPartnumber.MPQ8837:
                    case EnumPartnumber.MPQ4236:
                    case EnumPartnumber.MP4236A:
                    case EnumPartnumber.MP5494:
                    case EnumPartnumber.MPQ4249:
                    //case EnumPartnumber.MPQ76400FS:
                    //case EnumPartnumber.MPQ76402FS:
                    case EnumPartnumber.GCS_48V_V02:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ75000FS:
                    case EnumPartnumber.MPQ8878:
                    case EnumPartnumber.MPQ5885:
                    case EnumPartnumber.MPQ5886D:
                    case EnumPartnumber.MPQ5888D:
                    case EnumPartnumber.MPQ77415FS:
                    case EnumPartnumber.MPQ77416FS:
                    case EnumPartnumber.MPQ77418FS:
                    case EnumPartnumber.MPQ77426FS:
                    case EnumPartnumber.MPQ77428FS:
                    case EnumPartnumber.MP5499:
                    case EnumPartnumber.MP28165:
                    case EnumPartnumber.MP2737:
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MPQ8889:
                    case EnumPartnumber.MP8741B:
                    case EnumPartnumber.MP8860:
                        return Brushes.LightGray;
                    case EnumPartnumber.MPM3895_25:
                    case EnumPartnumber.MP8850:
                    case EnumPartnumber.MPM2896_20:
                    case EnumPartnumber.MPM3898_50:
                    case EnumPartnumber.MPM36100GPL:
                        return license.IsMPSAE ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F")) : Brushes.LightGray;
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
                }
            }
            else if (type == "WriteRAM")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return Brushes.LightGray;
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
                }
            }
            else if (type == "ReadRAM")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return Brushes.LightGray;
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
                }
            }
            else if (type == "Readback")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return Brushes.LightGray;
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
                }
            }

            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0A3C7F"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OperationToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var operation = parameter as string;
            var chip = values[0] as Chip;
            var license = values[1] as LicenseInfo;
            if (chip == null || license == null)
            {
                return operation;
            }

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chip.ChipName.Replace('-', '_'));
            if (operation == "ReadROM")
            {
                switch (pn)
                {
                    case EnumPartnumber.ZCU_REF_GEN2:
                    case EnumPartnumber.MPQ7212:
                    case EnumPartnumber.MPQ7213:
                    case EnumPartnumber.MPQ7212C:
                    case EnumPartnumber.MPQ7213C:
                    case EnumPartnumber.MPQ76200FS:
                    case EnumPartnumber.MPQ76201FS:
                    case EnumPartnumber.MPQ76252FS_B:
                    case EnumPartnumber.MPQ70245FS:
                    case EnumPartnumber.MP5470B:
                    case EnumPartnumber.MP5470:
                    case EnumPartnumber.MPM54304:
                    case EnumPartnumber.MP8853:
                    case EnumPartnumber.MP8854:
                    case EnumPartnumber.MPQ8883:
                    case EnumPartnumber.MPM3593:
                    case EnumPartnumber.mEZDPD3603A:
                    // case EnumPartnumber.mEZDPD3603AS:
                    case EnumPartnumber.MP8861:
                    case EnumPartnumber.MPQ8861:
                    case EnumPartnumber.MP8869:
                    case EnumPartnumber.MP8869S:
                    case EnumPartnumber.MP8869W:
                    case EnumPartnumber.MPQ8875A:
                    case EnumPartnumber.MPQ8872:
                    case EnumPartnumber.MPQ8873:
                    case EnumPartnumber.MPQ8874:
                    case EnumPartnumber.MP8843:
                    case EnumPartnumber.MPQ8856:
                    case EnumPartnumber.MP8856GQE:
                    case EnumPartnumber.MP8845:
                    case EnumPartnumber.MPM54313:
                    case EnumPartnumber.MPQ4210:
                    case EnumPartnumber.MP28167_A:
                    case EnumPartnumber.MP28167_B:
                    case EnumPartnumber.MPM4730:
                    case EnumPartnumber.MPQ5031:
                    case EnumPartnumber.MPQ2026:
                    case EnumPartnumber.MPQ2026A:
                    case EnumPartnumber.MPQ2024:
                    case EnumPartnumber.MPQ2024A:
                    case EnumPartnumber.MPQ2022:
                    case EnumPartnumber.MPQ2022A:
                    case EnumPartnumber.MPQ7240:
                    case EnumPartnumber.MPQ7241:
                    case EnumPartnumber.MP5511:
                    case EnumPartnumber.MP5511A:
                    case EnumPartnumber.MPQ7210:
                    case EnumPartnumber.MPQ7210A:
                    case EnumPartnumber.MP2772:
                    case EnumPartnumber.MPM54322:
                    case EnumPartnumber.MPM54522:
                    case EnumPartnumber.MPQ5472:
                    case EnumPartnumber.MPM54532GPB:
                    case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MPQ7225:
                    case EnumPartnumber.MPQ7227:
                    case EnumPartnumber.MPQ7228:
                    case EnumPartnumber.MPQ7225B:
                    case EnumPartnumber.MPM54524:
                    case EnumPartnumber.MP5479:
                    case EnumPartnumber.MP5516:
                    case EnumPartnumber.MP5516N:
                    case EnumPartnumber.MP5519:
                    case EnumPartnumber.CD5519:
                    case EnumPartnumber.MP5514:
                    case EnumPartnumber.MP5515:
                    case EnumPartnumber.MPQ79700FS:
                    case EnumPartnumber.MPQ79750FS:
                    case EnumPartnumber.MPQ7960:
                    case EnumPartnumber.MPQ79500FS:
                    case EnumPartnumber.MPQ79550FS:
                    case EnumPartnumber.MPQ79501FS:
                    case EnumPartnumber.MPQ79500AFS:
                    case EnumPartnumber.MPQ79505FS:
                    case EnumPartnumber.MPQ79530FS:
                    case EnumPartnumber.MP5473:
                    case EnumPartnumber.MPQ4263:
                    case EnumPartnumber.MPQ5858:
                    case EnumPartnumber.MPQ5856H:
                    case EnumPartnumber.MPQ5857:
                    case EnumPartnumber.MPQ7920:
                    case EnumPartnumber.MPQ7936:
                    case EnumPartnumber.MPQ4262A:
                    case EnumPartnumber.MPQ4214:
                    case EnumPartnumber.MPQ4284:
                    case EnumPartnumber.MP3924:
                    case EnumPartnumber.MP3921:
                    case EnumPartnumber.MP3928:
                    case EnumPartnumber.CD3029:
                    case EnumPartnumber.MTS1312:
                    case EnumPartnumber.MP8826:
                    case EnumPartnumber.MP5413:
                    case EnumPartnumber.MP5417:
                    case EnumPartnumber.MP5417C:
                    case EnumPartnumber.MPQ71000FS:
                    case EnumPartnumber.MPQ2023:
                    case EnumPartnumber.MPQ2023A:
                    case EnumPartnumber.MPQ74180FS:
                    case EnumPartnumber.MPQ4282:
                    case EnumPartnumber.MPQ7795:
                    case EnumPartnumber.MPQ7795A:
                    case EnumPartnumber.MPQ7795U:
                    case EnumPartnumber.MP5424:
                    case EnumPartnumber.MP8128:
                    case EnumPartnumber.MP8859:
                    case EnumPartnumber.MP8021:
                    case EnumPartnumber.MP8031:
                    case EnumPartnumber.MPQ4232:
                    case EnumPartnumber.MPQ9232:
                    case EnumPartnumber.MPQ4232A:
                    case EnumPartnumber.MPQ9232A:
                    case EnumPartnumber.MPQ4233:
                    case EnumPartnumber.MPQ4233A:
                    case EnumPartnumber.MP4235:
                    case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ70171FS:
                    case EnumPartnumber.MPQ4234:
                    case EnumPartnumber.MP4246:
                    case EnumPartnumber.CD4246:
                    case EnumPartnumber.MPQ8847A:
                    case EnumPartnumber.MPQ4272:
                    case EnumPartnumber.MP5518:
                    case EnumPartnumber.MP5475:
                    case EnumPartnumber.MPQ5856:
                    case EnumPartnumber.MP5520:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MPQ5864:
                    case EnumPartnumber.MPQ77240FS:
                    case EnumPartnumber.MPQ77220FS:
                    case EnumPartnumber.MPQ5862:
                    case EnumPartnumber.MP8833A:
                    case EnumPartnumber.MP8833B:
                    case EnumPartnumber.MP9107:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP3439:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MP2733B:
                    case EnumPartnumber.MP2770A:
                    case EnumPartnumber.MPM3599:
                    case EnumPartnumber.MPM38852:
                    case EnumPartnumber.MP2984:
                    case EnumPartnumber.MPQ8857:
                    case EnumPartnumber.MP8857:
                    case EnumPartnumber.MP5431:
                    case EnumPartnumber.MPM24622:
                    case EnumPartnumber.MP5416:
                    case EnumPartnumber.MP5415:
                    case EnumPartnumber.MP5496:
                    case EnumPartnumber.MPQ4242:
                    case EnumPartnumber.MPQ4242B:
                    case EnumPartnumber.MPQ4240:
                    case EnumPartnumber.MP2980:
                    case EnumPartnumber.MPQ74201FS:
                    case EnumPartnumber.MPQ74202FS:
                    case EnumPartnumber.MP5446:
                    case EnumPartnumber.MP5446A:
                    case EnumPartnumber.MPQ7926:
                    case EnumPartnumber.MPQ7921:
                    case EnumPartnumber.MPQ7922:
                    case EnumPartnumber.MP5491:
                    case EnumPartnumber.MPQ7970:
                    case EnumPartnumber.MPQ70700FS:
                    case EnumPartnumber.MPQ70701FS:
                    case EnumPartnumber.MP5472:
                    case EnumPartnumber.MP5471:
                    case EnumPartnumber.MP8846:
                    case EnumPartnumber.MP8847:
                    case EnumPartnumber.MP8868:
                    case EnumPartnumber.MP8867:
                    case EnumPartnumber.MP2718:
                    case EnumPartnumber.MP2718A:
                    case EnumPartnumber.MP2719:
                    case EnumPartnumber.MP2798:
                    case EnumPartnumber.MP3710:
                    case EnumPartnumber.MP3712:
                    case EnumPartnumber.MPQ3712:
                    case EnumPartnumber.MP3713:
                    case EnumPartnumber.MP3714:
                    case EnumPartnumber.MP3716:
                    case EnumPartnumber.MPQ3716:
                    case EnumPartnumber.MP3718:
                    case EnumPartnumber.MP3728:
                    case EnumPartnumber.MPQ7961:
                    case EnumPartnumber.MPQ7962:
                    case EnumPartnumber.MPQ7963:
                    case EnumPartnumber.MPQ7964:
                    case EnumPartnumber.MPQ7965:
                    case EnumPartnumber.MPQ7966:
                    case EnumPartnumber.MPQ7967:
                    case EnumPartnumber.MPQ79720FS:
                    case EnumPartnumber.MPQ79730FS:
                    case EnumPartnumber.MPQ70150FS:
                    case EnumPartnumber.MPQ70152FS:
                    case EnumPartnumber.MPQ70153FS:
                    case EnumPartnumber.MPQ70372FS:
                    case EnumPartnumber.MPQ70150FS_M:
                    case EnumPartnumber.MP8865:
                    case EnumPartnumber.MP8864:
                    case EnumPartnumber.MP5440:
                    case EnumPartnumber.MP8891:
                    case EnumPartnumber.MP8892:
                    case EnumPartnumber.MP4248:
                    case EnumPartnumber.MPQ73350FS:
                    case EnumPartnumber.MPQ76350FS:
                    case EnumPartnumber.MPF61000:
                    case EnumPartnumber.MPQ3445:
                    case EnumPartnumber.MPQ3446:
                    case EnumPartnumber.MP3446U:
                    case EnumPartnumber.MPQ3447:
                    case EnumPartnumber.MPQ3448:
                    case EnumPartnumber.MP5432:
                    case EnumPartnumber.MPQ70335FS:
                    case EnumPartnumber.MPQ70336FS:
                    case EnumPartnumber.MP5436:
                    case EnumPartnumber.MP5434:
                    case EnumPartnumber.MPF52100_MP3928:
                    case EnumPartnumber.MPQ7935:
                    case EnumPartnumber.MPQ70331FS:
                    case EnumPartnumber.MPQ70332FS:
                    case EnumPartnumber.MPQ70333FS:
                    case EnumPartnumber.MPQ7902:
                    case EnumPartnumber.MPQ70342FS:
                    case EnumPartnumber.MPQ70430FS:
                    case EnumPartnumber.MPQ8895:
                    case EnumPartnumber.MPQ8896:
                    case EnumPartnumber.MPQ76300FS:
                    case EnumPartnumber.M5CKD02GBZ_Z:
                    case EnumPartnumber.MPQ7260:
                    case EnumPartnumber.MPQ76230FS:
                    case EnumPartnumber.MPQ76233FS:
                    case EnumPartnumber.MPQ76250FS_B:
                    case EnumPartnumber.MPQ8897GX:
                    case EnumPartnumber.MPQ4243:
                    case EnumPartnumber.MPQ4244:
                    case EnumPartnumber.MP2769:
                    case EnumPartnumber.MPQ2294:
                    case EnumPartnumber.MPQ71020FS:
                    case EnumPartnumber.MP9187:
                    case EnumPartnumber.MPQ71018FS:
                    case EnumPartnumber.MPQ71411FS:
                    case EnumPartnumber.MPQ8835A:
                    case EnumPartnumber.MPQ4236:
                    case EnumPartnumber.MPQ8837:
                    case EnumPartnumber.MP4236A:
                    case EnumPartnumber.MP4241:
                    case EnumPartnumber.MPQ4241:
                    case EnumPartnumber.MP5494:
                    case EnumPartnumber.MP8899:
                    case EnumPartnumber.MP5456:
                    case EnumPartnumber.MPM26100:
                    case EnumPartnumber.MPQ2282:
                    case EnumPartnumber.MPQ2281:
                    case EnumPartnumber.MPQ2275:
                    case EnumPartnumber.MPQ4249:
                    case EnumPartnumber.GCS_48V_V02:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ77743FS:
                    case EnumPartnumber.MPQ5843:
                    case EnumPartnumber.MPQ75000FS:
                    case EnumPartnumber.MP5498:
                    case EnumPartnumber.MPQ8878:
                    case EnumPartnumber.MPQ5885:
                    case EnumPartnumber.MPQ5886D:
                    case EnumPartnumber.MPQ5888D:
                    case EnumPartnumber.MPQ77415FS:
                    case EnumPartnumber.MPQ77416FS:
                    case EnumPartnumber.MPQ77418FS:
                    case EnumPartnumber.MPQ77426FS:
                    case EnumPartnumber.MPQ77428FS:
                    case EnumPartnumber.MP5499:
                    case EnumPartnumber.MPQ4221:
                    case EnumPartnumber.MPQ8889:
                        return pn.ToString().Replace('_', '-') + " does not support reading from ROM. To read from ROM you can power cycle your device and select Read from RAM.";
                    case EnumPartnumber.MPF32010:
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MPQ5476:
                    case EnumPartnumber.MP2658:
                    case EnumPartnumber.MPQ7794T:
                    case EnumPartnumber.MPQ7794:
                    case EnumPartnumber.MPQ7794A:
                    case EnumPartnumber.MPQ7794_0001:
                    case EnumPartnumber.MPF52002_001B:
                    case EnumPartnumber.MPF52002_0002:
                    case EnumPartnumber.MPQ76400FS:
                    case EnumPartnumber.MPQ76402FS:
                    case EnumPartnumber.MP8850:
                    case EnumPartnumber.MP5701:
                    case EnumPartnumber.MPM2896_20:
                    case EnumPartnumber.MPM3898_50:
                    case EnumPartnumber.MP28165:
                    case EnumPartnumber.MP2737:
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MP8741B:
                    case EnumPartnumber.MP8860:
                        return pn.ToString().Replace('_', '-') + " does not support this function.";
                    case EnumPartnumber.MPF52002_0001:
                    case EnumPartnumber.MPF52002_0003:
                    case EnumPartnumber.MPF52002_0004:
                    case EnumPartnumber.MPF52002_0017:
                    case EnumPartnumber.MPF52002_001C:
                    case EnumPartnumber.MPF52002_0027:
                    case EnumPartnumber.MPF52002_0033:
                        return "restore default MTP value";
                    case EnumPartnumber.M5SPD0GX:
                        return "Read from NVM";
                    case EnumPartnumber.MPM3895_25:
                    case EnumPartnumber.MPM36100GPL:
                        return license.IsMPSAE ? "Read ROM" : pn.ToString().Replace('_', '-') + " does not provide this function, If you need MTP, please contact AE.";
                    default:
                        return "Read ROM";
                }
            }
            else if (operation == "WriteROM")
            {
                switch (pn)
                {
                    case EnumPartnumber.ZCU_REF_GEN2:
                    case EnumPartnumber.MPQ70245FS:
                    case EnumPartnumber.MP8853:
                    case EnumPartnumber.MP8854:
                    case EnumPartnumber.MP8861:
                    case EnumPartnumber.MPQ8861:
                    case EnumPartnumber.MP8869:
                    case EnumPartnumber.MP8869S:
                    case EnumPartnumber.MP8869W:
                    case EnumPartnumber.MP8843:
                    case EnumPartnumber.MPQ8856:
                    case EnumPartnumber.MP8856GQE:
                    case EnumPartnumber.MP8845:
                    case EnumPartnumber.MPQ4210:
                    case EnumPartnumber.MP28167_A:
                    case EnumPartnumber.MP28167_B:
                    case EnumPartnumber.MPM4730:
                    case EnumPartnumber.MP4255:
                    case EnumPartnumber.MPQ5031:
                    case EnumPartnumber.MP2772:
                    case EnumPartnumber.MP5514:
                    case EnumPartnumber.MP5515:
                    case EnumPartnumber.MPQ4263:
                    case EnumPartnumber.MPQ4262A:
                    case EnumPartnumber.MPQ4214:
                    case EnumPartnumber.MPQ4284:
                    case EnumPartnumber.MP3924:
                    case EnumPartnumber.MP3921:
                    case EnumPartnumber.MP3928:
                    case EnumPartnumber.CD3029:
                    case EnumPartnumber.MTS1312:
                    case EnumPartnumber.MP5413:
                    case EnumPartnumber.MP5417:
                    case EnumPartnumber.MP5417C:
                    case EnumPartnumber.MPQ74180FS:
                    case EnumPartnumber.MPQ4282:
                    case EnumPartnumber.MPQ7795:
                    case EnumPartnumber.MPQ7795A:
                    case EnumPartnumber.MPQ7795U:
                    case EnumPartnumber.MP8128:
                    case EnumPartnumber.MP8859:
                    case EnumPartnumber.MP8021:
                    case EnumPartnumber.MP8031:
                    case EnumPartnumber.MP8747B:
                    case EnumPartnumber.MPQ4232:
                    case EnumPartnumber.MPQ9232:
                    case EnumPartnumber.MPQ4232A:
                    case EnumPartnumber.MPQ9232A:
                    case EnumPartnumber.MPQ4233:
                    case EnumPartnumber.MPQ4233A:
                    case EnumPartnumber.MP4235:
                    // case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ4234:
                    case EnumPartnumber.MP4246:
                    case EnumPartnumber.CD4246:
                    case EnumPartnumber.MPQ8847A:
                    case EnumPartnumber.MPQ4272:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MPQ5864:
                    case EnumPartnumber.MPQ77240FS:
                    case EnumPartnumber.MPQ77220FS:
                    case EnumPartnumber.MPQ5862:
                    case EnumPartnumber.MP8833A:
                    case EnumPartnumber.MP8833B:
                    case EnumPartnumber.MP9107:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP3439:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MP2733B:
                    case EnumPartnumber.MP2770A:
                    case EnumPartnumber.MPM3599:
                    case EnumPartnumber.MP2984:
                    case EnumPartnumber.MPQ8857:
                    case EnumPartnumber.MP8857:
                    case EnumPartnumber.MP2980:
                    case EnumPartnumber.MP5491:
                    //case EnumPartnumber.MPQ7970:
                    //case EnumPartnumber.MPQ70700FS:
                    //case EnumPartnumber.MPQ70701FS:
                    case EnumPartnumber.MP8846:
                    case EnumPartnumber.MP8847:
                    case EnumPartnumber.MP8868:
                    case EnumPartnumber.MP8867:
                    case EnumPartnumber.MP2718:
                    case EnumPartnumber.MP2718A:
                    case EnumPartnumber.MP2719:
                    //case EnumPartnumber.MP2798:
                    //case EnumPartnumber.MP3716:
                    //case EnumPartnumber.MP3712:
                    //case EnumPartnumber.MP3713:
                    //case EnumPartnumber.MP3714:
                    case EnumPartnumber.MP8865:
                    case EnumPartnumber.MP8864:
                    case EnumPartnumber.MP4248:
                    //case EnumPartnumber.MPQ73350FS:
                    case EnumPartnumber.MPQ76350FS:
                    case EnumPartnumber.MPQ70333FS:
                    //case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MPQ8835A:
                    case EnumPartnumber.MPQ4236:
                    case EnumPartnumber.MPQ8837:
                    case EnumPartnumber.MP4236A:
                    case EnumPartnumber.MP5494:
                    case EnumPartnumber.MPQ4249:
                    //case EnumPartnumber.MPQ76400FS:
                    //case EnumPartnumber.MPQ76402FS:
                    case EnumPartnumber.GCS_48V_V02:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ75000FS:
                    case EnumPartnumber.MPQ8878:
                    case EnumPartnumber.MPQ5885:
                    case EnumPartnumber.MPQ5886D:
                    case EnumPartnumber.MPQ5888D:
                    case EnumPartnumber.MPQ77415FS:
                    case EnumPartnumber.MPQ77416FS:
                    case EnumPartnumber.MPQ77418FS:
                    case EnumPartnumber.MPQ77426FS:
                    case EnumPartnumber.MPQ77428FS:
                    case EnumPartnumber.MP5499:
                    case EnumPartnumber.MP2737:
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MPQ8889:
                        return pn.ToString().Replace('_', '-') + " does not support writing to ROM.";
                    case EnumPartnumber.MPF32010:
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                    case EnumPartnumber.MPQ7794T:
                    case EnumPartnumber.MPF52002_001B:
                    case EnumPartnumber.MPF52002_0002:
                    case EnumPartnumber.MPQ7794:
                    case EnumPartnumber.MPQ7794A:
                    case EnumPartnumber.MPQ7794_0001:
                    case EnumPartnumber.M5CKD02GBZ_Z:
                    case EnumPartnumber.MP2769:
                    case EnumPartnumber.MP28165:
                    case EnumPartnumber.MP8741B:
                    case EnumPartnumber.MP8860:
                        return pn.ToString().Replace('_', '-') + " does not support this funcion.";
                    case EnumPartnumber.MPF52002_0001:
                    case EnumPartnumber.MPF52002_0003:
                    case EnumPartnumber.MPF52002_0004:
                    case EnumPartnumber.MPF52002_0017:
                    case EnumPartnumber.MPF52002_001C:
                    case EnumPartnumber.MPF52002_0027:
                    case EnumPartnumber.MPF52002_0033:
                        return "Write I2C value to Flash";
                    case EnumPartnumber.M5SPD0GX:
                        return "Write to NVM";
                    case EnumPartnumber.MPQ8897GX:
                        return "DIMM Region Write MTP";
                    //case EnumPartnumber.MPQ70331FS:
                    case EnumPartnumber.MPQ70342FS:
                    case EnumPartnumber.MPQ7902:
                    case EnumPartnumber.MPF61000:
                        return pn.ToString().Replace('_', '-') + " does not support this funcion, If you need OTP, please contact FAE.";
                    case EnumPartnumber.MPM3895_25:
                    case EnumPartnumber.MP8850:
                    case EnumPartnumber.MPM2896_20:
                    case EnumPartnumber.MPM3898_50:
                    case EnumPartnumber.MPM36100GPL:
                        return license.IsMPSAE ? "Write ROM" : pn.ToString().Replace('_', '-') + " does not provide this function, If you need MTP, please contact AE.";
                    case EnumPartnumber.MP5416:
                    case EnumPartnumber.MP5415:
                    case EnumPartnumber.MP5496:
                        return $"Write ROM. Only the registers in OTP Region will be written for {pn.ToString().Replace('_', '-')}.";
                    case EnumPartnumber.MPQ5476:
                        return $"Write ROM. Only the registers in MTP and Mode Region will be written for {pn.ToString()}.";
                    default:
                        return "Write ROM";
                }

            }
            else if (operation == "WriteRAM")
            {
                switch (pn)
                {

                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return pn.ToString().Replace('_', '-') + " does not support this funcion.";
                    case EnumPartnumber.MP5416:
                    case EnumPartnumber.MP5415:
                    case EnumPartnumber.MP5496:
                        return $"Write RAM. Only the registers in I2C Region will be written for {pn.ToString().Replace('_', '-')}.";
                    case EnumPartnumber.M5SPD0GX:
                        return "Write to MR";
                    default:
                        return "Write RAM";
                }

            }
            else if (operation == "ReadRAM")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return pn.ToString().Replace('_', '-') + " does not support this funcion.";
                    case EnumPartnumber.M5SPD0GX:
                        return "Read from MR";
                    default:
                        return "Read RAM";
                }
            }
            else if (operation == "Readback")
            {
                switch (pn)
                {
                    case EnumPartnumber.MPF52000:
                    case EnumPartnumber.MPF52000_A:
                    case EnumPartnumber.MP2658:
                        return pn.ToString().Replace('_', '-') + " does not support this funcion.";
                    default:
                        return "The \"Start Readback\" verifies that the ROM was written correctly. Before proceeding, ensure that you have followed the correct sequence for writing the ROM.";
                }
            }

            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadbackVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var chip = values[0] as Chip;
            var license = values[1] as LicenseInfo;
            if (chip == null || license == null)
            {
                return Visibility.Collapsed;
            }
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chip.ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MP8796B:
             //   case EnumPartnumber.MPM3696_35:
                case EnumPartnumber.MPM3695_10:
                case EnumPartnumber.MPM3695_25:
                case EnumPartnumber.MPM3695_100:
                case EnumPartnumber.mEZDPD1620A:
                case EnumPartnumber.mEZDPD1620AS:
                case EnumPartnumber.MPM82504:
                case EnumPartnumber.MPM3701:
                case EnumPartnumber.MPM3690_50D:
                case EnumPartnumber.CDM1680:
                case EnumPartnumber.MP9795B:
                case EnumPartnumber.MPM3695_20:
                case EnumPartnumber.MP8796K:
                case EnumPartnumber.MPM3690S_15:
                case EnumPartnumber.MPM3695GMJ_15:
                case EnumPartnumber.MPQ8645P:
                case EnumPartnumber.MPM3695L_20:
                    return Visibility.Visible;
                case EnumPartnumber.MPM3895_25:
                    return license.IsMPSAE ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CompareButtonEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var chip = values[0] as Chip;
            var IsAddrAvailable = values[1] as bool?;

            //No device Added
            if (chip == null)
            {
                return false;
            }

            if (IsAddrAvailable == true) // Real device connected
            {
                return true;
            }
            else if (IsAddrAvailable == false) // Real device disconnected
            {
                return false;
            }
            else // IsAddrAvailable == null. Detached device
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BitToolTip_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bt = value as BitControl;
            if (bt == null) return null;

            if (bt.BitLength == 0)
            {
                if (!string.IsNullOrEmpty(bt.Formula))
                {
                    return bt.Formula;
                }
                else
                {
                    return null;
                }
            }
            if (bt.BitLength == 1)
            {
                //return string.Format("{0}h D[{1}] {2}", bt.Address.ToString("X2"), bt.Position, bt.Page);
                return string.Format("{0}h D[{1}] {2}", bt.Address_H.ToString("X2").TrimStart('0') + bt.Address.ToString("X2"), bt.Position, bt.Page);
            }
            else
            {
                //return string.Format("{0}h D[{1}:{2}] {3}", bt.Address.ToString("X2"), bt.Position + bt.BitLength - 1, bt.Position, bt.Page);
                return string.Format("{0}h D[{1}:{2}] {3}", bt.Address_H.ToString("X2").TrimStart('0') + bt.Address.ToString("X2"), bt.Position + bt.BitLength - 1, bt.Position, bt.Page);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitorMTPtimesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var times = values[0] as string;
            var license = values[1] as LicenseInfo;
            var chip = values[2] as Chip;

            if (values[1]==null||values[2]==null)
            {
                return times;
            }
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chip.ChipName.Replace('-', '_'));

            if (pn== EnumPartnumber.MPM3895_25)
            {
                if (chip.IsAddrAvalible == true)
                {
                    if (license.IsMPSAE)
                    {
                        return "∞"; 
                    }
                    else 
                    {
                        return "-";
                    }

                }
                else 
                {
                    return "-";
                }

            }
          

          
            return times;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class unitVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.ToString();
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("({0})", val);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ClearFaultVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentChip = values[0] as Chip;
            var currentPage = values[1] as MPS.Common.Model.Page;

            if (currentChip == null)
                return Visibility.Collapsed;

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), currentChip.ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MP5470B:
                case EnumPartnumber.MP5470:
                case EnumPartnumber.MPM54304:
                case EnumPartnumber.MPQ8883:
                case EnumPartnumber.MPM3593:
                case EnumPartnumber.mEZDPD3603A:
                case EnumPartnumber.mEZDPD3603AS:
                case EnumPartnumber.MP8854:
                case EnumPartnumber.MP8853:
                case EnumPartnumber.MP8861:
                case EnumPartnumber.MPQ8861:
                case EnumPartnumber.MP8869:
                case EnumPartnumber.MP8869S:
                case EnumPartnumber.MP8869W:
                case EnumPartnumber.MPQ8875A:
                case EnumPartnumber.MPQ8873:
                case EnumPartnumber.MP8843:
                case EnumPartnumber.MP8845:
                case EnumPartnumber.MPQ5031:
                //case EnumPartnumber.MPQ2026:
                //case EnumPartnumber.MPQ2026A:
                //case EnumPartnumber.MPQ2024:
                //case EnumPartnumber.MPQ2022:
                case EnumPartnumber.MPF32010:
                case EnumPartnumber.MP5479:
                case EnumPartnumber.MPQ7222:
                case EnumPartnumber.MPQ7225:
                case EnumPartnumber.MPQ7227:
                case EnumPartnumber.MPQ7228:
                case EnumPartnumber.MPQ7225B:
                case EnumPartnumber.MP5413:
                case EnumPartnumber.MP5417:
                case EnumPartnumber.MP5417C:
                case EnumPartnumber.MPQ2023:
                case EnumPartnumber.MPQ2023A:
                case EnumPartnumber.MPQ74180FS:
                case EnumPartnumber.MPQ4282:
                case EnumPartnumber.MP5424:
                case EnumPartnumber.MPQ8847A:
                case EnumPartnumber.MP5055A:
                case EnumPartnumber.MP5451:
                case EnumPartnumber.MP2764:
                case EnumPartnumber.MP2765:
                case EnumPartnumber.MP2764A:
                case EnumPartnumber.MP2733B:
                case EnumPartnumber.MP2770A:
                case EnumPartnumber.MP2772:
                case EnumPartnumber.MP2718:
                case EnumPartnumber.MP2718A:
                case EnumPartnumber.MP2719:
                case EnumPartnumber.MP5416:
                case EnumPartnumber.MP5415:
                case EnumPartnumber.MP5496:
                case EnumPartnumber.MPQ74201FS:
                case EnumPartnumber.MPQ74202FS:
                case EnumPartnumber.MP5446:
                case EnumPartnumber.MP5446A:
                case EnumPartnumber.MPQ7926:
                case EnumPartnumber.MPQ7921:
                case EnumPartnumber.MPQ7922:
                case EnumPartnumber.MP5491:
                case EnumPartnumber.MP2658:
                case EnumPartnumber.MPQ7794T:
                case EnumPartnumber.MP8846:
                case EnumPartnumber.MP8847:
                case EnumPartnumber.MP8868:
                case EnumPartnumber.MP8867:
                case EnumPartnumber.MPF52002_0017:
                case EnumPartnumber.MPF52002_0001:
                case EnumPartnumber.MPF52002_0004:
                case EnumPartnumber.MPF52002_001C:
                case EnumPartnumber.MPF52002_0027:
                case EnumPartnumber.MPF52002_001B:
                case EnumPartnumber.MPF52002_0003:
                case EnumPartnumber.MPF52002_0002:
                case EnumPartnumber.MPF52002_0033:
                case EnumPartnumber.MP2798:
                case EnumPartnumber.MP3710:
                case EnumPartnumber.MP3712:
                case EnumPartnumber.MPQ3712:
                case EnumPartnumber.MP3713:
                case EnumPartnumber.MP3714:
                case EnumPartnumber.MP3716:
                case EnumPartnumber.MPQ3716:
                case EnumPartnumber.MP3718:
                case EnumPartnumber.MP3728:
                case EnumPartnumber.MP8865:
                case EnumPartnumber.MP8864:
                case EnumPartnumber.MP8891:
                case EnumPartnumber.MP8892:
                case EnumPartnumber.MPQ3445:
                case EnumPartnumber.MPQ3446:
                case EnumPartnumber.MP3446U:
                case EnumPartnumber.MPQ3447:
                case EnumPartnumber.MPQ3448:
                case EnumPartnumber.MPQ76230FS:
                case EnumPartnumber.MPQ76233FS:
                case EnumPartnumber.MPQ76250FS_B:
                case EnumPartnumber.MP8021:
                case EnumPartnumber.MP8031:
                case EnumPartnumber.MPQ4243:
                case EnumPartnumber.MPQ4244:
                case EnumPartnumber.MPQ2294:
                case EnumPartnumber.MPQ71020FS:
                case EnumPartnumber.MP9187:
                case EnumPartnumber.MPQ71018FS:
                case EnumPartnumber.MPQ71411FS:
                //case EnumPartnumber.MPQ76400FS:
                case EnumPartnumber.M5CKD02GBZ_Z:
                case EnumPartnumber.MP4241:
                case EnumPartnumber.MPQ4241:
                case EnumPartnumber.MP8899:
                case EnumPartnumber.MP5456:
                case EnumPartnumber.GCS_48V_V02:
                case EnumPartnumber.MPQ78232FS:
                case EnumPartnumber.MPQ70170FS:
                case EnumPartnumber.MPQ70171FS:
                case EnumPartnumber.MP5499:
                case EnumPartnumber.MPQ4221:
                case EnumPartnumber.MP28165:
                case EnumPartnumber.MP2737:
                case EnumPartnumber.MPQ76200FS:
                case EnumPartnumber.MPQ76201FS:
                case EnumPartnumber.MPQ76252FS_B:
                case EnumPartnumber.MP8860:
                    return Visibility.Collapsed;
                case EnumPartnumber.MPF52100_MP3928:
                    return currentPage?.PageName.StartsWith("Port") == true ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as Chip).ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MP5470B:
                case EnumPartnumber.MP5470:
                case EnumPartnumber.MPM54304:
                case EnumPartnumber.MPQ8883:
                case EnumPartnumber.MPM3593:
                case EnumPartnumber.mEZDPD3603A:
                case EnumPartnumber.mEZDPD3603AS:
                case EnumPartnumber.MP8854:
                case EnumPartnumber.MP8853:
                case EnumPartnumber.MP8861:
                case EnumPartnumber.MPQ8861:
                case EnumPartnumber.MP8869:
                case EnumPartnumber.MP8869S:
                case EnumPartnumber.MP8869W:
                case EnumPartnumber.MPQ8875A:
                case EnumPartnumber.MPQ8873:
                case EnumPartnumber.MP8843:
                case EnumPartnumber.MP8845:
                case EnumPartnumber.MPQ5031:
                //case EnumPartnumber.MPQ2026:
                //case EnumPartnumber.MPQ2026A:
                //case EnumPartnumber.MPQ2024:
                //case EnumPartnumber.MPQ2022:
                case EnumPartnumber.MPF32010:
                case EnumPartnumber.MP5479:
                case EnumPartnumber.MPQ7222:
                case EnumPartnumber.MPQ7225:
                case EnumPartnumber.MPQ7227:
                case EnumPartnumber.MPQ7228:
                case EnumPartnumber.MPQ7225B:
                case EnumPartnumber.MP5413:
                case EnumPartnumber.MP5417:
                case EnumPartnumber.MP5417C:
                case EnumPartnumber.MPQ2023:
                case EnumPartnumber.MPQ2023A:
                case EnumPartnumber.MPQ74180FS:
                case EnumPartnumber.MPQ4282:
                case EnumPartnumber.MP5424:
                case EnumPartnumber.MPQ8847A:
                case EnumPartnumber.MP5055A:
                case EnumPartnumber.MP5451:
                case EnumPartnumber.MP2764:
                case EnumPartnumber.MP2765:
                case EnumPartnumber.MP2764A:
                case EnumPartnumber.MP2733B:
                case EnumPartnumber.MP2770A:
                case EnumPartnumber.MP2772:
                case EnumPartnumber.MP2718:
                case EnumPartnumber.MP2718A:
                case EnumPartnumber.MP2719:
                case EnumPartnumber.MP5416:
                case EnumPartnumber.MP5415:
                case EnumPartnumber.MP5496:
                case EnumPartnumber.MPQ74201FS:
                case EnumPartnumber.MPQ74202FS:
                case EnumPartnumber.MP5446:
                case EnumPartnumber.MP5446A:
                case EnumPartnumber.MPQ7926:
                case EnumPartnumber.MPQ7921:
                case EnumPartnumber.MPQ7922:
                case EnumPartnumber.MP5491:
                case EnumPartnumber.MP2658:
                case EnumPartnumber.MPQ7794T:
                case EnumPartnumber.MP8846:
                case EnumPartnumber.MP8847:
                case EnumPartnumber.MP8868:
                case EnumPartnumber.MP8867:
                case EnumPartnumber.MPF52002_0017:
                case EnumPartnumber.MPF52002_0001:
                case EnumPartnumber.MPF52002_0004:
                case EnumPartnumber.MPF52002_001C:
                case EnumPartnumber.MPF52002_0027:
                case EnumPartnumber.MPF52002_001B:
                case EnumPartnumber.MPF52002_0003:
                case EnumPartnumber.MPF52002_0002:
                case EnumPartnumber.MPF52002_0033:
                case EnumPartnumber.MP2798:
                case EnumPartnumber.MP3716:
                case EnumPartnumber.MPQ3716:
                case EnumPartnumber.MP3718:
                case EnumPartnumber.MP3728:
                case EnumPartnumber.MP3712:
                case EnumPartnumber.MPQ3712:
                case EnumPartnumber.MP3713:
                case EnumPartnumber.MP3714:
                case EnumPartnumber.MP8865:
                case EnumPartnumber.MP8864:
                case EnumPartnumber.MP8891:
                case EnumPartnumber.MP8892:
                case EnumPartnumber.MPQ3445:
                case EnumPartnumber.MPQ3446:
                case EnumPartnumber.MP3446U:
                case EnumPartnumber.MPQ3447:
                case EnumPartnumber.MPQ3448:
                case EnumPartnumber.MPQ76230FS:
                case EnumPartnumber.MPQ76233FS:
                case EnumPartnumber.MPQ76250FS_B:
                case EnumPartnumber.MPF52100_MP3928:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadFaultVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            var index = int.Parse(parameter.ToString());
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as Chip).ChipName.Replace('-', '_'));
            if (index == 1)
            {
                switch (pn)
                {
                    case EnumPartnumber.MPQ7225:
                    case EnumPartnumber.MPQ7227:
                    case EnumPartnumber.MPQ7228:
                    case EnumPartnumber.MPQ7225B:
                    case EnumPartnumber.MP5451:
                    case EnumPartnumber.MP5055A:
                    case EnumPartnumber.MP2765:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else if (index == 3)
            {
                switch (pn)
                {
                    case EnumPartnumber.MPQ8896:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MP2745:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else
            {
                switch (pn)
                {
                    case EnumPartnumber.MPQ70150FS:
                    case EnumPartnumber.MPQ70152FS:
                    case EnumPartnumber.MPQ70153FS:
                    case EnumPartnumber.MPQ70150FS_M:
                    case EnumPartnumber.MPQ70372FS:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ClearFaultButtonContent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as Chip).ChipName.Replace('-', '_'));
            switch (pn)
            {

                default:
                    return "Clear Fault";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ScanI2CVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as Chip).ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MPQ2026:
                case EnumPartnumber.MPQ2026A:
                case EnumPartnumber.MPQ2024:
                case EnumPartnumber.MPQ2024A:
                case EnumPartnumber.MPQ2022:
                case EnumPartnumber.MPQ2022A:
                case EnumPartnumber.MPF32010:
                case EnumPartnumber.MPQ7241:
                case EnumPartnumber.MPQ7240:
                case EnumPartnumber.MPQ7210:
                case EnumPartnumber.MPQ7210A:
                case EnumPartnumber.MPQ70331FS:
                case EnumPartnumber.MPQ70332FS:
                case EnumPartnumber.MPQ70333FS:
                case EnumPartnumber.MPQ70430FS:
                case EnumPartnumber.MPQ7902:
                case EnumPartnumber.MPQ7225:
                case EnumPartnumber.MPQ7227:
                case EnumPartnumber.MPQ7228:
                case EnumPartnumber.MPQ7225B:
                case EnumPartnumber.MP2658:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class bgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LicenseStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            var param = parameter as string;
            if (status == null)
            {
                return Visibility.Collapsed;
            }
            if (status.ToLower() == "active")
            {
                if (param == "active" || param == "all")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            if (status.ToLower() == "inactive")
            {
                if (param == "inactive" || param == "all")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else if (status.ToLower() == "expired")
            {
                if (param == "expired" || param == "all")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            //else if (status.ToLower() == "expiring")
            //{
            //    if (param == "expiring" || param == "expdate" || param == "all")
            //    {
            //        return Visibility.Visible;
            //    }
            //    else
            //    {
            //        return Visibility.Collapsed;
            //    }
            //}
            else if (status.ToLower() == "offline")
            {
                if (param == "offline" || param == "all")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LicenseExpDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "30 DAYS REMAINING";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LicenseDetailStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as string;
            if (val == "ACTIVE")
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3ba266"));
            }
            else if (val == "INACTIVE")
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eaa435"));
            }
            else if (val == "EXPIRED")
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b94358"));
            }
            //else if (val == "EXPIRING")
            //{
            //    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eaa435"));
            //}
            else if (val == "OFFLINE")
            {
                return new SolidColorBrush(Colors.Gray);
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eaa435"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChipNameAddrConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var name = values[0] as string;
            var addr = values[1] as string;
            return string.Format("{0} ({1})", name, addr);
            //if (!addr.Contains("-"))
            //{
            //    return string.Format("{0} ({1})", name, addr);
            //}
            //else
            //{
            //    return name + " ";
            //}

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChipNameAddrFriendlyNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var name = values[0] as string;
            var addr = values[1] as string;
            var firendlyName = values[2] as string;

            var DisplayContent = "";
            if (!addr.Contains("-"))
            {
                DisplayContent = string.Format("{0} ({1})", name, addr);
            }
            else
            {
                DisplayContent = name;
            }

            if (!string.IsNullOrEmpty(firendlyName))
            {
                DisplayContent += $" ({firendlyName})";
            }

            return DisplayContent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AdvancedVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var adsetting =  value as ObservableCollection<Category>;
            if (adsetting != null && adsetting.Count > 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BasicVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var basicsetting = value as ObservableCollection<ParameterGroup>;
            if (basicsetting != null && basicsetting.Count > 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ParametersGridSplitterVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var basicSetting = values[0] as ObservableCollection<ParameterGroup>;
            var advancedSetting = values[1] as ObservableCollection<Category>;

            var isBasicSettingVisible = (basicSetting?.Count > 0) ? true : false;
            var isAdvancedSettingVisible = (advancedSetting?.Count > 0) ? true : false;

            return (isBasicSettingVisible && isAdvancedSettingVisible) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BasicRowSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var adsetting = value as ObservableCollection<Category>;
            if (adsetting != null && adsetting.Count > 0)
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AdvancedRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var basicsetting = value as ObservableCollection<ParameterGroup>;
            if (basicsetting != null && basicsetting.Count > 0)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AdvancedRowSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var basicsetting = value as ObservableCollection<ParameterGroup>;
            if (basicsetting != null && basicsetting.Count > 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RegisterToolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isReversal = parameter.ToString() == "Reversal" ? true : false;
            var hasPMbus = (bool)value;
            if (hasPMbus)
            {
                return isReversal ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return isReversal ? Visibility.Visible : Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class UCSMConverter : IMultiValueConverter
    {
        //把刷新和编辑两个按钮合成一个
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var projectConfig = values[0] as ProjectConfig;
            var licenseInfo = values[1] as LicenseInfo;


            if (projectConfig != null)
            {
                //if (projectConfig.Chips.Any(o => o.ChipName.Contains("MPQ70170FS") || o.ChipName.Contains("MPQ70171FS")) && licenseInfo.HasPMBusTool)
                if (projectConfig.Chips.Any(o => o.ChipName.Contains("MPQ70170FS") || o.ChipName.Contains("MPQ70171FS")))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }

            }
            return Visibility.Collapsed;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class RightCornerTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Remaining write cycles in NVM";

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as Chip).ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MP5511:
                case EnumPartnumber.MP5511A:
                    return "Revision number and Vendor_ID";
                //case EnumPartnumber.MPQ70331FS:
                case EnumPartnumber.MPQ7902:
                    return "Current state of the chip";
                default:
                    return "Remaining write cycles in NVM";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChannelVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as string).Replace('-', '_'));
            switch (pn)
            {
                //case EnumPartnumber.MP2772:
                //    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BatteryTabVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            var chip = value as Chip;
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chip.ChipName.Replace('-', '_'));

            if (param == "Battery" && (pn == EnumPartnumber.MP2772 || pn == EnumPartnumber.MP2764 || pn == EnumPartnumber.MP2764A))  //pn == EnumPartnumber.MP2718 || pn == EnumPartnumber.MP2719
            {
                return Visibility.Visible;
            }
            else if (param == "JEITA" && (pn == EnumPartnumber.MP2718 || pn == EnumPartnumber.MP2718A || 
                pn == EnumPartnumber.MP2719 || pn == EnumPartnumber.MP2772 || 
                pn == EnumPartnumber.MP2764 || pn == EnumPartnumber.MP2764A || 
                pn == EnumPartnumber.MP2765 || pn == EnumPartnumber.MP2737 || pn == EnumPartnumber.MP2745))
            {
                return Visibility.Visible;
            }
            else if (param == "BlackBox" && pn == EnumPartnumber.MP5513)
            {
                return Visibility.Visible;
            }
            else if (param == "Fault/Interrupt")
            {
                switch (pn)
                {
                    case EnumPartnumber.MP2745:
                    case EnumPartnumber.MP2798:
                    case EnumPartnumber.MP3716:
                    case EnumPartnumber.MPQ3716:
                    case EnumPartnumber.MP3718:
                    case EnumPartnumber.MP3728:
                    case EnumPartnumber.MP3712:
                    case EnumPartnumber.MPQ3712:
                    case EnumPartnumber.MP3713:
                    case EnumPartnumber.MP3714:
                    case EnumPartnumber.MPQ8896:
                    case EnumPartnumber.MPQ7222:
                    case EnumPartnumber.MPQ70150FS:
                    case EnumPartnumber.MPQ70152FS:
                    case EnumPartnumber.MPQ70153FS:
                    case EnumPartnumber.MPQ70150FS_M:
                    case EnumPartnumber.MPQ70170FS:
                    case EnumPartnumber.MPQ70171FS:
                    case EnumPartnumber.MPQ72957FS:
                    case EnumPartnumber.MPQ76300FS:
                    case EnumPartnumber.MPQ78000FS:
                    case EnumPartnumber.MPQ76400FS:
                    case EnumPartnumber.MPQ78232FS:
                    case EnumPartnumber.MPQ70372FS:
                    case EnumPartnumber.MPQ76402FS:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }

            }
            else
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fault = value as MonitorFault;
            if (fault == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TitleVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var title = value as string;
            if (string.IsNullOrEmpty(title))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RegisterToolBtnEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            if (str == "Byte" || str == "Word")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class RegisterToolForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            //Color color = (Color)ColorConverter.ConvertFromString("#1C4EFF");
            byte r = 28, g = 78, b = 255;
            if (str == "Byte" || str == "Word")
            {
                return new SolidColorBrush(Color.FromRgb(r, g, b));
            }
            else
            {
                return Brushes.Gray;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Cold & Hot Temperature
    /// </summary>
    public class NTCTxtValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.ToString() != "")
            {
                string str = parameter.ToString();
                var CurrentValue = "";
                string[] strArray = value.ToString().Split(new char[1] { ';' });
                if (str == "Cold")
                {
                    CurrentValue = strArray[0];
                }
                else if (str == "Cool")
                {
                    CurrentValue = strArray[1];
                }
                else if (str == "Warm")
                {
                    CurrentValue = strArray[2];
                }
                else if (str == "Hot")
                {
                    CurrentValue = strArray[3];
                }
                string[] strArray1 = CurrentValue.Split(new char[1] { ')' });
                return strArray1[0] + ")";
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///Caculate NTC Resistance value
    /// </summary>
    public class NTCResistanceTxtValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values[0] != null && values[0] != DependencyProperty.UnsetValue && values[0].ToString() != "" && values[1].ToString() != null && values[2].ToString() != null)
                {
                    string str = parameter.ToString();
                    double RT1Value = double.Parse(values[1].ToString());
                    double RT2Value = double.Parse(values[2].ToString());
                    var CurrentValue = "";
                    string[] strArray = values[0].ToString().Split(new char[1] { ';' });
                    if (str == "Cold")
                    {
                        CurrentValue = strArray[0];
                    }
                    else if (str == "Cool")
                    {
                        CurrentValue = strArray[1];
                    }
                    else if (str == "Warm")
                    {
                        CurrentValue = strArray[2];
                    }
                    else if (str == "Hot")
                    {
                        CurrentValue = strArray[3];
                    }
                    double TemperatureValue = double.Parse(CurrentValue.Split(new char[1] { '%' })[0].ToString());
                    double result = RT1Value * RT2Value * TemperatureValue / 100.0 / (RT2Value - RT1Value * TemperatureValue / 100.0 - RT2Value * TemperatureValue / 100.0);
                    return result.ToString("0.0");
                }
                return double.NaN.ToString();
            }
            catch
            {
                return "0";
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Cool & Warm Temperature
    /// </summary>
    public class NTCTemperatureValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values != null && values[0] != null && values[1].ToString() != "NaN" && values[2].ToString() != "NaN" && values[1] != null && values[2] != null)
                {
                    double RValue = double.Parse(values[0].ToString());
                    double NTCValue = double.Parse(values[1].ToString());
                    double FactorValue = double.Parse(values[2].ToString());
                    double Result = 1 / (Math.Log(RValue / NTCValue, Math.E) / FactorValue + 1 / (25 + 273.15)) - 273.15;
                    return Result.ToString("0.0");
                }
                return double.NaN.ToString();
            }
            catch
            {
                return "0";
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class PageColumnVisiblity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var x = value as string;
            if (x == "MPM3698")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RegionParamsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as RegisterRegion) != null)
            {
                var region = value as RegisterRegion;
                if (region.Parameters != null && region.Parameters.Count > 0)
                {
                    return Visibility.Visible;
                }
                if (region.ModeSelectParameter != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            else if ((value as ObservableCollection<BitControl>) != null)
            {
                var paras = value as ObservableCollection<BitControl>;
                if (paras != null && paras.Count > 0 && paras.Any(p => p.Disabled == false))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else if ((value as ModeSelectParameter) != null)
            {
                var mparam = value as ModeSelectParameter;
                if (mparam != null && !string.IsNullOrEmpty(mparam.Name))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ToggleBitVaueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var currentValue = value as string;
            //if (string.IsNullOrEmpty(currentValue))
            //{
            //    return false;
            //}
            //if (currentValue.Contains("On") || currentValue.Contains("Enable"))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            var flg = value as bool?;
            if (flg != null || !(bool)flg)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ischecked = value == null ? false : (bool)value;
            //string bitvalue;
            //if (ischecked)
            //{
            //    bitvalue = "On";
            //}
            //else
            //{
            //    bitvalue = "Off";
            //}
            //return bitvalue;

            if (ischecked)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }


    public class BitBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var color = values[1] as SolidColorBrush;
            if ((values[0] != DependencyProperty.UnsetValue) && values[0] != null)
            {
                bool isEnable = (bool)values[0];
                if (!isEnable)
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e7ef"));  // 
                }
                else
                {
                    return color;
                }
            }
            else
            {
                return color;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class BitOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                return 1;
            }
            else
            {
                return 0.7;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class BasicNoteContentConvert : IValueConverter
    {
        //CipherSqliteHelper sqlite = new CipherSqliteHelper();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var CurrentPage = (MPS.Common.Model.Page)value;
            var basicConfig = CurrentPage.Configurations.FirstOrDefault(pt => pt.Name == "Basic Configuration");
            if (basicConfig != null && basicConfig.Isnote != "")
            {
                var str = ConstantModel.sqlite.GetHelpStringId(basicConfig.Isnote);
                return str;
                //return Visibility.Visible;
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsAdvanveNoteVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var AdvanceSetting = (ObservableCollection<MPS.Common.Model.Category>)values[0];
            var CategorySelectedId = (int)values[1] < 0 ? 0 : (int)values[1];
            var SubcategorySelectedId = (int)values[2] < 0 ? 0 : (int)values[2];

            if (AdvanceSetting != null)
            {
                CategorySelectedId = CategorySelectedId >= AdvanceSetting.Count ? AdvanceSetting.Count - 1 : CategorySelectedId;
                SubcategorySelectedId = SubcategorySelectedId >= AdvanceSetting[CategorySelectedId].Subcategories.Count ? AdvanceSetting[CategorySelectedId].Subcategories.Count - 1 : SubcategorySelectedId;
                try
                {
                    if (AdvanceSetting[CategorySelectedId].Isnote != "")
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        if (AdvanceSetting[CategorySelectedId].Subcategories[SubcategorySelectedId].Isnote != "")
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                }
                catch
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //public class NoteContentConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return "";
    //        var chip = (MPS.Common.Model.Chip)value;
    //        switch (chip.ChipName)
    //        {
    //            case "MPQ70331FS":
    //                return "*To get write acess unlock the 'Lock Write Register' from the Global Settings";
    //            case "MPQ2286":
    //                return "*To get write acess unlock the 'Current Compensation Settings' from Compensation Settings";
    //            case "MPQ79700FS":
    //            case "MPQ79500FS":
    //                return "*To get write acess,unlock the 'Sequence(SEQ)'bit from the Global/Safety/Write Protection";
    //            case "MPQ70160FS":
    //                return "*The write access to this bit is not affected by 'Secure Lockout' bit in Gloabl/Safety/PMBus";
    //            //return "*To get write acess unlock the 'Lock Write/Read Advanced Config' from Current Compensation Settings in Compensation of Advanced Parameters";
    //            default:
    //                return "";
    //        }
    //    }
    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class AdvanceNoteContentMultConverter : IMultiValueConverter
    {
        //CipherSqliteHelper sqlite = new CipherSqliteHelper();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var AdvanceSetting = (ObservableCollection<MPS.Common.Model.Category>)values[0];
            var CategorySelectedId = (int)values[1] < 0 ? 0 : (int)values[1];
            var SubcategorySelectedId = (int)values[2] < 0 ? 0 : (int)values[2];

            if (AdvanceSetting != null && AdvanceSetting.Count != 0)
            {
                CategorySelectedId = CategorySelectedId >= AdvanceSetting.Count ? AdvanceSetting.Count - 1 : CategorySelectedId;
                SubcategorySelectedId = SubcategorySelectedId >= AdvanceSetting[CategorySelectedId].Subcategories.Count ? AdvanceSetting[CategorySelectedId].Subcategories.Count - 1 : SubcategorySelectedId;
                try
                {
                    if (AdvanceSetting[CategorySelectedId].Isnote != "")
                    {
                        var str = ConstantModel.sqlite.GetHelpStringId(AdvanceSetting[CategorySelectedId].Isnote);
                        return str;
                    }
                    else
                    {
                        if (AdvanceSetting[CategorySelectedId].Subcategories[SubcategorySelectedId].Isnote != "")
                        {
                            var str = ConstantModel.sqlite.GetHelpStringId(AdvanceSetting[CategorySelectedId].Subcategories[SubcategorySelectedId].Isnote);
                            return str;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CompareVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as string).Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MPF52000:
                case EnumPartnumber.MPF52000_A:
                case EnumPartnumber.MP2658:
                case EnumPartnumber.MPF52002_0017:
                case EnumPartnumber.MPF52002_0001:
                case EnumPartnumber.MPF52002_0004:
                case EnumPartnumber.MPF52002_001C:
                case EnumPartnumber.MPF52002_0027:
                case EnumPartnumber.MPF52002_001B:
                case EnumPartnumber.MPF52002_0003:
                case EnumPartnumber.MPF52002_0002:
                case EnumPartnumber.MPF52002_0033:
                case EnumPartnumber.MPQ4282:
                case EnumPartnumber.MPQ74180FS:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WaveFormsPauseButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var chip = value as Chip;
            if (chip != null && chip.ChipName != "MPQ70350FS" && chip.ChipName != "MPQ70250FS" && chip.ChipName != "MPQ76252FS-A")
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ParameterTipVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var tab = values[1] as TabControl;
            if (tab.Items.Count > 2)
            {
                var title = values[0] as string;
                if (string.IsNullOrEmpty(title))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NonValidationTextBoxWaterMarkVisibilityConveter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isFocused = values[0] as bool?;
            var text = values[1] as string;
            if (string.IsNullOrEmpty(text) & isFocused != true)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class YpointColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double Pvalue = double.Parse(value.ToString());
                return Pvalue == 0 ? Brushes.White : Brushes.Orange;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TabItemHeaderForeConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var index = parameter.ToString();
            var Color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180"));
            var Color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9EACBF"));
            var Color = index == "1" ? Color1 : Color2;
            if (values[1] != DependencyProperty.UnsetValue && values[1] != null)
            {
                var chFault = (MPS.Monitor.MonitorFault)values[1];
                return chFault.IsFault == true ? Brushes.Red : Color;
            }
            return Color;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitorButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = int.Parse(parameter.ToString());

            if (value != null && value != DependencyProperty.UnsetValue )
            {
                var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as string).Replace('-', '_'));
                switch (pn)
                {
                    case EnumPartnumber.MP2798:
                    case EnumPartnumber.MP3716:
                    case EnumPartnumber.MPQ3716:
                    case EnumPartnumber.MP3718:
                    case EnumPartnumber.MP3728:
                    case EnumPartnumber.MP3712:
                    case EnumPartnumber.MPQ3712:
                    case EnumPartnumber.MP3713:
                    case EnumPartnumber.MP3714:
                    case EnumPartnumber.MP2764:
                    case EnumPartnumber.MP2765:
                    case EnumPartnumber.MP2764A:
                    case EnumPartnumber.MPQ78232FS:
                        return index == 1 ? "Record" : "Save";
                    default:
                        return index == 1 ? "Pause" : "Continue";
                }
            }
            return index == 1 ? "Pause" : "Continue";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BitButtonContentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var chipName = values[0] as string;
            var bt = values[1] as BitControl;

            if (chipName == null || bt == null)
            {
                return null;
            }

            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), chipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MPQ78000FS:
                    return GetBitButtonContent_MPQ78000FS(bt);
                case EnumPartnumber.MP2770A:
                case EnumPartnumber.MP2733B:
                case EnumPartnumber.MP2765:
                case EnumPartnumber.MPQ5885:
                case EnumPartnumber.MPQ5886D:
                case EnumPartnumber.MPQ5888D:
                case EnumPartnumber.MPQ77415FS:
                case EnumPartnumber.MPQ77416FS:
                case EnumPartnumber.MPQ77418FS:
                case EnumPartnumber.MPQ77426FS:
                case EnumPartnumber.MPQ77428FS:
                    return "Reset";
                case EnumPartnumber.MP5701:
                case EnumPartnumber.MP2745:
                    return "Set";
                case EnumPartnumber.MP3710:
                    return "Operation";
                case EnumPartnumber.MP5499:
                    return bt.BitID== "ID0E6AAC"? "Reset": GetBitButtonContent(bt);
                default:
                    return GetBitButtonContent(bt);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string GetBitButtonContent(BitControl bt)
        {
            var possibleValues = bt?.PossibleValues;

            if (string.IsNullOrEmpty(possibleValues))
                return null;

            var possibleValueList = possibleValues.Split(';').Select(i => i.Trim()).ToList();
            var possibleValue = possibleValueList[0];
            var splittedPossibleValue = possibleValue.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            //var possibleValueName = splittedPossibleValue[0];  // Name must be "WO"

            if (splittedPossibleValue.Length < 2) // Have No Value
            {
                if (bt.BitLength == 1)
                {
                    return "Set";
                }
                else
                {
                    return "Write";
                }
            }
            else  // Have Value
            {
                var possibleValueValue = splittedPossibleValue[1];
                if (bt.BitLength == 1)
                {
                    if (possibleValueValue == "0b1")
                    {
                        return "Set";
                    }
                    else // "0b0"
                    {
                        return "Reset";
                    }
                }
                else
                {
                    return "Write " + possibleValueValue;
                }
            }
        }

        private static string GetBitButtonContent_MPQ78000FS(BitControl bt)
        {
            if (bt.BitName == "WRITE_TRANSMIT_DATA")
            {
                return "Write";
            }
            else if (bt.BitName == "READ_RECEIVE_DATA")
            {
                return "Read";
            }
            else
            {
                return GetBitButtonContent(bt);
            }
        }
    }

    public class OverViewFeaturesMinWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 500;
            else
                return 350;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region Detailed Register Map Converters


    public class DetailRegMap_BitRangeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int position = System.Convert.ToInt32(values[0]);
            int bitlength = System.Convert.ToInt32(values[1]);
            if (bitlength == 1)
            {
                return string.Format("[{0}]", position);
            }
            else
            {
                return string.Format("[{0}:{1}]", position + bitlength - 1, position);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DetailRegMap_OperationEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var reg = value as Register;
            var operation = parameter as string;
            if (reg.Right == "RW")
            {
                return Visibility.Visible;
            }
            else if (reg.Right == "R")
            {
                if (operation == "Read")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else if (reg.Right == "W")
            {
                if (operation == "Read")
                {
                    return Visibility.Collapsed;
                }
                
                //Write button
                if (reg.BitControls.All(o => o.Access == "Send byte"))   //Send byte
                {
                    return Visibility.Collapsed;
                }
                else  //normal W
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DetailRegMap_BitValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var reg = values[0] as Register;
            string binstr;
            if (reg.DataType == "Byte")
            {
                binstr = System.Convert.ToString(reg.Value, 2).PadLeft(8, '0');
            }
            else if (reg.DataType == "Word")
            {
                binstr = System.Convert.ToString(reg.Value, 2).PadLeft(16, '0');
            }
            else //if (reg.DataType == "DoubleWord")
            {
                binstr = System.Convert.ToString(reg.Value, 2).PadLeft(32, '0');
            }

            uint regval = System.Convert.ToUInt16(reg.Value);
            int position = System.Convert.ToInt32(values[1]);
            int bitlength = System.Convert.ToInt32(values[2]);

            int startposition = binstr.Length - position - bitlength;
            string res = binstr.Substring(startposition, bitlength);
            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DetailRegMap_BitBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentBitValue = values[0].ToString();
            var sourceBitValue = values[1].ToString();
            bool isEnable;
            if (values[2] == DependencyProperty.UnsetValue)
                isEnable = false;
            else
                isEnable = (bool)(values[2]);
            if (!isEnable)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1e7ef"));
            }
            else
            {
                if (currentBitValue != sourceBitValue)
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C5F1FF"));
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowDetailedRegisterMapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), (value as Chip).ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MPM3695_10:
                case EnumPartnumber.MPM3695GMJ_15:
                case EnumPartnumber.MPM3695_20:
                case EnumPartnumber.MPM54304:
                case EnumPartnumber.MP8796B:
                case EnumPartnumber.MPM82504:
                case EnumPartnumber.MPM3695L_20:
                case EnumPartnumber.mEZDPD1620A:
                case EnumPartnumber.mEZDPD1620AS:
                case EnumPartnumber.MPM3695_25:
                case EnumPartnumber.MPM3695_100:
                case EnumPartnumber.MPM3690_50D:
                case EnumPartnumber.MPM3895_25:
                case EnumPartnumber.MPM3690S_15:
                case EnumPartnumber.MP8796K:
                case EnumPartnumber.MP9795B:
                case EnumPartnumber.CDM1680:
                case EnumPartnumber.MP4246:
                case EnumPartnumber.MP9477B:
                case EnumPartnumber.MP9107:
                case EnumPartnumber.MPQ4237GUTE:
                case EnumPartnumber.MP8833B:
                case EnumPartnumber.MP8860:
                case EnumPartnumber.MP8887:
                    return Visibility.Visible;
                default:
                    return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
