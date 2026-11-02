using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.Views.CustomElements
{
    /// <summary>
    /// Interaction logic for TabItem_Button.xaml
    /// </summary>
    public partial class TabItem_Button : UserControl
    {
        public TabItem_Button()
        {
            InitializeComponent();
            DataContext = this;
        }

        public TabItem_Button(SubCategory subCategory)
        {
            InitializeComponent();
            DataContext = this;

            ContentSite.Content = subCategory.Name;
            ContentSite.Padding = new Thickness(0);
            if (subCategory.InfoSubCategory != "")
            {
                Help_Icon h = new Help_Icon(true, subCategory.InfoSubCategory)
                {
                    Width = 12,
                    Height = 12,
                    Margin = new Thickness(5,0,0,0)
                };

                Header_Content.Children.Add(h);
            }


        }

        public TabItem_Button(Category category)
        {
            InitializeComponent();
            DataContext = this;

            ContentSite.Content = category.Name;
            ContentSite.Padding = new Thickness(0, 5, 0, 5);
            //if (category.InfoCategory != "")
            //{
                Help_Icon h = new Help_Icon(true, category.InfoCategory)
                {
                    Width = 12,
                    Height = 12,
                    Margin = new Thickness(5, 0, 0, 0)

                };
                Header_Content.Children.Add(h);
            //}
        }
    }
}
