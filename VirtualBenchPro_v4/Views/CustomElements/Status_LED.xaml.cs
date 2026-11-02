using System.Windows.Controls;
using System.Windows.Media;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    public partial class Status_LED : UserControl
    {
        private readonly string COLOR_ON = "#FF34D08C";
        private readonly string COLOR_OFF = "#FFD8D8D8";
        private readonly string COLOR_ERROR = "#FFEF4055";

        private byte _status;
        public byte STATUS
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;

                if (value == 0)
                {
                    LED.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(COLOR_OFF));
                    LED.ToolTip = "Offline";
                }
                else if (value == 1)
                {
                    LED.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(COLOR_ON));
                    LED.ToolTip = "Online";

                }
                else if (value == 2)
                {
                    LED.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(COLOR_ERROR));
                    LED.ToolTip = "Error";

                }

            }
        }


        public Status_LED()
        {
            InitializeComponent();
            STATUS = 0;
        }

        public Status_LED(byte s)
        {
            InitializeComponent();
            STATUS = s;
        }
    }
}
