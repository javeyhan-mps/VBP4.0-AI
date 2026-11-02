using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.TemplateSelector
{
    class ParamGroupTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringInfoTemplate => Application.Current.Resources["ParamGroup_StringInfoTemplate"] as DataTemplate;

        public DataTemplate ImageInfoTemplate => Application.Current.Resources["ParamGroup_ImageInfoTemplate"] as DataTemplate;

        public DataTemplate EmptyTemplate => Application.Current.Resources["ParamGroup_EmptyTemplate"] as DataTemplate;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            MPS.Common.Model.ParameterGroup sc = item as MPS.Common.Model.ParameterGroup;

            if (sc.Name != "")
            {
                if (sc.HelpInfo == "")
                {
                    return StringInfoTemplate;
                }
                else
                {
                    var IsStringInfo = ResolveInfo(sc.HelpInfo);
                    if (IsStringInfo)
                    {
                        return StringInfoTemplate;
                    }
                    else
                    {
                        return ImageInfoTemplate;
                    }
                }
            }
            else
            {
                return EmptyTemplate;
            }

        }

        private bool ResolveInfo(string key)
        {
            bool res = true;
            if (!key.StartsWith("P_"))
            {
                res = false;
            }
            return res;
        }
    }
}
