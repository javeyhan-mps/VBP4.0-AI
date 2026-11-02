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
    public class CustomerSelector : DataTemplateSelector
    {
        /// <summary>
        /// NormalRegTemplate
        /// </summary>
        public DataTemplate NormalRegTemplate => Application.Current.Resources["NormalRegTemplate"] as DataTemplate;

        /// <summary>
        /// CmdRegComboxTemplate 
        /// </summary>
        public DataTemplate CmdRegComboxTemplate => Application.Current.Resources["CmdRegComboxTemplate"] as DataTemplate;

        /// <summary>
        /// CmdRegTextTemplate 
        /// </summary>
        public DataTemplate CmdRegTextTemplate => Application.Current.Resources["CmdRegTextTemplate"] as DataTemplate;

        public DataTemplate ParameterRegTextTemplate => Application.Current.Resources["ParameterRegTextTemplate"] as DataTemplate;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //MfrRegister reginfo = item as MfrRegister;
            //if (reginfo != null)
            //    return NormalRegTemplate;

            //ParameterRegister param = item as ParameterRegister;
            //if (param != null)
            //    return ParameterRegTextTemplate;
            //CommandRegister cmdinfo = item as CommandRegister;
            //if (cmdinfo != null)
            //{
            //    if (string.IsNullOrEmpty(cmdinfo.CommandPossibleValues))
            //        return CmdRegTextTemplate;
            //    else
            //        return CmdRegComboxTemplate;

            //}
            var param = item as ParameterRegister;
            if (param != null)
                return ParameterRegTextTemplate;
            return NormalRegTemplate;


        }

    }
}
