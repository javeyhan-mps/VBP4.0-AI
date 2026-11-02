using System;
using System.Windows.Input;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for ChangeChipAddress.xaml
    /// </summary>
    public partial class ChangeChipAddress
    {
        public ChangeChipAddress()
        {
            InitializeComponent();
            this.txtAddress.Focus();
        }

        public byte ChangedAddress { get; set; }
        private void Change_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            byte addr;

            if (!string.IsNullOrEmpty(this.txtAddress.Text) && IsNumberic(this.txtAddress.Text, out addr))
            {
                e.CanExecute = addr <= 0x7f;
            }

            else
                e.CanExecute = false;

        }

        private void Change_Execute(object sender, ExecutedRoutedEventArgs e)
        {

            ChangedAddress = Convert.ToByte(this.txtAddress.Text, 16);
            this.DialogResult = true;
        }

        private bool IsNumberic(string str, out byte result)
        {
            try
            {
                result = Convert.ToByte(str, 16);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }

        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            //if (string.IsNullOrEmpty(this.txtAddress.Text) || e.Key != Key.Enter)
            //    return;
            byte addr;
            if (!string.IsNullOrEmpty(this.txtAddress.Text) && IsNumberic(this.txtAddress.Text, out addr) && e.Key == Key.Enter)
            {
                ChangedAddress = Convert.ToByte(this.txtAddress.Text, 16);
                this.DialogResult = true;
            }

        }
    }
}

