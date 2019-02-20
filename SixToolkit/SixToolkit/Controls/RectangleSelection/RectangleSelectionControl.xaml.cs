// *****************************************************************
// Copyright (c) 2019. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// Source: https://github.com/rudyhuyn/SixToolkit
// *****************************************************************

using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace SixToolkit.Controls
{
    public sealed partial class RectangleSelectionControl : UserControl
    {
        private const uint MinimumSelectionSize = 30;
        private GeneralTransform _transform;
        private Point? _startPoint = null;
        private Point? _endPoint = null;
        private Point _startPointOriginal;
        private Point _endPointOriginal;
        private readonly Window _window;
        private readonly CoreWindow _coreWindow;
        public event EventHandler<Rect> SelectionMade;
        private bool _selectionStarted;

        public RectangleSelectionControl()
        {
            InitializeComponent();

            _window = Window.Current;
            _coreWindow = CoreWindow.GetForCurrentThread();
        }

        public bool IsInProgress()
        {
            return IsEnabled && _startPoint != null;
        }

        private void _coreWindow_PointerExited(CoreWindow sender, PointerEventArgs args)
        {
            CleanSelection();
        }

        private static Rect? GetBoundsRelativeTo(FrameworkElement element, UIElement otherElement)
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
            _selectionStarted = false;
        }

        private void RectangularSelectionControl_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            if (!_startPoint.HasValue)
            {
                return;
            }

            var startPoint = _startPoint.Value;
            var newPoint = _transform.TransformPoint(args.CurrentPoint.Position);

            if (!_selectionStarted)
            {
                var diffX = (_startPoint.Value.X - newPoint.X);
                var diffY = (_startPoint.Value.Y - newPoint.Y);
                var distanceSquare = diffX * diffX + diffY * diffY;
                if (distanceSquare < MinimumSelectionSize * MinimumSelectionSize)
                {
                    return;
                }

                _selectionStarted = true;
                RootPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            double startX, endX;
            if (startPoint.X < newPoint.X)
            {
                startX = startPoint.X;
                endX = newPoint.X;
            }
            else
            {
                startX = newPoint.X;
                endX = startPoint.X;
            }

            if (startX < 0)
            {
                startX = 0;
            }

            if (endX > ActualWidth)
            {
                endX = ActualWidth;
            }

            Canvas.SetLeft(SelectionRect, startX);
            SelectionRect.Width = endX - startX;

            double startY, endY;
            if (startPoint.Y < newPoint.Y)
            {
                startY = startPoint.Y;
                endY = newPoint.Y;
            }
            else
            {
                startY = newPoint.Y;
                endY = startPoint.Y;
            }

            if (startY < 0)
            {
                startY = 0;
            }

            if (endY > ActualHeight)
            {
                endY = ActualHeight;
            }

            Canvas.SetTop(SelectionRect, startY);
            SelectionRect.Height = endY - startY;


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

        #region RectangleStyle



        public Style RectangleStyle
        {
            get => (Style)GetValue(RectangleStyleProperty);
            set => SetValue(RectangleStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for RectangleStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleStyleProperty =
            DependencyProperty.Register("RectangleStyle", typeof(Style), typeof(RectangleSelectionControl), new PropertyMetadata(null, RectangleStyleCallback));

        private static void RectangleStyleCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((RectangleSelectionControl)d);
            ctrl.SelectionRect.Style = (Style)e.NewValue ?? (Style)ctrl.Resources["RectangleStyle"];
        }

        #endregion

    }
}
