using MPS.Common.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.TemplateSelector
{
    public class DetailRegMap_BitControlSelector : DataTemplateSelector
    {
        public DataTemplate BitTextBoxTemplate => Application.Current.Resources["DetailRegMap_BitTextBoxTemplate"] as DataTemplate;

        public DataTemplate BitReadOnlyTextBoxTemplate => Application.Current.Resources["DetailRegMap_BitReadOnlyTextBoxTemplate"] as DataTemplate;

        public DataTemplate ReservedBitTextBoxTemplate => Application.Current.Resources["DetailRegMap_ReservedBitTextBoxTemplate"] as DataTemplate;

        public DataTemplate BitComboxTemplate => Application.Current.Resources["DetailRegMap_BitComboxTemplate"] as DataTemplate;

        public DataTemplate BitButtonTemplate => Application.Current.Resources["DetailRegMap_BitSendByteButtonTemplate"] as DataTemplate;

        public DataTemplate BitReadOnlyComboxTemplate => Application.Current.Resources["DetailRegMap_BitReadOnlyComboxTemplate"] as DataTemplate;

        //public DataTemplate BitReservedTemplate => Application.Current.Resources["DetailRegMap_BitReservedTemplate"] as DataTemplate;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            BitControl bitControl = item as BitControl;

            if (bitControl != null)
            {
                if (bitControl.Access == "RW")
                {
                    if (string.IsNullOrEmpty(bitControl.PossibleValues))
                    {
                        return BitTextBoxTemplate;
                    }
                    else
                    {
                        return BitComboxTemplate;
                    }
                }
                else if (bitControl.Access == "R")
                {
                    if (bitControl.BitName.ToUpper() == "RESERVED")
                    {
                        return ReservedBitTextBoxTemplate;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(bitControl.PossibleValues))
                        {
                            return BitReadOnlyTextBoxTemplate;
                        }
                        else
                        {
                            return BitReadOnlyComboxTemplate;
                        }
                    }
                }
                else if (bitControl.Access == "W")
                {
                    if (string.IsNullOrEmpty(bitControl.PossibleValues))
                    {
                        return BitTextBoxTemplate;
                    }
                    else
                    {
                        return BitComboxTemplate;
                    }
                }
                else if (bitControl.Access == "Send byte")
                {
                    return BitButtonTemplate;
                }

            }

            return null;
        }

    }

}
