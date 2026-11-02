using MPS.Common.Command;
using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace VirtualBenchPro_v4.Models
{
    public class ScanDataModel : ObservableObject
    {
        #region Properties
        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(nameof(Address)); }
        }

        private List<string> _devices;

        public List<string> Devices
        {
            get { return _devices; }
            set { _devices = value; RaisePropertyChanged(nameof(Devices)); }
        }

        private string _selectedDevice;

        public string SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; RaisePropertyChanged(nameof(SelectedDevice)); }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(nameof(IsSelected)); }
        }

        private bool _isAutoScan;

        public bool IsAutoScan
        {
            get { return _isAutoScan; }
            set { _isAutoScan = value; RaisePropertyChanged(nameof(IsAutoScan)); }
        }


        private bool _isChipEnabled;

        public bool IsChipEnabled
        {
            get { return _isChipEnabled; }
            set { _isChipEnabled = value; RaisePropertyChanged(nameof(IsChipEnabled)); }
        }
        #endregion

        public ScanDataModel(string _address=null, string _selectedDevice=null, bool _isSelected=false, bool _isAutoScan=false,bool _isChipEnabled=true, List<string> _devices=null)
        {
            Address = _address;
            SelectedDevice = _selectedDevice;
            IsSelected = _isSelected;
            IsAutoScan = _isAutoScan;
            IsChipEnabled = _isChipEnabled;
            Devices = _devices;



        }
    }
}
