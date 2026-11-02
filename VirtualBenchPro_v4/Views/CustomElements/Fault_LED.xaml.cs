using System.Windows.Controls;
using System.Windows.Media;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    public partial class Fault_LED : UserControl
    {

        private readonly string NO_ERROR = "#FF34D08C";
        private readonly string ERROR = "#FFEF4055";

        private bool _status;
        public bool STATUS
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;

                if (value)
                {
                    LED.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ERROR));
                    LED.ToolTip = "Error";
                }
                else
                {
                    LED.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(NO_ERROR));
                    LED.ToolTip = "OK";
                }

            }
        }

        public Fault_LED()
        {
            InitializeComponent();
            STATUS = false;
        }

        public Fault_LED(bool b)
        {
            InitializeComponent();
            STATUS = b;
        }
    }
}
