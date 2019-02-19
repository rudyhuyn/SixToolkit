using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SixToolkit.Panels
{
    public sealed class AdaptivePanel : ContentControl
    {
        private Grid _container;
        private ContentPresenter _innerContentPresenter;

        public AdaptivePanel()
        {
            DefaultStyleKey = typeof(AdaptivePanel);
            IsTabStop = false;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = (Grid)VisualTreeHelper.GetChild(this, 0);
            _innerContentPresenter = (ContentPresenter)VisualTreeHelper.GetChild(_container, 0);

            if (ContentVerticalAlignment != VerticalAlignment.Center)
            {
                ManageContentVerticalAlignment(ContentVerticalAlignment);
            }

            if (ContentHorizontalAlignment != HorizontalAlignment.Center)
            {
                ManageContentHorizontalAlignment(ContentHorizontalAlignment);
            }

            ManageMaxHeight(ContentMaxHeight);
            ManageMaxWidth(ContentMaxWidth);
            ManageMinHeight(ContentMinHeight);
            ManageMinWidth(ContentMinWidth);
        }

        #region ContentVerticalAlignment

        public static readonly DependencyProperty ContentVerticalAlignmentProperty = DependencyProperty.Register(
            "ContentVerticalAlignment", typeof(VerticalAlignment), typeof(AdaptivePanel),
            new PropertyMetadata(VerticalAlignment.Center, ContentVerticalAlignmentCallback));

        public VerticalAlignment ContentVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(ContentVerticalAlignmentProperty);
            set => SetValue(ContentVerticalAlignmentProperty, value);
        }

        private static void ContentVerticalAlignmentCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdaptivePanel)d).ContentVerticalAlignmentCallback((VerticalAlignment)e.NewValue);
        }

        private void ContentVerticalAlignmentCallback(VerticalAlignment value)
        {
            ManageContentVerticalAlignment(value);
            ManageMaxHeight(ContentMaxHeight);
            ManageMinHeight(ContentMinHeight);
        }

        private void ManageContentVerticalAlignment(VerticalAlignment value)
        {
            if (_innerContentPresenter == null)
            {
                return;
            }

            switch (value)
            {
                case VerticalAlignment.Top:
                    _container.RowDefinitions.Clear();
                    _container.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star),
                        MaxHeight = 400
                    });
                    Grid.SetRow(_innerContentPresenter, 0);
                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    _container.RowDefinitions.Clear();
                    Grid.SetRow(_innerContentPresenter, 0);
                    break;
                case VerticalAlignment.Bottom:
                    _container.RowDefinitions.Clear();
                    _container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    _container.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(int.MaxValue, GridUnitType.Star),
                        MaxHeight = 400
                    });
                    Grid.SetRow(_innerContentPresenter, 1);
                    break;
            }
        }

        #endregion

        #region ContentHorizontalAlignment

        public static readonly DependencyProperty ContentHorizontalAlignmentProperty =
            DependencyProperty.Register("ContentHorizontalAlignment", typeof(HorizontalAlignment),
                typeof(AdaptivePanel),
                new PropertyMetadata(HorizontalAlignment.Center, ContentHorizontalAlignmentCallback));

        public HorizontalAlignment ContentHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ContentHorizontalAlignmentProperty);
            set => SetValue(ContentHorizontalAlignmentProperty, value);
        }

        private static void ContentHorizontalAlignmentCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdaptivePanel)d).ContentHorizontalAlignmentCallback((HorizontalAlignment)e.NewValue);
        }

        private void ContentHorizontalAlignmentCallback(HorizontalAlignment value)
        {
            ManageContentHorizontalAlignment(value);
            ManageMaxWidth(ContentMaxWidth);
            ManageMinWidth(ContentMinWidth);
        }

        private void ManageContentHorizontalAlignment(HorizontalAlignment value)
        {
            if (_innerContentPresenter == null)
            {
                return;
            }

            switch (value)
            {
                case HorizontalAlignment.Left:
                    _container.ColumnDefinitions.Clear();
                    _container.ColumnDefinitions.Add(
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    Grid.SetColumn(_innerContentPresenter, 0);
                    break;
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    _container.ColumnDefinitions.Clear();
                    Grid.SetColumn(_innerContentPresenter, 0);
                    break;
                case HorizontalAlignment.Right:
                    _container.ColumnDefinitions.Clear();
                    _container.ColumnDefinitions.Add(
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    _container.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(int.MaxValue, GridUnitType.Star)
                    });
                    Grid.SetColumn(_innerContentPresenter, 1);
                    break;
            }
        }

        #endregion

        #region ContentMaxHeight

        public static readonly DependencyProperty ContentMaxHeightProperty = DependencyProperty.Register(
            "ContentMaxHeight", typeof(double), typeof(AdaptivePanel),
            new PropertyMetadata(double.PositiveInfinity, ContentMaxHeightCallback));

        public double ContentMaxHeight
        {
            get => (double)GetValue(ContentMaxHeightProperty);
            set => SetValue(ContentMaxHeightProperty, value);
        }

        private static void ContentMaxHeightCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdaptivePanel)d).ManageMaxHeight((double)e.NewValue);
        }

        private void ManageMaxHeight(double value)
        {
            if (_innerContentPresenter == null)
            {
                return;
            }

            switch (ContentVerticalAlignment)
            {
                case VerticalAlignment.Top:
                    _innerContentPresenter.MaxHeight = double.MaxValue;
                    if (_container.RowDefinitions.Count == 1)
                    {
                        _container.RowDefinitions[0].MaxHeight = value;
                    }

                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    _innerContentPresenter.MaxHeight = value;
                    break;
                case VerticalAlignment.Bottom:
                    _innerContentPresenter.MaxHeight = double.MaxValue;
                    if (_container.RowDefinitions.Count == 2)
                    {
                        _container.RowDefinitions[1].MaxHeight = value;
                    }

                    break;
            }
        }

        #endregion

        #region ContentMaxWidth

        public static readonly DependencyProperty ContentMaxWidthProperty = DependencyProperty.Register(
            "ContentMaxWidth", typeof(double), typeof(AdaptivePanel),
            new PropertyMetadata(double.PositiveInfinity, ContentMaxWidthCallback));

        public double ContentMaxWidth
        {
            get => (double)GetValue(ContentMaxWidthProperty);
            set => SetValue(ContentMaxWidthProperty, value);
        }

        private static void ContentMaxWidthCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdaptivePanel)d).ManageMaxWidth((double)e.NewValue);
        }

        private void ManageMaxWidth(double value)
        {
            if (_innerContentPresenter == null)
            {
                return;
            }

            switch (ContentHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    _innerContentPresenter.MaxWidth = double.MaxValue;
                    if (_container.ColumnDefinitions.Count == 1)
                    {
                        _container.ColumnDefinitions[0].MaxWidth = value;
                    }

                    break;
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    _innerContentPresenter.MaxWidth = value;
                    break;
                case HorizontalAlignment.Right:
                    _innerContentPresenter.MaxWidth = double.MaxValue;
                    if (_container.ColumnDefinitions.Count == 2)
                    {
                        _container.ColumnDefinitions[1].MaxWidth = value;
                    }

                    break;
            }
        }

        #endregion

        #region ContentMinHeight

        public static readonly DependencyProperty ContentMinHeightProperty = DependencyProperty.Register(
            "ContentMinHeight", typeof(double), typeof(AdaptivePanel),
            new PropertyMetadata(0d, ContentMinHeightCallback));

        public double ContentMinHeight
        {
            get => (double)GetValue(ContentMinHeightProperty);
            set => SetValue(ContentMinHeightProperty, value);
        }

        private static void ContentMinHeightCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdaptivePanel)d).ManageMinHeight((double)e.NewValue);
        }

        private void ManageMinHeight(double value)
        {
            if (_innerContentPresenter == null)
            {
                return;
            }

            switch (ContentVerticalAlignment)
            {
                case VerticalAlignment.Top:
                    _innerContentPresenter.MinHeight = 0;
                    if (_container.RowDefinitions.Count == 1)
                    {
                        _container.RowDefinitions[0].MinHeight = value;
                    }

                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    _innerContentPresenter.MinHeight = value;
                    break;
                case VerticalAlignment.Bottom:
                    _innerContentPresenter.MinHeight = 0;
                    if (_container.RowDefinitions.Count == 2)
                    {
                        _container.RowDefinitions[1].MinHeight = value;
                    }

                    break;
            }
        }

        #endregion

        #region ContentMinWidth

        public static readonly DependencyProperty ContentMinWidthProperty = DependencyProperty.Register(
            "ContentMinWidth", typeof(double), typeof(AdaptivePanel),
            new PropertyMetadata(0d, ContentMinWidthCallback));

        public double ContentMinWidth
        {
            get => (double)GetValue(ContentMinWidthProperty);
            set => SetValue(ContentMinWidthProperty, value);
        }

        private static void ContentMinWidthCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdaptivePanel)d).ManageMinWidth((double)e.NewValue);
        }

        private void ManageMinWidth(double value)
        {
            if (_innerContentPresenter == null)
            {
                return;
            }

            switch (ContentHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    _innerContentPresenter.MinWidth = 0;
                    if (_container.ColumnDefinitions.Count == 1)
                    {
                        _container.ColumnDefinitions[0].MinWidth = value;
                    }

                    break;
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    _innerContentPresenter.MinWidth = value;
                    break;
                case HorizontalAlignment.Right:
                    _innerContentPresenter.MinWidth = 0;
                    if (_container.ColumnDefinitions.Count == 2)
                    {
                        _container.ColumnDefinitions[1].MinWidth = value;
                    }

                    break;
            }
        }

        #endregion
    }
}