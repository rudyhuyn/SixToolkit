using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SixToolkit.Bindings.CrossUI
{
    public class ObservableCollectionCrossUI<T> : ObservableCollection<T>
        where T : class
    {
        private readonly Dictionary<CoreDispatcher, ObservableCollectionLocalClone<T, object>> _clones =
            new Dictionary<CoreDispatcher, ObservableCollectionLocalClone<T, object>>();

        public ObservableCollectionCrossUI()
        {
            CollectionChanged += Source_CollectionChanged;
        }

        public ObservableCollectionLocalClone<T, object> Clone
        {
            get
            {
                var dispatcher = Window.Current.Dispatcher;
                if (_clones.TryGetValue(dispatcher, out var clone))
                {
                    return clone;
                }

                var localClone = new ObservableCollectionLocalClone<T, object>(dispatcher, this);
                _clones[dispatcher] = localClone;
                return localClone;
            }
        }

        public event EventHandler<ObservableCollectionChange> Changed;

        private void Source_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            var change = new ObservableCollectionChange
            {
                NewItems = e.NewItems?.OfType<object>()?.ToList(),
                OldItems = e.OldItems?.OfType<object>()?.ToList(),
                Action = e.Action,
                NewStartingIndex = e.NewStartingIndex,
                OldStartingIndex = e.OldStartingIndex
            };
            Changed?.Invoke(this, change);
        }
    }
}