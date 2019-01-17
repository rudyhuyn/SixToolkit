// ******************************************************************
// Copyright (c) Rudy Huyn. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// Source: https://github.com/rudyhuyn/huyntoolkit
// *****************************************************************

using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Huyn
{
    public sealed partial class RectangleSelectionControl : UserControl
    {
        private GeneralTransform _transform;
        private Point? _startPoint = null;
        private Point? _endPoint = null;
        private Point _startPointOriginal;
        private Point _endPointOriginal;
        private readonly Window _window;
        private readonly CoreWindow _coreWindow;
        public event EventHandler<Rect> SelectionMade;

        public RectangleSelectionControl()
        {
            InitializeComponent();

            _window = Window.Current;
            _coreWindow = CoreWindow.GetForCurrentThread();
        }

        private void _coreWindow_PointerExited(CoreWindow sender, PointerEventArgs args)
        {
            CleanSelection();
        }

        private static Rect? GetBoundsRelativeTo( FrameworkElement element, UIElement otherElement)
        {
         
            try
            {
                var generalTransform = element.TransformToVisual(otherElement);
                if (generalTransform != null && generalTransform.TryTransform(default(Point), out var point) &&
                    generalTransform.TryTransform(new Point(element.ActualWidth, element.ActualHeight), out var point2))
                {
                    return new Rect(point, point2);
                }
            }
            catch (ArgumentException)
            {
            }

            return default(Rect?);
        }

        private void RectangularSelectionControl_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            if (!IsEnabled)
            {
                return;
            }

            var controlRect = GetBoundsRelativeTo(this, Window.Current.Content);
            if (!controlRect.HasValue || !controlRect.Value.Contains(args.CurrentPoint.Position))
            {
                return;
            }

            var items = VisualTreeHelper.FindElementsInHostCoordinates(args.CurrentPoint.Position, Window.Current.Content);
            if (items.Any(i => i is ButtonBase || i is CheckBox))
            {
                return;
            }

            _transform = _window.Content.TransformToVisual(this);
            _startPointOriginal = args.CurrentPoint.Position;
            _startPoint = _transform.TransformPoint(args.CurrentPoint.Position);

            _coreWindow.PointerExited += _coreWindow_PointerExited;
            _coreWindow.PointerMoved += RectangularSelectionControl_PointerMoved;
            _coreWindow.PointerReleased += CoreWindow_PointerReleased;
        }

        private void CoreWindow_PointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            EndSelection();
        }

        private void EndSelection()
        {
            if (!_startPoint.HasValue || !_endPoint.HasValue)
            {
                _startPoint = null;
                return;
            }
            SelectionMade?.Invoke(this, new Rect(_startPointOriginal, _endPointOriginal));
            CleanSelection();
        }

        private void CleanSelection()
        {
            _coreWindow.PointerExited -= _coreWindow_PointerExited;
            _coreWindow.PointerMoved -= RectangularSelectionControl_PointerMoved;
            _coreWindow.PointerReleased -= CoreWindow_PointerReleased;
            RootPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _endPoint = _startPoint = null;
        }

        private void RectangularSelectionControl_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            if (!_startPoint.HasValue)
            {
                return;
            }
            var startPoint = _startPoint.Value;
            var newPoint = _transform.TransformPoint(args.CurrentPoint.Position);
            RootPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (startPoint.X < newPoint.X)
            {
                Canvas.SetLeft(SelectionRect, startPoint.X);
                SelectionRect.Width = newPoint.X - startPoint.X;
            }
            else
            {
                Canvas.SetLeft(SelectionRect, newPoint.X);
                SelectionRect.Width = startPoint.X - newPoint.X;
            }

            if (startPoint.Y < newPoint.Y)
            {
                Canvas.SetTop(SelectionRect, startPoint.Y);
                SelectionRect.Height = newPoint.Y - startPoint.Y;
            }
            else
            {
                Canvas.SetTop(SelectionRect, newPoint.Y);
                SelectionRect.Height = startPoint.Y - newPoint.Y;
            }

            _endPoint = newPoint;
            _endPointOriginal = args.CurrentPoint.Position;
        }

        #region IsEnabled



        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(RectangleSelectionControl), new PropertyMetadata(false, IsEnabledCallback));

        private static void IsEnabledCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RectangleSelectionControl)d).IsEnabledCallback((bool)e.NewValue);
        }

        private void IsEnabledCallback(bool value)
        {
            if (!value)
            {
                _coreWindow.PointerPressed -= RectangularSelectionControl_PointerPressed;
                CleanSelection();
            }
            else
            {
                _coreWindow.PointerPressed += RectangularSelectionControl_PointerPressed;
            }
        }

        #endregion
    }
}
