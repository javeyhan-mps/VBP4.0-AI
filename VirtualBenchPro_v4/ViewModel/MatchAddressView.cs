using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Common.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VirtualBenchPro_v4.Views.DeviceViews;

namespace VirtualBenchPro_v4.ViewModel
{
    public class MatchAddressView : ObservableObject
    {
        #region Attribute
        
        private MatchAddressWindow window { get; set; }
        public Chip chip { get; set; }

        private ObservableCollection<AddrVsROhms> _addrVsROhmsList;

        public ObservableCollection<AddrVsROhms> AddrVsROhmsList
        {
            get { return _addrVsROhmsList; }
            set
            {
                _addrVsROhmsList = value;
                RaisePropertyChanged("AddrVsROhmsList");
            }
        }
        private ObservableCollection<string> _addressList;

        public ObservableCollection<string> AddressList
        {
            get { return _addressList; }
            set
            {
                _addressList = value;
                RaisePropertyChanged("AddressList");
            }
        }

      


        private string _addressSelected;

        public string AddressSelected
        {
            get { return _addressSelected; }
            set
            {
                _addressSelected = value;
                RaisePropertyChanged("AddressSelected");
            }
        }


        private string _Register_Map_DataGrid_Address_Tile="Address";
        public string Register_Map_DataGrid_Address_Tile
        {
            get { return _Register_Map_DataGrid_Address_Tile; }
            set
            {
                _Register_Map_DataGrid_Address_Tile = value;
                RaisePropertyChanged("Register_Map_DataGrid_Address_Tile");
            }
        }
       

        private string _Register_Map_DataGrid_Address_Description = "Description";
        public string Register_Map_DataGrid_Address_Description
        {
            get { return _Register_Map_DataGrid_Address_Description; }
            set
            {
                _Register_Map_DataGrid_Address_Description = value;
                RaisePropertyChanged("Register_Map_DataGrid_Address_Description");
            }
        }

        //List<string> Register_Map_DataGrid_Address_Description_PartList = new List<string>() {"MP3921" };


        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        private string _AddressList_Title;

        public string AddressList_Title
        {
            get { return _AddressList_Title; }
            set
            {
                _AddressList_Title = value;
                RaisePropertyChanged("AddressList_Title");
            }
        }

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }


        //Added properties for UART communication
        private Visibility _NoCOMFound_Visibility;

        public Visibility NoCOMFound_Visibility
        {
            get { return _NoCOMFound_Visibility; }
            set
            {
                _NoCOMFound_Visibility = value;
                RaisePropertyChanged("NoCOMFound_Visibility");
            }
        }

        private Visibility _Normal_Visibility;

        public Visibility Normal_Visibility
        {
            get { return _Normal_Visibility; }
            set
            {
                _Normal_Visibility = value;
                RaisePropertyChanged("Normal_Visibility");
            }
        }

        

        #endregion

        public List<AddrVsROhms> Public_Addr_ROhms_List = new List<ViewModel.AddrVsROhms>();

