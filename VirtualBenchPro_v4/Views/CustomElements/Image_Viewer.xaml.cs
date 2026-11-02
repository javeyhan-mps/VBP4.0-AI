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
using System.Windows.Shapes;

namespace VirtualBenchPro_v4.Views.CustomElements
{
	/// <summary>
	/// Lógica de interacción para Image_Viewer.xaml
	/// </summary>
	public partial class Image_Viewer : Window
	{
		public Image_Viewer(BitmapImage img)
		{
			InitializeComponent();
			Image_Preview.Source = img;
		}

		public Image_Viewer(Image img)
		{
			InitializeComponent();
			Image_Preview.Source = img.Source;
		}
	}
}
