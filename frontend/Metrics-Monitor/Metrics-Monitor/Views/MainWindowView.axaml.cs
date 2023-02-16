using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetricsMonitorClient.Views {
    public partial class MainWindowView : Window {
        public MainWindowView() {
            InitializeComponent();
            this.AttachDevTools();
        }
        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
