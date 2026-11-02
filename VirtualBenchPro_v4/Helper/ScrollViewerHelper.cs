using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VirtualBenchPro_v4.Helper
{
    public class ScrollViewerHelper
    {
        #region IsPreviewMouseWheelEventMovetoNext
        // Set IsPreviewMouseWheelEventMovetoNext to true to prevent the ItemsControl that contains ScrollViwer capturing PreviewMouseWheel event and making it being handled
        // so the scrollViewer that contains this control cannot be scrolled
        public static bool GetIsPreviewMouseWheelEventMovetoNext(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsPreviewMouseWheelEventMovetoNextProperty);
        }

        public static void SetIsPreviewMouseWheelEventMovetoNext(DependencyObject obj, bool value)
        {
            obj.SetValue(IsPreviewMouseWheelEventMovetoNextProperty, value);
        }

        public static readonly DependencyProperty IsPreviewMouseWheelEventMovetoNextProperty =
            DependencyProperty.RegisterAttached("IsPreviewMouseWheelEventMovetoNext", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false, IsPreviewMouseWheelEventMovetoNextChangedCallback));

        private static void IsPreviewMouseWheelEventMovetoNextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ic = d as ItemsControl;

            if (ic == null)
                return;

            if ((bool)e.NewValue == true)
            {
                ic.PreviewMouseWheel += PreviewMouseWheel;
            }
            else
            {
                ic.PreviewMouseWheel -= PreviewMouseWheel;
            }
        }

        private static void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //e.Handled = true;
            var ic = sender as ItemsControl;
            if (ic == null)
            {
                return;
            }

            var childComboBoxList = FindVisualChildren<ComboBox>(ic);
            var anyComboBoxOpen = childComboBoxList.Any(i => i.IsDropDownOpen);

            if (anyComboBoxOpen)
            {
                return;
            }

            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            ic.RaiseEvent(eventArg);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    //if (child != null && (T)child != null)
                    if (child != null && child.GetType() == typeof(T))
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        #endregion
    }
}
