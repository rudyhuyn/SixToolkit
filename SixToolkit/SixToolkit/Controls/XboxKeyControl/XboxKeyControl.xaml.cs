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

using SixToolkit.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace SixToolkit.Controls
{

    public sealed partial class XboxKeyControl : UserControl
    {
        public enum Key { Unknown, X, Y, A, B, View, Play, Menu }
        public enum KeyTheme { ColorBackground, DarkBackground }

        private readonly Brush _whiteBrush;
        private readonly Brush _blackBrush;

        public XboxKeyControl()
        {
            _whiteBrush = new SolidColorBrush(Colors.White);
            _blackBrush = new SolidColorBrush(Colors.Black);
            InitializeComponent();
        }

        #region Symbol

        public Key Symbol
        {
            get => (Key)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof(Key), typeof(XboxKeyControl), new PropertyMetadata(Key.Unknown, SymbolCallback));

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

        public static readonly DependencyProperty SymbolMediaProperty =
         DependencyProperty.Register("SymbolMedia", typeof(Key), typeof(XboxKeyControl), new PropertyMetadata(Key.Unknown, SymbolMediaCallback));

        private static void SymbolMediaCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((XboxKeyControl)d).SymbolMediaCallback(e.NewValue);

        }

        private void SymbolMediaCallback(object newValue)
        {
            UpdateButton();
            GameControllerDetector.Instance.GameControllerChanged -= GameControllerDetector_GameControllerChanged;
            if (newValue != null)
            {
                GameControllerDetector.Instance.GameControllerChanged += GameControllerDetector_GameControllerChanged;
            }
        }

        private void GameControllerDetector_GameControllerChanged(object sender, GameControllerDetector.GameControllerType e)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                UpdateButton();
            });
        }

        #endregion

        #region KeyTheme

        public KeyTheme Theme
        {
            get => (KeyTheme)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        // Using a DependencyProperty as the backing store for Theme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(KeyTheme), typeof(XboxKeyControl), new PropertyMetadata(KeyTheme.ColorBackground, ThemeCallback));

        private static void ThemeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((XboxKeyControl)d).UpdateButton();
        }


        #endregion


        private void UpdateButton()
        {

            var symbol = Symbol;
            if (GameControllerDetector.Instance.CurrentControllerType == GameControllerDetector.GameControllerType.MEDIAREMOTE && SymbolMedia != Key.Unknown)
            {
                symbol = SymbolMedia;
            }

            switch (symbol)
            {
                case Key.A:
                    SetMainKeyColor((Brush)Resources["XboxGreenBrush"]);
                    Label.Text = "A";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;

                    break;
                case Key.B:
                    SetMainKeyColor((Brush)Resources["XboxRedBrush"]);
                    Label.Text = "B";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    break;
                case Key.X:
                    SetMainKeyColor((Brush)Resources["XboxBlueBrush"]);
                    Label.Text = "X";
                    Label.Visibility = Visibility.Visible;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;

                    break;
                case Key.Y:
                    SetMainKeyColor((Brush)Resources["XboxDarkYellowBrush"]);
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
                    MenuPath.Visibility = Visibility.Collapsed;
                    break;
                case Key.Play:
                    CircleBackground.Fill = new SolidColorBrush(Colors.Black);
                    Label.Visibility = Visibility.Collapsed;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Visible;
                    MenuPath.Visibility = Visibility.Collapsed;
                    break;
                case Key.Menu:
                    CircleBackground.Fill = new SolidColorBrush(Colors.Black);
                    Label.Visibility = Visibility.Collapsed;
                    ViewPath.Visibility = Visibility.Collapsed;
                    PlayPath.Visibility = Visibility.Collapsed;
                    MenuPath.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SetMainKeyColor(Brush colorBrush)
        {
            if (Theme == KeyTheme.ColorBackground)
            {
                CircleBackground.Fill = colorBrush;
                Label.Foreground = _whiteBrush;
            }
            else
            {
                Label.Foreground = colorBrush;
                CircleBackground.Fill = _blackBrush;
            }
        }
    }
}
