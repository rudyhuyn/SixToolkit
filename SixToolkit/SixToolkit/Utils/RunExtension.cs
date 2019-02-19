
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace SixToolkit.Utils
{
    internal class RunExtension : DependencyObject
    {
        #region Text

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(Run),
                new PropertyMetadata(null, OnTextChanged));

        private static async void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Run)d).Text = e.NewValue?.ToString() ?? "";
        }
        #endregion


        #region Foreground

        public static Brush GetForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(Run),
                new PropertyMetadata(null, OnForegroundChanged));

        private static async void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Run)d).Foreground = e.NewValue as Brush;
        }
        #endregion


        #region FontWeight

        public static Brush GetFontWeight(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FontWeightProperty);
        }

        public static void SetFontWeight(DependencyObject obj, Brush value)
        {
            obj.SetValue(FontWeightProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.RegisterAttached("FontWeight", typeof(FontWeight), typeof(Run),
                new PropertyMetadata(null, OnFontWeightChanged));

        private static async void OnFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Run)d).FontWeight = (FontWeight)e.NewValue;
        }
        #endregion

    }
}