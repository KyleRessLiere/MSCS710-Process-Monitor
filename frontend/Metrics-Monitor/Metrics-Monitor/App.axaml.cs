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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient {
    public partial class App : Application {


        public DataFactoryBootstrapper _dIPack { get; set; }

        public override void Initialize() {
            //System.Diagnostics.Process.Start("../../../startService.bat");
            startFlaskServer();
         //   startFlaskServer();
            AvaloniaXamlLoader.Load(this);
        }



        private void startFlaskServer() {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\samal\AppData\Local\Microsoft\WindowsApps\python.exe";
            start.Arguments = "app.py";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

           // Task.Run(() => Process.Start(start));

            //using (Process process = Process.Start(start)) {
            //    using (StreamReader reader = process.StandardOutput) {
            //        string result = reader.ReadToEnd();
            //        Console.Write(result);
            //    }
            //}
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
