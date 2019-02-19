using SixToolkit.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace InteractiveTextBlock
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private string[] _formats = new string[]
{
            "To win a race, the swiftness of a dart. Availeth not without a timely start. The {0} and {1} are my witnesses. Said {1} to the swiftest thing that is, 'I'll bet that you'll not reach, so soon as I. The {2} on yonder hill we spy.'",
            "Hello {1} and {0}, here is {2}!",
            "{2} is now the best friend of {1}, they met during {2}'s wedding!"
};
        private int _formatsIndex = 0;

        public MainPage()
        {
            this.InitializeComponent();
            this.InterTextBlock.Format = _formats[_formatsIndex];
        }

        protected void Text_ActionClicked(object sender, string action)
        {
            new MessageDialog(action).ShowAsync();
        }

        private void ButtonChangeFirstValue_Click(object sender, RoutedEventArgs e)
        {
            InterTextBlock.Values[0].Text = "Human" + new Random().Next(100);
        }

        private void ButtonChangeFormat_Click(object sender, RoutedEventArgs e)
        {
            _formatsIndex = (_formatsIndex + 1) % _formats.Length;
            this.InterTextBlock.Format = _formats[_formatsIndex];
        }


        private void ButtonChangeColor_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            InterTextBlock.ActionForeground = new SolidColorBrush() { Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)) };
        }

        private void ButtonAddValue_Click(object sender, RoutedEventArgs e)
        {
            var value = new Random().Next(100);
            InterTextBlock.Values.Insert(0, new InteractiveTextBlockValue() { Text = "Human" + value, Action = "value=" + value });
        }
    }
}
