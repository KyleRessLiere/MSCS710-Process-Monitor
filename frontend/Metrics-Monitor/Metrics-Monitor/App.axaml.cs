using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MetricsMonitorClient.DI;
using MetricsMonitorClient.Services;
using MetricsMonitorClient.ViewModels;
using MetricsMonitorClient.Views;
using Splat;
using System;
using System.Linq.Expressions;

namespace MetricsMonitorClient {
    public partial class App : Application {
        public override void Initialize() {
            AvaloniaXamlLoader.Load(this);
        }
        public DataFactoryBootstrapper _dIPack {get;set;}
        public override void OnFrameworkInitializationCompleted() {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
                    _dIPack = new DataFactoryBootstrapper();

                    ICPUDataFactory? _cpuFactory = Locator.Current.GetService<ICPUDataFactory>();

                    if (_cpuFactory == null) {
                        throw new MissingFieldException("Failed to load Metrics Monitor, could not resolve CPU factory.");
                    }



                    desktop.MainWindow = new MainWindowView {
                        DataContext = new MainWindowViewModel(),
                    };
                }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
