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
using System.ComponentModel;
using Windows.UI.Core;

namespace SixToolkit.Bindings.CrossUI
{
    
    public class BindingCrossUIItemClone<T> : INotifyPropertyChanged
    {

        private readonly bool _autoRaisePropertyChanged;
        private readonly CoreDispatcher _dispatcher;
        private readonly BindingCrossUIItem<T> _source;

        internal BindingCrossUIItemClone(BindingCrossUIItem<T> source, CoreDispatcher dispatcher)
        {
            _source = source;
            _dispatcher = dispatcher;
            _source.Updated += Source_Updated;

        }
        public T Value
        {
            get
            {
                return _source.Value;
            }
            set
            {
                _source.SetValue(value);
            }
        }

        internal void Unlink()
        {
            this._source.Updated -= Source_Updated;
        }

        private void Source_Updated(object sender, object s)
        {
            try
            {
                _dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                });
            }
            catch(Exception)
            {
                _source.Updated -= Source_Updated;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
