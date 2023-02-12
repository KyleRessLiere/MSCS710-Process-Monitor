using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetricsMonitorClient.Views
{
    public partial class CPUView : UserControl {
        public CPUView() {
            InitializeComponent();
        }
        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
