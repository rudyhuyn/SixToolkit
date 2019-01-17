// Copyright (c) Rudy Huyn.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Huyn
{
    public static class RectangularSelectionHelper
    {
        public static bool SelectionWithRect(this ListViewBase control, Rect rec)
        {

            switch (control.SelectionMode)
            {
                case ListViewSelectionMode.Multiple:
                    control.SelectedItems.Clear();
                    break;
                case ListViewSelectionMode.Single:
                    control.SelectedItem = null;
                    break;
            }
            var selection = VisualTreeHelper.FindElementsInHostCoordinates(rec, control).OfType<SelectorItem>().Where(c => (c is ListViewItem || c is GridViewItem) && c.Content != null).Distinct();
            if (selection.Any())
            {
                control.SelectionMode = ListViewSelectionMode.Multiple;
                foreach (var item in selection)
                {
                    control.SelectedItems.Add(item.Content);
                }
                return true;
            }
            return false;
        }
    }
}
