using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetricsMonitorAvalonia.Views;

public partial class MemoryView : UserControl
{
    public MemoryView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}