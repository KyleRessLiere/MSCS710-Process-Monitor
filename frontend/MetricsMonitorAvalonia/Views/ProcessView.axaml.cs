using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetricsMonitorAvalonia.Views;

public partial class ProcessorView : UserControl
{
    public ProcessorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}