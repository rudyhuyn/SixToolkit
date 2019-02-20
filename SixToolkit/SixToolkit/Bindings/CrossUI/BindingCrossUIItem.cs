// ******************************************************************
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
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace SixToolkit.Bindings.CrossUI
{
    public class BindingCrossUIItem<T>
    {
        public event EventHandler Updated;
        private T _item;
        private bool _autoRaisePropertyChanged;
        public BindingCrossUIItem(T property = default(T), bool autoRaisePropertyChanged = false)
        {
            _item = property;
            _autoRaisePropertyChanged = autoRaisePropertyChanged;
        }

        public T Value
        {
            get
            {
                return _item;
            }
            set
            {
                SetValue(value);
            }
        }

        public Dictionary<CoreDispatcher, BindingCrossUIItemClone<T>> _clones = new Dictionary<CoreDispatcher, BindingCrossUIItemClone<T>>();

        public BindingCrossUIItemClone<T> Clone
        {
            get
            {
                BindingCrossUIItemClone<T> val;
                var dispatcher = Window.Current.Dispatcher;
                if (_clones.TryGetValue(dispatcher, out val))
                {
                    return val;
                }
                else
                {
                    val = new BindingCrossUIItemClone<T>(this, dispatcher);
                    _clones[dispatcher] = val;

                    ApplicationView.GetForCurrentView().Consolidated += (sender, e) =>
                      {
                          if (e.IsAppInitiated || e.IsUserInitiated)
                          {
                              BindingCrossUIItemClone<T> cloneToUnlink;
                              if (_clones.TryGetValue(dispatcher, out cloneToUnlink))
                              {
                                  _clones.Remove(dispatcher);
                                  cloneToUnlink.Unlink();
                              }
                          }
                      };
                    return val;
                }
            }
        }


        public void SetValue(T value)
        {
            if (_item == null)
            {
                if (value == null)
                    return;
            }
            else if (_item.Equals(value))
                return;

            _item = value;
            if(_autoRaisePropertyChanged)
                Updated?.Invoke(this, null);
        }

        public void RaisePropertyChanged()
        {
            Updated?.Invoke(this, null);
        }
    }
}
