using MPS.Common;
using MPS.Common.Model;
using MPS.Instrument;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4.ViewModel
{
    class AutoScanSelectView : ObservableObject
    {
        public AutoScanSelectView( ObservableCollection<ChipItem> chiplist)
        {
            ChipList = chiplist;
        }

        private ObservableCollection<ChipItem> _ChipList;

        public ObservableCollection<ChipItem> ChipList
        {
            get
            {
                return _ChipList;
            }
            set
            {
                _ChipList = value;
                RaisePropertyChanged("ChipList");
            }
        }
        
    }

    public class ChipItem : ObservableObject
    {
        public ChipItem(string chipName = null, string chipAddr = null, bool flag = false)
        {
            ChipName = chipName;
            ChipAddr = chipAddr;
            IsSelected = flag;
        }

        private string _ChipName;

        public string ChipName
        {
            get { return _ChipName; }
            set
            {
                _ChipName = value;
                RaisePropertyChanged("ChipName");
            }
        }

        private string _chipaddr;

        public string ChipAddr
        {
            get { return _chipaddr; }
            set 
            { 
                _chipaddr = value;
                RaisePropertyChanged("ChipAddr");
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



    }
}
