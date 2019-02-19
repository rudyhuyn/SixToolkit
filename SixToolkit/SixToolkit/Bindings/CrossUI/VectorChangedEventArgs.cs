using Windows.Foundation.Collections;

namespace SixToolkit.Bindings.CrossUI
{
    internal sealed class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        public VectorChangedEventArgs(CollectionChange action, object item, int index)
        {
            CollectionChange = action;

            Index = (uint)index;
            Item = item;
        }

        /// <summary>
        ///     Gets the affected item.
        /// </summary>
        public object Item { get; }

        /// <summary>
        ///     Gets the type of change that occurred in the vector.
        /// </summary>
        public CollectionChange CollectionChange { get; }

        /// <summary>
        ///     Gets the position where the change occurred in the vector.
        /// </summary>
        public uint Index { get; }
    }
}