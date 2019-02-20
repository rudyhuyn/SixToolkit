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