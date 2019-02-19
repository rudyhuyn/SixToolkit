using SixToolkit.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SixToolkit.Controls
{
    public sealed partial class XboxKeyControl : UserControl
    {
        public enum Key
        {
            Unknown,
            X,
            Y,
            A,
            B,
            View,
            Play
        }

        private Key _currentKey = Key.Unknown;

        public XboxKeyControl()
        {
            InitializeComponent();
        }

        private void Instance_IsMediaRemoteLastUsedChanged(object sender, bool e)
        {
            UpdateButton();
        }

        private void UpdateButton()
        {
            var isMediaRemoteLastUsed = GamepadHelper.Instance.IsMediaRemoteLastUsed;

            var selectedKey = SymbolMedia != Key.Unknown && isMediaRemoteLastUsed ? SymbolMedia : Symbol;

            if (_currentKey == selectedKey)
            {
                return;
            }

            _currentKey = selectedKey;
            switch (selectedKey)
            {
                case Key.Unknown:
                    CircleBackground.Fill = null;
                    Label.Text = "";
                    Label.Visibility = Visibility.Collapsed;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;
                    break;
                case Key.A:
                    CircleBackground.Fill = (Brush)Resources["XboxGreenBrush"];
                    Label.Text = "A";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;

                    break;
                case Key.B:
                    CircleBackground.Fill = (Brush)Resources["XboxRedBrush"];
                    Label.Text = "B";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    break;
                case Key.X:
                    CircleBackground.Fill = (Brush)Resources["XboxBlueBrush"];
                    Label.Text = "X";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;

                    break;
                case Key.Y:
                    CircleBackground.Fill = (Brush)Resources["XboxYellowBrush"];
                    Label.Text = "Y";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;

                    break;
                case Key.View:
                    CircleBackground.Fill = new SolidColorBrush(Colors.Black);
                    Label.Visibility = Visibility.Collapsed;
                    ViewPath.Visibility = Visibility.Visible;
                    PlayPath.Visibility = Visibility.Collapsed;

                    break;
                case Key.Play:
                    CircleBackground.Fill = new SolidColorBrush(Colors.Black);
                    Label.Visibility = Visibility.Collapsed;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Visible;

                    break;
            }
        }

        #region Symbol

        public Key Symbol
        {
            get => (Key)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof(Key), typeof(XboxKeyControl),
                new PropertyMetadata(Key.Unknown, SymbolCallback));

        private static void SymbolCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((XboxKeyControl)d).UpdateButton();
        }

        #endregion

        #region SymbolMedia

        public Key SymbolMedia
        {
            get => (Key)GetValue(SymbolMediaProperty);
            set => SetValue(SymbolMediaProperty, value);
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolMediaProperty =
            DependencyProperty.Register("SymbolMedia", typeof(Key), typeof(XboxKeyControl),
                new PropertyMetadata(Key.Unknown, SymbolMediaCallback));

        private static void SymbolMediaCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((XboxKeyControl)d).SymbolMediaCallback(e.NewValue);
        }

        private void SymbolMediaCallback(object newValue)
        {
            UpdateButton();
            if (newValue == null)
            {
                GamepadHelper.Instance.IsMediaRemoteLastUsedChanged -= Instance_IsMediaRemoteLastUsedChanged;
            }
            else
            {
                GamepadHelper.Instance.IsMediaRemoteLastUsedChanged += Instance_IsMediaRemoteLastUsedChanged;
            }
        }

        #endregion
    }
}