        public MatchAddressView(Chip chip)
        {
            this.chip = chip; 
            try
            {
             
                _description = ConstantModel.sqlite.GetHelpStringId(chip.KeyAddressdescription);
                var addressConfigeration = ConstantModel.sqlite.GetHelpStringId(chip.KeyAddressConfigeration);
                List<AddrVsROhms> Addr_ROhms_List = new List<ViewModel.AddrVsROhms>();

                if (addressConfigeration == "COM")  //via COM port
                {
                    //var coms = SerialPort.GetPortNames();
                    var coms = GetSerialPortDevices();
                    if (coms.Count() > 0)
                    {
                        AddressList_Title = "CONNECTED COM PORTS";
                        for (int i = 0; i < coms.Count(); i++)
                        {
                            Addr_ROhms_List.Add(new ViewModel.AddrVsROhms() { Address = coms[i], Rohms = "Select dongle serial COM port" });
                        }
                    }
                    else
                    {
                        AddressSelected = "COMx";
                        NoCOMFound_Visibility = Visibility.Visible;
                        Normal_Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                else
                {
                     AddressList_Title = "SUPPORTED ADDRESS VALUES";

                    if (addressConfigeration.Contains("[I2C]") || addressConfigeration.Contains("[SPI]") || addressConfigeration.Contains("[UART]"))
                    {
                        var Temp_List = new List<AddrVsROhms>();

                        Public_Addr_ROhms_List = GetAddressData(chip.KeyAddressConfigeration, false);

                        foreach (var data in Public_Addr_ROhms_List)
                        {
                            if (!Temp_List.Any(o=>o.Address.Contains(data.Protocol)))
                            {
                                Temp_List.Add(new ViewModel.AddrVsROhms() { Address = data.Protocol, Rohms = data.Rohms });
                            }
                        }

                        Addr_ROhms_List = Temp_List;
                    }
                    else 
                    {
                       
                        Addr_ROhms_List = GetAddressData(chip.KeyAddressConfigeration);

                    }

                  
                }

                AddrVsROhmsList = new ObservableCollection<AddrVsROhms>(Addr_ROhms_List);
                AddressList = new ObservableCollection<string>(AddrVsROhmsList.Select(o => o.Address.Trim()));
                Register_Map_DataGrid_Address_Tile = Public_Addr_ROhms_List.Count!=0? "Protocol": Register_Map_DataGrid_Address_Tile;
                //Register_Map_DataGrid_Address_Description = Register_Map_DataGrid_Address_Description_PartList.Contains(chip.ChipName) ? "Description" : Register_Map_DataGrid_Address_Description;
                NoCOMFound_Visibility = Visibility.Collapsed;
                Normal_Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string[] GetSerialPortDevices()
        {
            var search = new ManagementObjectSearcher(@"root\cimv2", @"SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%CH340%' or Name LIKE '%cp210%' or Name LIKE '%(COM%)'");
            var devs = search.Get();
            List<string> comNmes = new List<string>();
            foreach (var dev in devs)
            {
                var com = dev.GetPropertyValue("Name").ToString().Split(new char[] { '(', ')' })[1];
                comNmes.Add(com);
            }
            return comNmes.ToArray();

        }

        private List<AddrVsROhms> GetAddressData(string StringId,bool IsNormal=true)
        {
            List<AddrVsROhms> AddrVsROhmsList = new List<AddrVsROhms>();

            string Address_Data = ConstantModel.sqlite.GetHelpStringId(StringId);

            if (IsNormal)
            {
                var addresLst = Address_Data.Split(';');

                foreach (var item in addresLst)
                {
                    var add = item.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                    if (add.Length < 2)
                        continue;

                    add[0] = add[0].Replace(',', ' ').Trim();
                    add[0] = add[0].Replace('(', ' ').Trim();
                    add[0] = add[0].Replace(')', ' ').Trim();
                    if (string.IsNullOrEmpty(add[0])) continue;
                    AddrVsROhmsList.Add(new ViewModel.AddrVsROhms() { Address = add[0], Rohms = add[1] });
                }

            }
            else 
            {

                string[] sections = Address_Data.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < sections.Length; i += 2)
                {
                    string protocol = sections[i].Trim(); // Either "I2C" or "UART"
                    string data = sections[i + 1].Replace('(', ' ').Replace(')', ' ').Trim();
                 
                    // Split the data entries
                    string[] entries = data.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (entries.Count() != 0)
                    {
                        foreach (string entry in entries)
                        {
                            string trimmedEntry = entry.Trim();
                            if (string.IsNullOrEmpty(trimmedEntry)) continue;

                            // Split each entry into Address and Rohms
                            int commaIndex = trimmedEntry.IndexOf(',');
                            if (commaIndex == -1) continue;

                            string address = trimmedEntry.Substring(0, commaIndex).Trim();
                            string rohms = trimmedEntry
                                .Substring(commaIndex + 1)
                                .Trim(new[] { '{', '}', ' ' });

                            AddrVsROhmsList.Add(new AddrVsROhms
                            {
                                Address = address,
                                Rohms = rohms,
                                Protocol = protocol
                            });
                        }
                    }
                    else 
                    {
                        AddrVsROhmsList.Add(new AddrVsROhms
                        {
                            Address = "",
                            Rohms = "Set by "+ protocol,
                            Protocol = protocol
                        });
                    }

                   
                }
            }

            return AddrVsROhmsList;

        }
    }

    public class AddrVsROhms
    {
        public string Rohms { get; set; }

        public string Address { get; set; }

        public string Protocol { get; set; }
    }
}
