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

using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace SixToolkit.Controls
{
    public enum RectangularSelectionType { REPLACE, ADD, ADD_OR_REMOVE };
    public static class RectangularSelectionHelper
    {
        public static bool SelectionWithRect(this ListViewBase control, Rect rec, RectangularSelectionType selectionType = RectangularSelectionType.REPLACE)
        {

            switch (control.SelectionMode)
            {
                case ListViewSelectionMode.Multiple:
                    if (selectionType == RectangularSelectionType.REPLACE)
                    {
                        control.SelectedItems.Clear();
                    }
                    break;
                case ListViewSelectionMode.Single:
                    {
                        switch (selectionType)
                        {
                            case RectangularSelectionType.REPLACE:
                                control.SelectedItem = null;
                                break;
                            case RectangularSelectionType.ADD:
                            case RectangularSelectionType.ADD_OR_REMOVE:
                                {
                                    if (control.SelectedItem != null)
                                    {
                                        var selectedItem = control.SelectedItem;
                                        control.SelectionMode = ListViewSelectionMode.Multiple;
                                        if (!control.SelectedItems.Contains(selectedItem))
                                        {
                                            control.SelectedItems.Add(selectedItem);
                                        }
                                    }

                                    break;
                                }
                        }
                    }
                    break;
            }
            var selection = VisualTreeHelper.FindElementsInHostCoordinates(rec, control).OfType<SelectorItem>().Where(c => (c is ListViewItem || c is GridViewItem) && c.Content != null).Distinct();
            if (selection.Any())
            {
                control.SelectionMode = ListViewSelectionMode.Multiple;
                foreach (var item in selection)
                {
                    if (control.SelectedItems.Contains(item.Content))
                    {
                        if(selectionType == RectangularSelectionType.ADD_OR_REMOVE)
                        {
                            control.SelectedItems.Remove(item.Content);
                        }
                    }
                    else
                    {
                        control.SelectedItems.Add(item.Content);
                    }
                }
                return true;
            }
            return false;
        }
    }
}
