using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Core;

namespace SixToolkit.Bindings.CrossUI
{
    public class ObservableCollectionLocalClone<T, BaseT> :
        IObservableVector<BaseT>, IReadOnlyList<BaseT>, INotifyPropertyChanged
        where BaseT : class
        where T : class, BaseT
    {
        private readonly ObservableCollectionCrossUI<T> _observable;

        private readonly CoreDispatcher Dispatcher;

        public ObservableCollectionLocalClone(CoreDispatcher dispatcher, ObservableCollectionCrossUI<T> source)
        {
            Dispatcher = dispatcher;
            _observable = source;
#pragma warning disable 4014
            InitAsync();
#pragma warning restore 4014
        }

        public bool IsFixedSize => false;

        public bool IsSynchronized => false;

        public object SyncRoot => null;
        public event VectorChangedEventHandler<BaseT> VectorChanged;

        public void Add(BaseT item)
        {
        }

        public void Clear()
        {
        }

        public bool Contains(BaseT item)
        {
            if (item is T itemT)
            {
                return _observable.Contains(itemT);
            }

            return false;
        }

        public void CopyTo(BaseT[] array, int arrayIndex)
        {
        }

        public bool Remove(BaseT item)
        {
            return false;
        }

        public int Count => _observable.Count;

        public bool IsReadOnly { get; } = true;

        public void RemoveAt(int index)
        {
            _observable.RemoveAt(index);
        }

        public BaseT this[int i]
        {
            get => _observable[i];
            set
            {
                if (value is T value1)
                {
                    _observable[i] = value1;
                }
            }
        }

        public IEnumerator<BaseT> GetEnumerator()
        {
            return _observable.OfType<BaseT>().GetEnumerator();
        }

        public void Insert(int index, BaseT item)
        {
            if (item is T tItem)
            {
                _observable.Insert(index, tItem);
            }
        }

        int IList<BaseT>.IndexOf(BaseT item)
        {
            return IndexOf(item);
        }

        private async Task InitAsync()
        {
            await Task.Delay(50);
            _observable.Changed += InternalCollection_Changed;
        }

        public void Unload()
        {
            _observable.Changed -= InternalCollection_Changed;
        }

        ~ObservableCollectionLocalClone()
        {
            Unload();
        }

        private void InternalCollection_Changed(object sender, ObservableCollectionChange e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            for (var i = 0; i < e.NewItems.Count; ++i)
                            {
                                OnVectorChanged(CollectionChange.ItemInserted, (T)e.NewItems[i], e.NewStartingIndex + i);
                            }

                            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () => { RaisePropertyChanged(nameof(Count)); });
                            break;
                        }
                    case NotifyCollectionChangedAction.Remove:
                        {
                            for (var i = 0; i < e.OldItems.Count; ++i)
                            {
                                OnVectorChanged(CollectionChange.ItemRemoved, (T)e.OldItems[i], e.OldStartingIndex + i);
                            }

                            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () => { RaisePropertyChanged(nameof(Count)); });
                            break;
                        }
                    case NotifyCollectionChangedAction.Replace:
                        {
                            for (var i = 0; i < e.OldItems.Count; ++i)
                            {
                                OnVectorChanged(CollectionChange.ItemChanged, (T)e.NewItems[i], e.OldStartingIndex + i);
                            }

                            break;
                        }
                    case NotifyCollectionChangedAction.Move:
                        {
                            for (var i = 0; i < e.OldItems.Count; ++i)
                            {
                                InternalCollectionChangedMove((T)e.OldItems[i], e.OldStartingIndex + i,
                                e.NewStartingIndex + i);
                            }

                            break;
                        }
                    case NotifyCollectionChangedAction.Reset:
                        OnVectorChanged(CollectionChange.Reset, null, 0);
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () => { RaisePropertyChanged(nameof(Count)); });
                        break;
                }
            }
            catch
            {
            }
        }

        private void InternalCollectionChangedMove(T item, int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
            {
                return;
            }

            OnVectorChanged(CollectionChange.ItemRemoved, item, oldIndex);
            OnVectorChanged(CollectionChange.ItemInserted, item, newIndex);
        }

        private void OnVectorChanged(CollectionChange action, T item, int index)
        {
            if (VectorChanged == null)
            {
                return;
            }

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => { VectorChanged?.Invoke(this, new VectorChangedEventArgs(action, item, index)); });
        }

        public int IndexOf(BaseT current)
        {
            if (!(current is T))
            {
                return -1;
            }

            return _observable.IndexOf((T)current);
        }

        #region IList

        public bool Contains(object value)
        {
            var item = value as BaseT;
            return Contains(item);
        }

        public int IndexOf(object value)
        {
            if (!(value is BaseT item))
            {
                return -1;
            }

            return IndexOf(item);
        }

        public void Insert(int index, object value)
        {
        }

        public void Remove(object value)
        {
        }

        public void CopyTo(Array array, int index)
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region property changed

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}