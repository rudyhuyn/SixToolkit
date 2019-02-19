using SixToolkit.Bindings.CrossUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossUIBindingSample.ViewModels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        #region Standard Binding
        public int Counter { get; set; } = 0;
        public void IncrementCounter()
        {
            ++Counter;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Counter)));
        }
        #endregion

        #region CrossUIBinding Manual
        public BindingCrossUIItem<int> CounterCrossUIManual { get; set; } = new BindingCrossUIItem<int>(0, false);

        public void IncrementCounterCrossUIManual()
        {
            ++CounterCrossUIManual.Clone.Value;
            CounterCrossUIManual.RaisePropertyChanged();
        }

        #endregion

        #region CrossUIBinding
        public BindingCrossUIItem<int> CounterCrossUI { get; set; } = new BindingCrossUIItem<int>(0, true);

        public void IncrementCounterCrossUI()
        {
            ++CounterCrossUI.Clone.Value;
        }

        #endregion

        #region CrossIndirectUIBinding
        public BindingCrossUIItem<CounterContainer> CounterContainerCrossUI { get; set; } = new BindingCrossUIItem<CounterContainer>(new CounterContainer(), true);
        public void IncrementCounterContainerCrossUI()
        {
            CounterContainerCrossUI.Clone.Value = new CounterContainer() { Value = CounterContainerCrossUI.Value.Value + 1 };
        }

        #endregion

        public BindingCrossUIItem<double> CrossUISliderValue { get; set; } = new BindingCrossUIItem<double>(0d, true);
        public BindingCrossUIItem<bool> CrossUICheckboxValue { get; set; } = new BindingCrossUIItem<bool>(false, true);
        public BindingCrossUIItem<string> CrossUITextboxValue { get; set; } = new BindingCrossUIItem<string>("Nodo, Mango, Apollo", true);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
