using System;
using System.ComponentModel;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace SixToolkit.Utils
{
    public class GamepadHelper : INotifyPropertyChanged
    {

        #region Instance
        private static readonly Lazy<GamepadHelper> _instance = new Lazy<GamepadHelper>(() => new GamepadHelper());
        public static GamepadHelper Instance => _instance.Value;
        #endregion

        private readonly bool _isEnable;

        private GamepadHelper()
        {
            _isEnable = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
        }


        public bool IsMediaRemoteLastUsed { get; private set; }

        public event EventHandler<bool> IsMediaRemoteLastUsedChanged;

        public static string GetDeviceId(KeyRoutedEventArgs arg)
        {
            return ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Input.KeyRoutedEventArgs",
                nameof(KeyRoutedEventArgs.DeviceId))
                ? arg.DeviceId
                : null;
        }

        public static string GetDeviceId(KeyEventArgs arg)
        {
            return ApiInformation.IsPropertyPresent("Windows.UI.Core.KeyEventArgs", nameof(KeyEventArgs.DeviceId))
                ? arg.DeviceId
                : null;
        }

        public static bool IsMediaRemote(KeyRoutedEventArgs arg)
        {
            return IsMediaRemote(GetDeviceId(arg));
        }

        private static bool IsMediaRemote(string deviceId)
        {
            return deviceId == "GIP:0000F50000000001";
        }

        public void SetCurrentDeviceId(KeyRoutedEventArgs arg)
        {
            SetCurrentDeviceId(GetDeviceId(arg));
        }

        public void SetCurrentDeviceId(KeyEventArgs arg)
        {
            SetCurrentDeviceId(GetDeviceId(arg));
        }

        private void SetCurrentDeviceId(string deviceId)
        {
            if (!_isEnable)
            {
                return;
            }

            var isMediaRemoteLastUsed = IsMediaRemote(deviceId);
            if (IsMediaRemoteLastUsed == isMediaRemoteLastUsed)
            {
                return;
            }

            IsMediaRemoteLastUsed = isMediaRemoteLastUsed;
            RaisePropertyChanged(nameof(IsMediaRemoteLastUsed));
            IsMediaRemoteLastUsedChanged?.Invoke(this, isMediaRemoteLastUsed);
        }

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}