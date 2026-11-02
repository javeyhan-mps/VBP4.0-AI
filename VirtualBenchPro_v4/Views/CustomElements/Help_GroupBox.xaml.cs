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
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4.Views.CustomElements
{
	/// <summary>
	/// Interaction logic for Help_GroupBox.xaml
	/// </summary>
	public partial class Help_GroupBox : UserControl
	{
		private Frame Parent_View;
		private Image Image_File;
		
		public Help_GroupBox(Help_Group list_items, Frame parent)
		{
			InitializeComponent();
			InitItems(list_items);
			Parent_View = parent;
		}

		private void InitItems(Help_Group items)
		{
			if (items.Help_Group_Title != "")
				SubGroup_Name.Content = items.Help_Group_Title;
			else
				SubGroup_Name.Visibility = Visibility.Collapsed;

			foreach (Help H in items.Help_List)
			{
				Image_File = H.Help_Image;
				StackPanel sp = new StackPanel { Orientation = Orientation.Horizontal };

				sp.Children.Add(new Image
				{
					Source = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/navigation-right@3x.png")),
					Width = 6,
					Margin = new Thickness(8, 0, 8, 0)
				});
				Label l = new Label
				{
					Content = H.Help_Name,
					FontFamily = new FontFamily("Arial"),
					FontSize = 14,
					Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF")),
					Cursor = Cursors.Hand
				};
				l.MouseLeftButtonUp += new MouseButtonEventHandler(ShowImage);

				sp.Children.Add(l);

				Content.Children.Add(sp);
			}


		}

		private void ShowImage(object sender, MouseButtonEventArgs e)
		{
			Page p = new Page();
			Label element_clicked = (Label)sender;
			Grid parent = (Grid)(((WrapPanel)((StackPanel)element_clicked.Parent).Parent).Parent);
			IEnumerable<Label> collection = parent.Children.OfType<Label>();
			WrapPanel wp = new WrapPanel { Orientation = Orientation.Vertical };
			StackPanel sp = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(0, 23, 0, 30)
			};

			//All this is to generate a new page dynamically
			Label la = new Label
			{
				Content = "Index",
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF")),
				Cursor = Cursors.Hand,
				Margin = new Thickness(5, 0, 0, 0)
			};
			la.MouseLeftButtonUp += new MouseButtonEventHandler(GoBack);
			sp.Children.Add(la);
			sp.Children.Add(new Image
			{
				Source = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/navigation-right@3x.png")),
				Width = 6,
				Margin = new Thickness(2, 0, 0, 0)
			});
			if (collection.First().Content.ToString() != "")
			{
				Label lb = new Label
				{
					Content = collection.First().Content,
					FontFamily = new FontFamily("Arial"),
					FontSize = 12,
					Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF")),
					Margin = new Thickness(2, 0, 0, 0)
				};
				sp.Children.Add(lb);
				sp.Children.Add(new Image
				{
					Source = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/navigation-right@3x.png")),
					Width = 6,
					Margin = new Thickness(2, 0, 0, 0)
				});
			}
			Label lc = new Label
			{
				Content = element_clicked.Content,
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF496180")),
				Margin = new Thickness(2, 0, 0, 0)
			};
			sp.Children.Add(lc);
			Image im = new Image
			{
				//Source needs to be "Source = image_file", this one is only an example
				Source = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/Placeholder.png")),
				Margin = new Thickness(15, 0, 15, 0)
			};
			//End

			wp.Children.Add(sp);
			wp.Children.Add(im);

			p.Content = wp;
			Parent_View.Content = p;
		}

		private void GoBack(object sender, MouseButtonEventArgs e)
		{
			Parent_View.GoBack();
		}

	}
}
