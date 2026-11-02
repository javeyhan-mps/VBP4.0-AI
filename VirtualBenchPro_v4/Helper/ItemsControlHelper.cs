using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VirtualBenchPro_v4.Helper
{
    public class ItemsControlHelper
    {
        #region GridPanelColumns
        public static int GetGridPanelColumns(DependencyObject obj)
        {
            return (int)obj.GetValue(GridPanelColumnsProperty);
        }

        public static void SetGridPanelColumns(DependencyObject obj, int value)
        {
            obj.SetValue(GridPanelColumnsProperty, value);
        }

        public static readonly DependencyProperty GridPanelColumnsProperty =
            DependencyProperty.RegisterAttached("GridPanelColumns", typeof(int), typeof(ItemsControlHelper), new PropertyMetadata(0, GridPanelColumnsPropertyChangedCallback));

        private static void GridPanelColumnsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid grid = d as Grid;
            int columns = (int)e.NewValue;

            // Column definitions
            for (int column = 0; column < columns; column++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            grid.Loaded += (s, e2) =>
            {
                UpdateItemsControlLayout(grid, columns);
            };
        }
        #endregion

        #region ItemsSource
        public static IEnumerable GetItemsSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable), typeof(ItemsControlHelper), new PropertyMetadata(null, ItemsSourcePropertyChangedCallback));

        private static void ItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl control = d as ItemsControl;

            // set the ItemsSource of the ItemsControl that this property is attached to
            control.ItemsSource = e.NewValue as IEnumerable;

            var itemsGridPanel = FindVisualChildren<Grid>(control).ToList().FirstOrDefault(g => GetGridPanelColumns(g) > 0);

            if (itemsGridPanel != null)
            {
                UpdateItemsControlLayout(itemsGridPanel, GetGridPanelColumns(itemsGridPanel));
            }
        }
        #endregion

        private static void UpdateItemsControlLayout(Grid grid, int columns)
        {
            var childCount = grid.Children.Count;

            grid.RowDefinitions.Clear();
            // add the required number of row definitions
            //var resultQuotient = childCount / itemsPerRow;
            //var resultRemainder = childCount / itemsPerRow;
            var rowsToAdd = (int)Math.Ceiling((double)childCount / columns);

            for (int row = 0; row < rowsToAdd; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }

            // set the row and column property for each chid
            for (int i = 0; i < childCount; i++)
            {
                var child = grid.Children[i] as FrameworkElement;
                Grid.SetRow(child, i / columns);
                Grid.SetColumn(child, i % columns);
            }
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
    }
}
