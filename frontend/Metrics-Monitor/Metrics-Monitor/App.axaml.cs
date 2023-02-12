using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.MonitorSystem;
using MetricsMonitorClient.DI;
using MetricsMonitorClient.ViewModels;
using MetricsMonitorClient.Views;
using Splat;
using System;

namespace MetricsMonitorClient {
    public partial class App : Application {


        public DataFactoryBootstrapper _dIPack { get; set; }

        public override void Initialize() {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted() {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime) {
                _dIPack = new DataFactoryBootstrapper();

                ICPUDataFactory? _cpuFactory = Locator.Current.GetService<ICPUDataFactory>();
                IMonitorSystemFactory? _monitorSystemFactory = Locator.Current.GetService<IMonitorSystemFactory>();
                if (_cpuFactory == null) {
                    throw new MissingFieldException("Failed to load Metrics Monitor, could not resolve CPU factory.");
                }
                if (_monitorSystemFactory == null) {
                    throw new MissingFieldException("Failed to load Metrics Monitor, could not resolve Monitor System factory.");
                }


                MainWindowViewModel mainVm = new MainWindowViewModel(_cpuFactory, _monitorSystemFactory);

                desktopLifetime.MainWindow = new MainWindowView {
                    DataContext = mainVm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}
