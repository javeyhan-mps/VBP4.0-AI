using MPS.Common.Command;
using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4.ViewModel
{
    class ChooseChipWindowViewModel : ObservableObject
    {
        private LicenseInfo license;
        public ChooseChipWindowViewModel(LicenseInfo _license)
        {
            license = _license;
            FilteredChipList = new ObservableCollection<string>(ChipList);
        }
        private ObservableCollection<string> _ChipList;

        public ObservableCollection<string> ChipList
        {
            get
            {
                  //     _ChipList = new ObservableCollection<string>() {"MP4203" };

                //string dn = Environment.UserDomainName.ToLower();
                //if (dn.Contains("monolithicpower"))
                //{
                //_ChipList = new ObservableCollection<string>() { "MPQ8645P", "MP8796B", "MPM3695-10", "MPM3695-25", "mEZDPD1620A","MPQ4233","MPQ5864",
                //                                                 "mEZDPD1620AS","MPQ8883","MPQ8886", "mEZDPD4506A", "mEZDPD4506AS","MPQ7210", "MPQ8856",
                //                                                 "MPM3596","MP5470", "MPM54304","MP2772","MPQ70331FS","MPQ2286","MPQ8856","MP5516",
                //                                                 "MPM3690-50D", "MPM3698", "MPM54322", "MPF32010", "MPQ8880","MPQ2026A","MPQ7930",
                //                                                 "MPQ79700FS","MPQ79500FS", "MP5473", "MPQ4263","MPQ70160FS","MPQ74180FS","MP5055A",
                //                                                 "MPQ7920", "MPQ4262A","MP8870", "MP5477", "MPF52000","MPQ4234","MP4246","MPF52000-A",
                //                                                 "MPQ4214", "MP3924", "MTS1312","MP8826", "MPM54313","MPQ2977","MP3439","MPM38852",
                //                                                 "MPM54524", "MPQ8874","MPQ71000FS","MPQ8875A", "MP5413","MP5417","MP2421B","MP8833A",
                //                                                 "MPQ5857","MPQ5858","MPQ2023","MPQ2967","MPQ7795","MPQ7928","MPQ71000FS","MPQ70340FS",
                //                                                 "MP5424","MPQ7228","MPQ7225","MPQ4232","MPQ7241","MPQ70240FS","MPM4730","MPQ77240FS",
                //                                                 "MPQ7240", "MP8878" ,"MPQ2022","MPQ2022A", "MPQ2026", "MPQ2024","MPQ2024A","MP8976B",
                //                                                 "MP5518",  "MP5475","MPQ5856", "MPQ7210A", "MP2764","MP5513","MPM3599", "MPQ2283",
                //                                                 "CDM1680","MP9795B","MPQ2946","MPQ79505FS","MPM3895-25","MP2422B","MPQ2284","MPQ2285",
                //                                                 "MPQ2287","MPQ2288","MPM3593","MP5519","MPQ7929","MP2718"
                //                                                };//"mEZDPD3603A", "mEZDPD3603AS"
                //}
                //else
                //{
                //    _ChipList = new ObservableCollection<string>() { };
                //}

                if (license._chipList_internal != null && license._chipList_internal.Count > 0)
                {
                    _ChipList = license._chipList_internal;
                }
                else
                {
                    _ChipList = license._chipList_public;
                }


                return _ChipList;
            }
            set
            {
                _ChipList = value;
            }
        }

        private ObservableCollection<string> _filteredChipList;
        public ObservableCollection<string> FilteredChipList
        {
            get
            {
                return _filteredChipList;
            }
            set
            {
                _filteredChipList = value;
                RaisePropertyChanged(nameof(FilteredChipList));
            }
        }


        private string _SelectedChip;

        public string SelectedChip
        {
            get { return _SelectedChip; }
            set
            {
                _SelectedChip = value;
                RaisePropertyChanged("SelectedChip");
            }
        }

        public string SearchText { get; set; }

        public ICommand FilterChipComand => new RelayCommand(FilterChipMethod);

        public void FilterChipMethod()
        {
            SelectedChip = null;

            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredChipList = new ObservableCollection<string>(ChipList);
            }
            else
            {
                var filteredList = _ChipList.Where(c => c.ToLower().Contains(SearchText.ToLower())).OrderBy(c => c);
                FilteredChipList = new ObservableCollection<string>(filteredList);
            }
        }
    }
}
