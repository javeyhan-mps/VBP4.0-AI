using MPS.Common.Command;
using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4.ViewModel
{
    class SetFriendlyNameWindowViewModel : ObservableObject
    {
        private Chip currentChip;
        public SetFriendlyNameWindowViewModel(Chip chip)
        {
            currentChip = chip;
            FriendlyName = currentChip.FriendlyName;
        }

        public string FriendlyName { get; set; }

        public ICommand SetFriendlyNameComand => new RelayCommand<Window>(SetFriendlyNameMethod);

        public void SetFriendlyNameMethod(Window window)
        {
            currentChip.FriendlyName = FriendlyName;
            window.DialogResult = true;
        }
    }
}
