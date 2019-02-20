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