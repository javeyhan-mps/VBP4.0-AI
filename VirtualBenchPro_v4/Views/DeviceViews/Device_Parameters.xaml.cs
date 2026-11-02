using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for Device_Parameters.xaml
    /// </summary>
    public partial class Device_Parameters : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<ObservablePoint> Data { get; set; }
        public short Ton_Max { get; set; } = 200;
        public short Toff_Max { get; set; } = 200;

        public Device_Parameters()
        {
            InitializeComponent();
            DataContext = this;

            


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 2",
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF979797")),
                    Values = Data,
                    LineSmoothness = 0,
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF979797")),
                    Values = new ChartValues<short> { 1, 1, 6, 6, 1, 1 },
                    LineSmoothness = 0,
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF979797")),
                    Values = new ChartValues<short> { 3, 3, 4, 4, 3, 3 },
                    LineSmoothness = 0,
                    PointGeometry = null
                }
            };
        }
    }
}
