using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using MetricsMonitorClient.DI;
using MetricsMonitorClient.ViewModels;
using MetricsMonitorClient.Views;
using ReactiveUI;
using Splat;

namespace MetricsMonitorClient {
    public partial class App : Application {
        public DataFactoryBootstrapper _dIPack { get; set; }
        public override void Initialize() {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnFrameworkInitializationCompleted() {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime) {
                _dIPack = new DataFactoryBootstrapper();

                desktopLifetime.MainWindow = new MainWindowView {
                    DataContext = WorkspaceFactory.CreateWorkspace<MainWindowViewModel>()
                };
                Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
                Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
                RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                base.OnFrameworkInitializationCompleted();
            }

        }

       
    }
}
