using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.TemplateSelector
{
    public class ParamsOverviewSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item.GetType() == typeof(RegisterRegion))
            {
                RegisterRegion region = item as RegisterRegion;

                Window win = Application.Current.MainWindow;

                if (region.RegionName != "Overview")
                {
                    return win.FindResource("ParametersTemplate") as DataTemplate;
                }
                else
                {
                    return win.FindResource("OverviewTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
