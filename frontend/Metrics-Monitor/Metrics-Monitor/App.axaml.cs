using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.MonitorSystem;
using MetricsMonitorClient.DI;
using MetricsMonitorClient.ViewModels;
using MetricsMonitorClient.Views;
using ReactiveUI;
using Splat;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                IMemoryFactory? _memorySystemFactory = Locator.Current.GetService<IMemoryFactory>();
                if (_cpuFactory == null) {
                    throw new MissingFieldException("Failed to load Metrics Monitor, could not resolve CPU factory.");
                }
                if (_monitorSystemFactory == null) {
                    throw new MissingFieldException("Failed to load Metrics Monitor, could not resolve Monitor System factory.");
                }
                if (_memorySystemFactory == null) {
                    throw new MissingFieldException("Failed to load Metrics Monitor, could not resolve Memory factory.");
                }


                MainWindowViewModel mainVm = new MainWindowViewModel(_cpuFactory, _monitorSystemFactory, _memorySystemFactory);

                desktopLifetime.MainWindow = new MainWindowView {
                    DataContext = mainVm
                };
            }
            Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
            Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
            base.OnFrameworkInitializationCompleted();
        }


    }
}
