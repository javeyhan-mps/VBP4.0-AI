using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VirtualBenchPro_v4.Classes
{
    public class ControlManager
    {
        private readonly UIElement _control;
        private DependencyObject _originalParent;
        private int _originalIndex;

        public ControlManager(UIElement control)
        {
            _control = control ?? throw new ArgumentNullException(nameof(control));
        }

        public void DetachFromParent()
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(_control);

            if (parent is Panel parentPanel)
            {
                _originalParent = parentPanel;
                _originalIndex = parentPanel.Children.IndexOf(_control);
                DisconnectFromParent(_control);
            }
            else
            {
                _originalParent = parent;
                DisconnectFromParent(_control);
            }
        }

        public void ReattachToParent()
        {
            if (_control == null) return;

            DisconnectFromParent(_control);

            if (_originalParent is Panel parentPanel)
            {
                if (_originalIndex >= 0 && _originalIndex <= parentPanel.Children.Count)
                {
                    parentPanel.Children.Insert(_originalIndex, _control);
                }
                else
                {
                    parentPanel.Children.Add(_control);
                }
            }
            else if (_originalParent is Decorator parentDecorator)
            {
                parentDecorator.Child = _control;
            }
            else if (_originalParent is ContentPresenter parentcontentPresenter)
            {
                parentcontentPresenter.Content = _control;
            }
            else if (_originalParent is ContentControl parentcontentControl)
            {
                parentcontentControl.Content = _control;
            }

            _originalIndex = -1;
            _originalParent = null;
        }

        private void DisconnectFromParent(UIElement control)
        {
            if (control == null) return;

            control.Dispatcher.Invoke(() =>
            {
                //DependencyObject parent = VisualTreeHelper.GetParent(control);
                //if (parent != null)
                //{
                //    var method = typeof(Visual).GetMethod("RemoveFromVisualTree", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                //    method?.Invoke(control, null);
                //}

                if (LogicalTreeHelper.GetParent(control) is DependencyObject logicalParent)
                {
                    if (logicalParent is ContentPresenter cp) cp.Content = null;
                    else if (logicalParent is ContentControl cc) cc.Content = null;
                    else if (logicalParent is Decorator dec) dec.Child = null;
                    else if (logicalParent is Panel panel) panel.Children.Remove(control);
                }
            });
        }
    }
}
