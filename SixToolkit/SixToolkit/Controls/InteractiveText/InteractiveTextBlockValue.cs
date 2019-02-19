using System.ComponentModel;


namespace SixToolkit.Controls
{
    public class InteractiveTextBlockValue : INotifyPropertyChanged
    {

        #region Text

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text == value)
                    return;
                _text = value;
                OnPropertyChanged(nameof(Text));
            }

        }

        #endregion

        #region Action
        public string Action { get; set; }

        #endregion

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;


    }
}
