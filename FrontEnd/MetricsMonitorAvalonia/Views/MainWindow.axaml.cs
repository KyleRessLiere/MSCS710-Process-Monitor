using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MetricsMonitorAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void process_btn_Checked(object sender, RoutedEventArgs e)
    {
        CC.Content = new ProcessUserControl();
    }
    private void memory_btn_Checked(object sender, RoutedEventArgs e)
    {
        CC.Content = new MemoryUserControl();
    }
    private void cpu_btn_Checked(object sender, RoutedEventArgs e)
    {
        CC.Content = new ProcessUserControl();
    }
}