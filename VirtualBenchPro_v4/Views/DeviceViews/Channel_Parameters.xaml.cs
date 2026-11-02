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
using VirtualBenchPro_v4.Views.CustomElements;

namespace VirtualBenchPro_v4.Views.DeviceViews
{
    /// <summary>
    /// Interaction logic for Channel_Parameters.xaml
    /// </summary>
    public partial class Channel_Parameters : Page
    {
        public Channel_Parameters()
        {
            InitializeComponent();
            DataContext = this;

            //foreach channel - Is there a way to know exactly the total number of channels??
            for(int i = 0; i < 4; i++)
            {
                Channel_Parameters_Channels.Children.Add(new Module_Button("CH " + i, false, 1));
            }

        }

        public void Load(List<Chip> chips, List<Category> categories) 
        {
            List<Chip> ch = chips;
            List<Category> cat = categories;

            foreach(Category c in cat)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Category: " + c.Name);
                TabItem category_tab = new TabItem
                {
                    Header = new TabItem_Button(c),
                    Style = Application.Current.FindResource("TabItem_HeaderFlat") as Style
                };

                WrapPanel category_tab_content = new WrapPanel();
                TabControl Subcategory_TabControl = new TabControl
                {
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(-2, 5,-2, 0)
                };

                foreach (SubCategory s in c.SubCategories)
                {

                    TabItem subcategory_tab = new TabItem
                    {
                        Header = new TabItem_Button(s),
                        Style = Application.Current.FindResource("TabItem_HeaderButton") as Style
                    };
                    WrapPanel subcategory_tab_content = new WrapPanel();
                    
                    Console.WriteLine("-SubCategory: " + s.Name);
                    if(s.Name == "Temperature")
                        Console.WriteLine("-SubCategory: " + s.Name);

                    foreach (ParameterGroup p in s.ParametersGroups)
                    {
                        Console.WriteLine("--ParameterGroup: " + p.Name);
                        Subcategory_GroupBox sub_box;
                        if (p.InfoParameterGroup == "")
                            sub_box = new Subcategory_GroupBox(p.Name); 
                        else
                            sub_box = new Subcategory_GroupBox(p.Name, p.InfoParameterGroup);

                        foreach (Parameter pa in p.Parameters)
                        {
                            Console.WriteLine("---Parameter: " + pa.Name);

                            foreach (Chip chp in ch)
                            {
                                foreach (Register r in chp.Registers)
                                {
                                    var bit = from btc in r.BitControls where btc.ID == pa.BitControl_Ref select btc;
                                    if(bit.Count() != 0)
                                    {
                                        foreach(BitControl result in bit)
                                        {
                                            if (result.Type == "PossibleValues")
                                            {
                                                if(pa.InfoParameter == "")
                                                    sub_box.SubCategory_Content.Children.Add(new PossibleValuesType(result.ID, pa.Name, result.Position, result.BitLength, result.PossibleValues));
                                                else
                                                    sub_box.SubCategory_Content.Children.Add(new PossibleValuesType(result.ID, pa.Name, result.Position, result.BitLength, result.PossibleValues, pa.InfoParameter));

                                            }
                                            else
                                            {
                                                if (pa.InfoParameter == "")
                                                    sub_box.SubCategory_Content.Children.Add(new MinMaxType(result.ID, pa.Name, result.Position, result.BitLength, result.Unit, result.Lsb, result.MinValue, result.MaxValue));
                                                else
                                                    sub_box.SubCategory_Content.Children.Add(new MinMaxType(result.ID, pa.Name, result.Position, result.BitLength, result.Unit, result.Lsb, result.MinValue, result.MaxValue, pa.InfoParameter));

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if(s.Name == "Basic")
                        {
                            Channel_Parameters_Basic.Children.Add(sub_box);
                        }
                        else
                        {
                            subcategory_tab_content.Children.Add(sub_box);
                            sub_box = null;
                        }
                    }

                    if(s.Name != "Basic")
                    {
                        ScrollViewer subcategory_scroll = new ScrollViewer
                        {
                            Content = subcategory_tab_content
                        };
                        subcategory_tab.Content = subcategory_scroll;

                        Subcategory_TabControl.Items.Add(subcategory_tab);
                    }
                }
                if (Subcategory_TabControl.Items.Count != 0)
                {
                    ScrollViewer sw = new ScrollViewer
                    {
                        Content = Subcategory_TabControl
                    };

                    category_tab.Content = sw;
                    Channel_Parameters_Advanced.Items.Add(category_tab);

                    category_tab = null;
                }
                

            }

        }
    }
}
