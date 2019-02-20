// ******************************************************************
// Copyright (c) 2019. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// Source: https://github.com/rudySixToolkit/SixToolkittoolkit
// *****************************************************************

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SixToolkit.Utils
{
    public static class VisiblityWatcherExtension
    {
        public static VisibilityWatcher GetVisibilityWatcher(this FrameworkElement item)
        {
            return new VisibilityWatcher(item);
        }
    }

    public class VisibilityWatcher
    {
        public event EventHandler<Visibility> VisibilityChanged;
        private readonly FrameworkElement _control;
        private readonly VisibilityWatcherElement _watcher;

        public VisibilityWatcher(FrameworkElement control)
        {
            _control = control;
            _watcher = new VisibilityWatcherElement(this);
        }

        private void LaunchVisibilityChanged(Visibility visibility)
        {
            VisibilityChanged?.Invoke(_control, visibility);
        }
        public class VisibilityWatcherElement : FrameworkElement
        {
            private readonly VisibilityWatcher _parent;

            public VisibilityWatcherElement(VisibilityWatcher parent)
            {
                _parent = parent;

                var binding = new Binding()
                {
                    Path = new PropertyPath("Visibility"),
                    Source = _parent._control,
                };

                SetBinding(VisibilityProperty, binding);
            }

            #region Visibility
            public static new readonly DependencyProperty VisibilityProperty = DependencyProperty.Register(
                "Visibility", typeof(Visibility), typeof(VisibilityWatcherElement), new PropertyMetadata(default(Visibility), PropertyChangedCallback));

            private static void PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                ((VisibilityWatcherElement)o).OnVisibilityChanged((Visibility)e.NewValue);
            }

            public new Visibility Visibility
            {
                get => (Visibility)GetValue(VisibilityProperty);
                set => SetValue(VisibilityProperty, value);
            }

            protected void OnVisibilityChanged(Visibility visible)
            {
                _parent.LaunchVisibilityChanged(visible);
            }
            #endregion
        }
    }
}