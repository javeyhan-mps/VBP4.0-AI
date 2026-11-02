using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VirtualBenchPro_v4.Helper
{
    public class WindowHelper
    {
        #region IsMainWindowOwner

        public static bool GetIsMainWindowOwner(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMainWindowOwnerProperty);
        }

        public static void SetIsMainWindowOwner(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMainWindowOwnerProperty, value);
        }

        public static readonly DependencyProperty IsMainWindowOwnerProperty =
            DependencyProperty.RegisterAttached("IsMainWindowOwner", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, IsMainWindowOwnerPropertyChangedCallBack));

        private static void IsMainWindowOwnerPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;

            if (window == null)
                return;

            if ((bool)e.NewValue)
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow?.IsLoaded == true)
                {
                    window.Owner = mainWindow;
                }
            }
        }

        #endregion
    }
}
