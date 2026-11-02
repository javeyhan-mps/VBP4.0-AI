using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MPS.Common.Model;

namespace VirtualBenchPro_v4.TemplateSelector
{
    class SubcategoryStyleSelector : StyleSelector
    {
        public Style StringInfoStyle => Application.Current.Resources["Subcategory_HeaderStringInfoButton"] as Style;

        public Style ImageInfoStyle => Application.Current.Resources["Subcategory_HeaderImageInfoButton"] as Style;

        public Style EmptyStyle => Application.Current.Resources["TabItem_EmptyHeaderButton"] as Style;

        public override Style SelectStyle(object item, DependencyObject container)
        {
            Subcategory sc = item as Subcategory;
            if (sc.Name != "")
            {
                if (sc.HelpInfo == "")
                {
                    return StringInfoStyle;
                }
                else
                {
                    var IsStringInfo = ResolveInfo(sc.HelpInfo);
                    if (IsStringInfo)
                    {
                        return StringInfoStyle;
                    }
                    else
                    {
                        return ImageInfoStyle;
                    }
                }
            }
            else
            {
                return EmptyStyle;
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
