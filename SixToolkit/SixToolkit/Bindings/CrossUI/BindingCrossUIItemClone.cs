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
