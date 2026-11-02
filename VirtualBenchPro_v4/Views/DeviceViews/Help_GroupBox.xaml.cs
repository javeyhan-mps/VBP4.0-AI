using MPS.Common.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace VirtualBenchPro_v4.Views.CustomElements
{
	/// <summary>
	/// Interaction logic for Help_GroupBox.xaml
	/// </summary>
	public partial class Help_GroupBox : UserControl
	{
		private Frame Parent_View;
		private Help_Window Visual_Parent;
		private double InitWidth = 0;
		private double InitHigh = 0;
		private Point InitPosition = new Point();
        //private BitmapImage Image_File;
        private Dictionary<string, BitmapImage> Image_Dic = new Dictionary<string, BitmapImage>();
		public MPS.Common.Sqlite.HelpGroup List_Items { get; set; }
		public Help_GroupBox(HelpGroup list_items, Frame parent, Help_Window visual_parent)
		{
			InitializeComponent();
			List_Items = list_items;
			Visual_Parent = visual_parent;
			InitItems(list_items);
			Parent_View = parent;
		}

		private void InitItems(HelpGroup items)
		{
			if (items.GroupTitle != "")
				SubGroup_Name.Content = items.GroupTitle;
			else
				SubGroup_Name.Visibility = Visibility.Collapsed;

			foreach (HelpInfo H in items.HelpInfoList)
			{
                if (H.Image == null)
                {
                    continue;
                }
                //Image_File = Bytes2BitmapImage(H.Image);

                Image_Dic.Add(H.Name, Bytes2BitmapImage(H.Image));
				StackPanel sp = new StackPanel { Orientation = Orientation.Horizontal };

				sp.Children.Add(new Image
				{
					Source = new BitmapImage(new Uri("pack://application:,,,/VirtualBenchPro_v4;component/Images/navigation-right@3x.png")),
					Width = 6,
					Margin = new Thickness(8, 0, 8, 0)
				});
				Label l = new Label
				{
					Content = H.Name,
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

        public static BitmapImage Bytes2BitmapImage(byte[] bytearray)
        {
            BitmapImage bmp = null;

            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(bytearray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }

            return bmp;
        }

        private void ShowImage(object sender, MouseButtonEventArgs e)
		{
			Page p = new Page();
			Visual_Parent.isShow = true;
			Label element_clicked = (Label)sender;
			Grid parent = (Grid)((WrapPanel)((StackPanel)element_clicked.Parent).Parent).Parent;
			IEnumerable<Label> collection = parent.Children.OfType<Label>();
			WrapPanel wp = new WrapPanel { Orientation = Orientation.Horizontal };
			StackPanel sp = new StackPanel
			{
				Name = "Index_View",
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(0, 15, 0, 15)
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
					Cursor = Cursors.Hand,
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
				lb.MouseLeftButtonUp += new MouseButtonEventHandler(GoMiddle);

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
            BitmapImage img;
            var hasImg = Image_Dic.TryGetValue(element_clicked.Content.ToString(), out img);
            img = hasImg ? img : new BitmapImage();
			Image im = new Image
			{
				//Source = Image_File,
				Source = img,
                RenderTransform = new ScaleTransform(0.9, 0.9),
                Margin = new Thickness(35, 0, 0, 0)
            };
			//End

			wp.Children.Add(sp);
			wp.Children.Add(im);

			p.Content = wp;
			Parent_View.Content = p;
		}
		//WrapPanel _wp;
		//Image im;
		//This function is to be called when there's only one help
		public WrapPanel GetImageView(WrapPanel wp)
		{
			//wp = new WrapPanel { Orientation = Orientation.Vertical, Background=Brushes.Red };
			
			BitmapImage img;
			var hasImg = Image_Dic.TryGetValue(List_Items.HelpInfoList[0].Name, out img);
			img = hasImg ? img : new BitmapImage();
			Image im = new Image
			{
				Source = img,
				Margin = new Thickness(5, 0, 5, 0)
			};
            //im.MouseWheel += Im_MouseWheel;
            //im.MouseEnter += Im_MouseEnter;
            //im.MouseMove += Im_MouseMove;
            //im.MouseLeftButtonDown += Im_MouseLeftButtonDown; ; ;
            //im.MouseLeftButtonUp += Im_MouseLeftButtonUp; ;
            //im.MouseRightButtonDown += Im_MouseRightButtonDown;
			wp.Children.Add(im);
			//_wp = wp;
			return wp;
			
		}
        #region Mouse Wheel
        //      private void Im_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //      {
        //	var image = sender as Image;
        //	isLeftMouseDown = false;
        //	image.ReleaseMouseCapture();
        //}

        //      private void Im_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //      {
        //	var image = sender as Image;
        //	var transform = image.RenderTransform as TranslateTransform;
        //	isLeftMouseDown = true;
        //	mouseDownPosition = e.GetPosition(image);
        //	if (transform == null)
        //	{
        //		transform = new TranslateTransform();
        //		image.RenderTransform = transform;
        //	}
        //	mouseDownControlPosition = new Point(transform.X, transform.Y);
        //	image.CaptureMouse();
        //}

        //      private void Im_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //      {
        //	if (InitWidth > 0 && InitHigh > 0)
        //	{
        //		_wp.Width = InitWidth;
        //		_wp.Height = InitHigh;
        //	}
        //}

        //private bool isStart = true;
        //private bool isLeftMouseDown = false;
        //private Point mouseDownPosition;
        //private Point mouseDownControlPosition;

        //      private void Im_MouseMove(object sender, MouseEventArgs e)
        //      {
        //	var image = sender as Image;
        //	var pos = e.GetPosition(image);
        //	var dp = pos - mouseDownPosition;
        //	var transform = image.RenderTransform as TranslateTransform;
        //	if (isLeftMouseDown)
        //	{
        //		transform.X = mouseDownControlPosition.X + dp.X;
        //		transform.Y = mouseDownControlPosition.Y + dp.Y;
        //	}
        //      }

        //      private void Im_MouseEnter(object sender, MouseEventArgs e)
        //      {
        //	im.Focus();
        //      }
        //      private void Im_MouseWheel(object sender, MouseWheelEventArgs e)
        //      {
        //	if (isStart)
        //	{
        //		InitWidth = _wp.ActualWidth;
        //		InitHigh = _wp.ActualHeight;
        //		isStart = false;
        //	}
        //	if (e.Delta >= 0)
        //          {
        //              _wp.Width = _wp.ActualWidth * 1.1;
        //              _wp.Height = _wp.ActualHeight * 1.1;
        //          }
        //          else
        //          {
        //              _wp.Width = _wp.ActualWidth * 0.9;
        //              _wp.Height = _wp.ActualHeight * 0.9;
        //          }
        //      }
        #endregion
        private void GoBack(object sender, MouseButtonEventArgs e)
		{
			Visual_Parent.isShow = false;
			Visual_Parent.GoHome();
		}

		private void GoMiddle(object sender, MouseButtonEventArgs e)
		{
			WrapPanel wp = new WrapPanel();
			Visual_Parent.isShow = false;
			Label element_clicked = (Label)sender;
			StackPanel sp = new StackPanel
			{
				Name = "Index_View",
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(0, 15, 0, 15)
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
			
			Label lb = new Label
			{
				Content = element_clicked.Content,
				Cursor = Cursors.Hand,
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1C4EFF")),
				Margin = new Thickness(2, 0, 0, 0)
			};
			sp.Children.Add(lb);
				

			wp.Children.Add(sp);
			wp.Children.Add(new Help_GroupBox(List_Items, Parent_View, Visual_Parent));

			Parent_View.Content = wp;
		}

	}
}
