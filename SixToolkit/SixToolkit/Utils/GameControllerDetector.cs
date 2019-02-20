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
using System.ComponentModel;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace SixToolkit.Utils
{
    public class GameControllerDetector : INotifyPropertyChanged
    {
        #region Singleton  
        public static GameControllerDetector Instance { get; }

        static GameControllerDetector()
        {
            Instance = new GameControllerDetector();
        }
        #endregion

        public enum GameControllerType { GAMEPAD, MEDIAREMOTE };
        public event EventHandler<GameControllerType> GameControllerChanged;
        public GameControllerType CurrentControllerType { get; private set; }

        private readonly bool _isEnable;

        private GameControllerDetector()
        {
            _isEnable = true;// AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
        }

        private static string GetDeviceId(KeyRoutedEventArgs arg)
        {
            return ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Input.KeyRoutedEventArgs", "DeviceId") ? arg.DeviceId : null;
        }

        private static string GetDeviceId(KeyEventArgs arg)
        {
            return ApiInformation.IsPropertyPresent("Windows.UI.Core.KeyEventArgs", "DeviceId") ? arg.DeviceId : null;
        }

        public static GameControllerType GetGameControllerType(KeyRoutedEventArgs e)
        {
            return GetGameControllerType(GetDeviceId(e));
        }

        public static GameControllerType GetGameControllerType(KeyEventArgs e)
        {
            return GetGameControllerType(GetDeviceId(e));
        }

        private static GameControllerType GetGameControllerType(string deviceId)
        {
            return deviceId == "GIP:0000F50000000001" ? GameControllerType.MEDIAREMOTE : GameControllerType.GAMEPAD;
        }

        public void AnalyzeKeyEvent(KeyRoutedEventArgs arg)
        {
            SetCurrentDeviceId(GetDeviceId(arg));
        }

        public void RegisterCurrentWindow()
        {
            if (!_isEnable)
            {
                return;
            }

            var coreWindow = CoreWindow.GetForCurrentThread();
            if (coreWindow == null)
            {
                return;
            }

            coreWindow.KeyDown -= CoreWindow_KeyDown;
            coreWindow.KeyDown += CoreWindow_KeyDown;
        }
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            AnalyzeKeyEvent(args);
        }

        public void AnalyzeKeyEvent(KeyEventArgs arg)
        {
            SetCurrentDeviceId(GetDeviceId(arg));
        }

        private void SetCurrentDeviceId(string deviceId)
        {
            if (!_isEnable)
            {
                return;
            }

            var controllerType = GetGameControllerType(deviceId);
            if (CurrentControllerType == controllerType)
            {
                return;
            }

            CurrentControllerType = controllerType;
            RaisePropertyChanged(nameof(CurrentControllerType));
            GameControllerChanged?.Invoke(this, controllerType);
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