using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitor.ViewModels {
    public class MainWindowViewModel : BindableBase {
        private Services.ICPUDataFactory _cpuData;

        public MainWindowViewModel(Services.ICPUDataFactory cpuData) {
            _cpuData = cpuData;
            _selectedRecord = string.Empty;
        }


        public ObservableCollection<string> Records { get; private set; } =
            new ObservableCollection<string>();


        private string _selectedRecord;
        public string SelectedRecord {
            get => _selectedRecord;
            set {
                if (SetProperty<string>(ref _selectedRecord, value)) {
                    Debug.WriteLine(_selectedRecord ?? "no record selected");
                }
            }
        }

        private DelegateCommand? _commandLoad;
        public DelegateCommand CommandLoad =>
            _commandLoad ?? (_commandLoad = new DelegateCommand(CommandLoadExecute));

        private void CommandLoadExecute() {
            Records.Clear();
            List<string> list = _cpuData.GetAllRecords().ToList();
            foreach (string item in list)
                Records.Add(item);
        }
    }
}