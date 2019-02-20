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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SixToolkit.Panels
{
    public sealed class PercentagePanel : ContentControl
    {
        private ColumnDefinition _leftColumn;
        private ColumnDefinition _rightColumn;
        private RowDefinition _topRow;
        private RowDefinition _bottomRow;

        public PercentagePanel()
        {
            DefaultStyleKey = typeof(PercentagePanel);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftColumn = GetTemplateChild("LeftColumn") as ColumnDefinition;
            _rightColumn = GetTemplateChild("RightColumn") as ColumnDefinition;
            _topRow = GetTemplateChild("TopRow") as RowDefinition;
            _bottomRow = GetTemplateChild("BottomRow") as RowDefinition;
            UpdateHorizontalSpacing();
            UpdateVerticalSpacing();
        }

        #region Left
        public double Left
        {
            get => (double)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(PercentagePanel), new PropertyMetadata(0.0, LeftCallback));

        private static void LeftCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PercentagePanel)d).UpdateHorizontalSpacing();
        }

        #endregion

        #region Right
        public double Right
        {
            get => (double)GetValue(RightProperty);
            set => SetValue(RightProperty, value);
        }

        public static readonly DependencyProperty RightProperty =
            DependencyProperty.Register("Right", typeof(double), typeof(PercentagePanel), new PropertyMetadata(0.0, RightCallback));

        private static void RightCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PercentagePanel)d).UpdateHorizontalSpacing();
        }

        #endregion

        #region Top
        public double Top
        {
            get => (double)GetValue(TopProperty);
            set => SetValue(TopProperty, value);
        }

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(PercentagePanel), new PropertyMetadata(0.0, TopCallback));

        private static void TopCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PercentagePanel)d).UpdateVerticalSpacing();
        }

        #endregion

        #region Bottom
        public double Bottom
        {
            get => (double)GetValue(BottomProperty);
            set => SetValue(BottomProperty, value);
        }

        public static readonly DependencyProperty BottomProperty =
            DependencyProperty.Register("Bottom", typeof(double), typeof(PercentagePanel), new PropertyMetadata(0.0, BottomCallback));

        private static void BottomCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PercentagePanel)d).UpdateVerticalSpacing();
        }
        #endregion

        private void UpdateVerticalSpacing()
        {
            if (_topRow == null || _bottomRow == null)
            {
                return;
            }

            var topValue = Math.Min(1,Math.Max(Top, 0));
            var bottomValue = Math.Min(1, Math.Max(Bottom, 0));

            if (topValue + bottomValue >= 1)
            {
                _topRow.Height = new GridLength(topValue / (1 - topValue), GridUnitType.Star);
                _bottomRow.Height = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                _topRow.Height = new GridLength(topValue / (1 - topValue - bottomValue), GridUnitType.Star);
                _bottomRow.Height = new GridLength(bottomValue / (1 - topValue - bottomValue), GridUnitType.Star);
            }
        }

        private void UpdateHorizontalSpacing()
        {
            if (_leftColumn == null || _rightColumn == null)
            {
                return;
            }

            var leftValue = Math.Min(1, Math.Max(Left, 0));
            var rightValue = Math.Min(1, Math.Max(Right, 0));

            if (leftValue + rightValue >= 1)
            {
                _leftColumn.Width = new GridLength(leftValue / (1 - leftValue), GridUnitType.Star);
                _rightColumn.Width = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                _leftColumn.Width = new GridLength(leftValue / (1 - leftValue - rightValue), GridUnitType.Star);
                _rightColumn.Width = new GridLength(rightValue / (1 - leftValue - rightValue), GridUnitType.Star);
            }
        }
    }
}
