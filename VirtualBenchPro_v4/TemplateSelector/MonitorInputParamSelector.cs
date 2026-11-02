using MPS.Common.Model;
using MPS.Monitor;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.TemplateSelector
{
    class MonitorInputParamSelector : DataTemplateSelector
    { 
        public DataTemplate MonitorInputParamTextBoxTemplate => Application.Current.Resources["MonitorInputParamTextBoxTemplate"] as DataTemplate;

        public DataTemplate MonitorInputParamComboBoxTemplate => Application.Current.Resources["MonitorInputParamComboBoxTemplate"] as DataTemplate;


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            MoniterInputParam param = item as MoniterInputParam;

            if (param != null)
            {
                if (param.ItemValues != null && param.ItemValues.Count() > 0)
                {
                    return MonitorInputParamComboBoxTemplate;
                }
                else
                {
                    return MonitorInputParamTextBoxTemplate;
                }
            }

            return null;
        }

    }
}
