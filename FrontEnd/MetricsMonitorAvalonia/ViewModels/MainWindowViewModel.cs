namespace MetricsMonitorAvalonia.ViewModels {
    public class MainWindowViewModel : ViewModelBase {
        public MainWindowViewModel() {
            CurrentResourceViewModel = new ViewModelBase();
            Name = string.Empty;
        }

        public ViewModelBase CurrentResourceViewModel { get; set; }

        public string Name { get; set; }
    }
}





    