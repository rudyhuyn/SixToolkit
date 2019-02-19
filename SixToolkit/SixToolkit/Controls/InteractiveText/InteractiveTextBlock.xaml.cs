using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.UI.Text;
using SixToolkit.Utils;

namespace SixToolkit.Controls
{
    public sealed partial class InteractiveTextBlock : UserControl
    {


        public event EventHandler<string> ActionClicked;

        private Regex _formatRegex = new Regex("{(?<index>\\d+)}", RegexOptions.Multiline);
        private UnderlineStyle _underlineStyle;
        private Dictionary<Hyperlink, int> _hyperlinkToIndex;




        public InteractiveTextBlock()
        {
            _hyperlinkToIndex = new Dictionary<Hyperlink, int>();
            _underlineStyle = UnderlineStyle.None;
            this.InitializeComponent();
            Values = new ObservableCollection<InteractiveTextBlockValue>();
        }

        #region Foreground


        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(InteractiveTextBlock), new PropertyMetadata(null));


        #endregion

        #region ActionForeground



        public Brush ActionForeground
        {
            get { return (Brush)GetValue(ActionForegroundProperty); }
            set { SetValue(ActionForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionForegroundProperty =
            DependencyProperty.Register("ActionForeground", typeof(Brush), typeof(InteractiveTextBlock), new PropertyMetadata(null));



        #endregion

        #region ActionFontWeight



        public FontWeight ActionFontWeight
        {
            get { return (FontWeight)GetValue(ActionFontWeightProperty); }
            set { SetValue(ActionFontWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionFontWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionFontWeightProperty =
            DependencyProperty.Register("ActionFontWeight", typeof(FontWeight), typeof(InteractiveTextBlock), new PropertyMetadata(FontWeights.Normal));



        #endregion

        #region TextWrapping



        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(InteractiveTextBlock), new PropertyMetadata(TextWrapping.NoWrap));



        #endregion

        #region TextAlignment



        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(InteractiveTextBlock), new PropertyMetadata(TextAlignment.Left));




        #endregion


        #region Values
        public ObservableCollection<InteractiveTextBlockValue> Values
        {
            get { return (ObservableCollection<InteractiveTextBlockValue>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(ObservableCollection<InteractiveTextBlockValue>), typeof(InteractiveTextBlock), new PropertyMetadata(null));
        #endregion


        #region Format

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Format.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(InteractiveTextBlock), new PropertyMetadata(null, FormatCallback));

        private static void FormatCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InteractiveTextBlock)d).ManageFormat();
        }

        private void ManageFormat()
        {
            _hyperlinkToIndex.Clear();

            MyTextBlock.Inlines.Clear();
            if (Format != null)
            {


                var previousIndex = 0;
                var runCollection = new List<Inline>();
                foreach (Match match in _formatRegex.Matches(Format))
                {
                    if (previousIndex < match.Index)
                    {
                        MyTextBlock.Inlines.Add(new Run() { Text = Format.Substring(previousIndex, match.Index - previousIndex) });
                    }

                    var index = int.Parse(match.Groups["index"].Value);



                    var hyperlink = new Hyperlink() { UnderlineStyle = _underlineStyle };
                    hyperlink.Click += (sender, e) =>
                    {
                        if (_hyperlinkToIndex.TryGetValue(hyperlink, out int val))
                        {
                            if (val >= 0 && val < Values.Count)
                            {
                                var action = this.Values[val]?.Action;
                                if (action != null)
                                    ActionClicked?.Invoke(this, action);
                            }
                        }
                    };
                    _hyperlinkToIndex[hyperlink] = index;

                    var run = new Run();

                    //bind to Text
                    BindingOperations.SetBinding(run, RunExtension.TextProperty, new Binding()
                    {
                        Path = new PropertyPath("Values[" + index + "].Text"),
                        Source = this
                    });


                    //bind foreground
                    BindingOperations.SetBinding(run, RunExtension.ForegroundProperty, new Binding()
                    {
                        Path = new PropertyPath("ActionForeground"),
                        Source = this
                    });

                    //bind fontWeight
                    BindingOperations.SetBinding(run, RunExtension.FontWeightProperty, new Binding()
                    {
                        Path = new PropertyPath("ActionFontWeight"),
                        Source = this
                    });
                    hyperlink.Inlines.Add(run);


                    //set text
                    MyTextBlock.Inlines.Add(hyperlink);

                    previousIndex = match.Index + match.Length;
                }

                if (previousIndex < Format.Length)
                {
                    MyTextBlock.Inlines.Add(new Run() { Text = Format.Substring(previousIndex) });
                }

            }
        }


        #endregion

    }
}
