using System.Collections.Generic;
using System.Collections.Specialized;

namespace SixToolkit.Bindings.CrossUI
{
    public class ObservableCollectionChange
    {
        public NotifyCollectionChangedAction Action { get; set; }

        public List<object> NewItems { get; set; }

        public int NewStartingIndex { get; set; }

        public List<object> OldItems { get; set; }

        public int OldStartingIndex { get; set; }
    }
}