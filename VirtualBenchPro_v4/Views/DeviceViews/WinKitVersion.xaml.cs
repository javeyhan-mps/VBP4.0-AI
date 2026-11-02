using MPS.Instrument;
using System;
using System.Collections.Generic;
using System.IO;
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
using VirtualBenchPro_v4.Views.CustomElements;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for WinKitVersion.xaml
    /// </summary>
    public partial class WinKitVersion : Window
    {
        private string Kit_Version;
        private byte FW_Revision;
        private string hex_filepath;
        private byte latest_Fw_Version;

        public WinKitVersion(string Kit_Ver, byte FW_Rev)
        {
            InitializeComponent();
            Kit_Version = Kit_Ver;
            FW_Revision = FW_Rev;
            updateWinContent();

        }

        private void updateWinContent()
        {
            /*----------------------- EVKT-USBI2C-02 (Rev. 0x00-0x6F) -------------------------------------*/
            if (Kit_Version != null && FW_Revision >= ConstantModel.Min_USBI2C_FwRev && FW_Revision <= ConstantModel.Latest_USBI2C_FwRev)
            {
                hex_filepath = ConstantModel.Latest_USBI2C_FwFilePath;
                latest_Fw_Version = ConstantModel.Latest_USBI2C_FwRev;

                kit_type.Text = "USBI2C";
                kit_version.Text = Kit_Version;
                Kit_Version_Label.Visibility = Visibility.Visible;
                fw_reversion.Text = "Rev" + FW_Revision;

                if (FW_Revision >= 24 && FW_Revision < latest_Fw_Version)  //Only >=Rev24 support dongle update funciton
                {
                    new_fw_reversion.Text = "Rev" + latest_Fw_Version;
                    update_msg.Visibility = Visibility.Visible;
                    latestfw_msg.Visibility = Visibility.Collapsed;
                }
                else
                {
                    update_msg.Visibility = Visibility.Collapsed;
                    latestfw_msg.Visibility = Visibility.Visible;
                }
                force_update_msg.Visibility = Visibility.Collapsed;
            }
            /*----------------------- EVKT-AUTOMOTIVE-00A (Rev. 0x70-0xFF) -------------------------------------*/
            else if (FW_Revision >= ConstantModel.Min_AUTOMOTIVE_FwRev && FW_Revision <= ConstantModel.Latest_AUTOMOTIVE_FwRev)
            {
                kit_type.Text = "AUTOMOTIVE";
                Kit_Version_Label.Visibility = Visibility.Collapsed;
                fw_reversion.Text = "Rev" + FW_Revision;

                hex_filepath = ConstantModel.Latest_AUTOMOTIVE_FwFilePath;
                latest_Fw_Version = ConstantModel.Latest_AUTOMOTIVE_FwRev;

                if (FW_Revision < latest_Fw_Version)
                {
                    new_fw_reversion.Text = "Rev" + latest_Fw_Version;
                    update_msg.Visibility = Visibility.Visible;
                    latestfw_msg.Visibility = Visibility.Collapsed;
                }
                else
                {
                    update_msg.Visibility = Visibility.Collapsed;
                    latestfw_msg.Visibility = Visibility.Visible;
                }
                force_update_msg.Visibility = Visibility.Collapsed;
            }
            else
            {
                kit_type.Text = "Unknown";
                kit_version.Text = "Unknown";
                fw_reversion.Text = "Rev" + FW_Revision;
                update_msg.Visibility = Visibility.Collapsed;
                latestfw_msg.Visibility = Visibility.Collapsed;
                force_update_msg.Visibility = Visibility.Visible;
            }
            
        }

        private void FW_Update_Button_Click(object sender, RoutedEventArgs e)
        {
            FW_Update(hex_filepath, latest_Fw_Version);
        }

        private void USBI2C_FW_Update_Button_Click(object sender, RoutedEventArgs e)
        {
            FW_Update(ConstantModel.Latest_USBI2C_FwFilePath, ConstantModel.Latest_USBI2C_FwRev);
        }

        private void Automotive_FW_Update_Button_Click(object sender, RoutedEventArgs e)
        {
            FW_Update(ConstantModel.Latest_AUTOMOTIVE_FwFilePath, ConstantModel.Latest_AUTOMOTIVE_FwRev);
        }

        private void FW_Update(string update_filepath, byte target_FW_Rev)
        {
            try
            {
                BusyIndicator.LoadingIndicator.LoadingUIRun1("Kit Update");

                //check file and decrypt
                if (!File.Exists(update_filepath))
                {
                    throw new FileLoadException("Dongle firmware is missing.");
                }
                string decryptedFile = "decrypted.hex";
                if (!HexFileCrypto.DecryptHexFile(update_filepath, decryptedFile, HexFileCrypto.password))
                {
                    throw new FileLoadException("Dongle firmware is corrupted.");
                }

                DeviceObject.Devices.PMBus.UpdateDeviceFirmware(decryptedFile);
                System.Threading.Thread.Sleep(1000);

                Kit_Version = DeviceObject.Devices.PMBus.CheckKitVersion();
                FW_Revision = DeviceObject.Devices.PMBus.CheckKitType();
                //Kit_Version = CheckKitVersion(FW_Revision);

                if (FW_Revision != target_FW_Rev)
                {
                    throw new FileLoadException("Program done but firmware version check failed. Please try again.");
                }

                BusyIndicator.LoadingIndicator.LoadingUIStop();
                //var ui = new WinDone();
                //ui.ShowDialog();
            }
            catch (Exception ex)
            {
                BusyIndicator.LoadingIndicator.LoadingUIStop();
                if (ex is FileLoadException)
                {
                    var ui = new WinError(string.Format("Error: {0}", ex.Message));
                    ui.ShowDialog();
                }
                else
                {
                    var ui = new WinError(string.Format("Kit update failed.\r\nPlease check the kit connection and make sure your dongle meets the upgrade requirements."));
                    ui.ShowDialog();
                }
            }
            finally
            {
                updateWinContent();
            }
        }

        //private string CheckKitVersion(byte fw_rev)
        //{
        //    string kit_ver;
        //    if (fw_rev == 0x03)     //R2 dongleVersion=0x03
        //    {
        //        kit_ver = "R2";
        //    }
        //    else if (fw_rev == 0x05)  //R5 dongleVersion=0x05
        //    {
        //        kit_ver = "R5";
        //    }
        //    else if (fw_rev == 0x0B)   //R6 dongleVersion=0x0b
        //    {
        //        kit_ver = "R6";
        //    }
        //    else if (fw_rev >= 0x18 && fw_rev < 0x1C)   //R7 24
        //    {
        //        kit_ver = "R7";
        //    }
        //    else if (fw_rev == 0x1C)   //R8 28(0x1C)
        //    {
        //        kit_ver = "R8";
        //    }
        //    else
        //    {
        //        kit_ver = null;
        //    }
        //    return kit_ver;
        //}
    }
}
