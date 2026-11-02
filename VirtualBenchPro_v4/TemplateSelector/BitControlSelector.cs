using MPS.Common.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.TemplateSelector
{
    public class BitControlSelector : DataTemplateSelector
    { /// <summary>
      /// NormalRegTemplate
      /// </summary>
        public DataTemplate BitTextBoxTemplate => Application.Current.Resources["BitTextBoxTemplate"] as DataTemplate;

        /// <summary>
        /// CmdRegComboxTemplate 
        /// </summary>
        public DataTemplate BitComboxTemplate => Application.Current.Resources["BitComboxTemplate"] as DataTemplate;

        public DataTemplate BitToggleTemplate => Application.Current.Resources["BitToggleTemplate"] as DataTemplate;

        public DataTemplate BitButtonTemplate => Application.Current.Resources["BitButtonTemplate"] as DataTemplate;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            MPS.Common.Model.BitControl bitControl = item as MPS.Common.Model.BitControl;

            if (bitControl != null)
            {
                if (!bitControl.isToggle)
                {
                    if (string.IsNullOrEmpty(bitControl.PossibleValues))
                        return BitTextBoxTemplate;
                    else
                    {
                        if (GetisButtonTemplate(bitControl))
                        {
                            return BitButtonTemplate;
                        }
                        else
                        {
                            return BitComboxTemplate;
                        }
                    }
                }
                else
                {
                    return BitToggleTemplate;
                }
            }

            return null;
        }

        private static bool GetisButtonTemplate(BitControl bitControl)
        {
            if (string.IsNullOrEmpty(bitControl?.PossibleValues))
                return false;

            var possibleValueList = bitControl.PossibleValues.Split(';').Select(i => i.Trim()).ToList();
            if (possibleValueList.Count() == 1)
            {
                var possibleValue = possibleValueList[0];
                var splittedPossibleValue = possibleValue.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                var possibleValueName = splittedPossibleValue[0];

                if (possibleValueName == "WO")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
