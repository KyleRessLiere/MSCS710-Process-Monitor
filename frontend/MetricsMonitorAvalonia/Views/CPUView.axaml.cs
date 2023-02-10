using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetricsMonitorAvalonia.Views;

public partial class CPUView : UserControl
{
    public CPUView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}