using SixToolkit.Controls;
using SixToolkit.Utils;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace XboxToolsSample
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            var controllerDetector = GameControllerDetector.Instance;
            ManageGameControllerType(controllerDetector.CurrentControllerType);
            controllerDetector.GameControllerChanged += Instance_GameControllerChanged;
            this.PreviewKeyDown += MainPage_PreviewKeyDown;
        }

        private void Instance_GameControllerChanged(object sender, GameControllerDetector.GameControllerType e)
        {
            ManageGameControllerType(GameControllerDetector.Instance.CurrentControllerType);
        }

        private void ManageGameControllerType(GameControllerDetector.GameControllerType currentControllerType)
        {
            switch(currentControllerType)
            {
                case GameControllerDetector.GameControllerType.GAMEPAD:
                    GamepadTextBlock.Visibility = Visibility.Visible;
                    MediaRemoteTextBlock.Visibility = Visibility.Collapsed;
                    break;
                case GameControllerDetector.GameControllerType.MEDIAREMOTE:
                    GamepadTextBlock.Visibility = Visibility.Collapsed;
                    MediaRemoteTextBlock.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void MainPage_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var gameControllerDetector = GameControllerDetector.Instance;
            gameControllerDetector.AnalyzeKeyEvent(e);

            if (gameControllerDetector.CurrentControllerType == GameControllerDetector.GameControllerType.GAMEPAD)
            {
                switch (e.Key)
                {
                    case Windows.System.VirtualKey.GamepadX:
                        new MessageDialog("Search").ShowAsync();
                        break;
                    case Windows.System.VirtualKey.GamepadY:
                        new MessageDialog("Add user").ShowAsync();
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Windows.System.VirtualKey.GamepadView:
                        new MessageDialog("Search").ShowAsync();
                        break;
                    case Windows.System.VirtualKey.GamepadMenu:
                        new MessageDialog("Add user").ShowAsync();
                        break;
                }
            }
        }
    }
}
