using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MetricsMonitorAvalonia.Views;

public partial class MainWindowView : Window
{
    public MainWindowView()
    {
        InitializeComponent();
    }

    //private void nav_Checked(object sender, RoutedEventArgs e)
    //{
    //    string senderName = (sender as RadioButton).Name;
    //    switch (senderName)
    //    {
    //        case "process_btn": CC.Content = new ProcessUserControl(); break;
    //        case "memory_btn": CC.Content = new MemoryUserControl(); break;
    //        case "cpu_btn": CC.Content = new CpuUserControl(); break;
    //        default:
    //            break;
    //    }

    //}
}