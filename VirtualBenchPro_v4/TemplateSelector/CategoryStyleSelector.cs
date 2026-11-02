using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.TemplateSelector
{
    class CategoryStyleSelector : StyleSelector
    {
        public Style StringInfoStyle => Application.Current.Resources["Category_HeaderStringInfoFlat"] as Style;

        public Style ImageInfoStyle => Application.Current.Resources["Category_HeaderImageInfoFlat"] as Style;

        public Style EmptyStyle => Application.Current.Resources["TabItem_EmptyHeaderButton"] as Style;

        public override Style SelectStyle(object item, DependencyObject container)
        {
            MPS.Common.Model.Category sc = item as MPS.Common.Model.Category;
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
