using Avalonia.Collections;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.Models.CPU;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels
{
    public class CPUViewModel : ViewModelBase {
        private readonly ICPUDataFactory _factory;
        #region Constructor
        public CPUViewModel(ICPUDataFactory factory) {
            _factory = factory;
            DataOptionDict = new AvaloniaDictionary<string, CPUD>();
            DataOptions = new AvaloniaList<string>();
        }
        #endregion Constructor

        #region Properties
        public AvaloniaDictionary<string, CPUD> DataOptionDict {get; private set;}
        public AvaloniaList<string> DataOptions { get; private set;}

        public CPUD SelectedItem { get; private set; }

        private string selectedLabel;
        public string SelectedLabel {
            get { return selectedLabel; }
            set {
                this.RaiseAndSetIfChanged(ref selectedLabel, value);
                if (value == null) { return; }
                CPUD newItem;
                if (DataOptionDict.TryGetValue(value, out newItem)) {
                    SelectedItem = newItem;
                }
            }
        }

        private string resourceText;
        public string ResourceText {
            get { return resourceText; }
            set { this.RaiseAndSetIfChanged(ref resourceText, value); }
        }
        #endregion Properties
        #region Commands
        public ReactiveCommand<Unit,Unit> DoTheThing =>  ReactiveCommand.Create(RefreshData);
        #endregion Commands
        #region Methods
        public void RefreshData() {
            var cpuDataThings = _factory.GetAllRecords();
            if (cpuDataThings == null || !cpuDataThings.Any()) { return; }
            DataOptionDict.Clear();
            DataOptions.Clear();
            foreach (var record in cpuDataThings) {
                if (DataOptionDict.ContainsKey(record.Info)) {
                    DataOptionDict.Remove(record.Info);
                }
                DataOptionDict.Add(record.Info, record);
            }
            if (!DataOptionDict.Any()) { return; }

            DataOptions.AddRange(DataOptionDict.Keys);
        }


        public void TickClock() {
            //do stuff
        }

        #endregion Methods
    }
}
