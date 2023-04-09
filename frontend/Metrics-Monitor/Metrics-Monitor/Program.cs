using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsMonitorClient {
    internal class Program {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) {
            startFlaskServer();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
       
        
        
        
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI();

        public static void startFlaskServer() {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\samal\AppData\Local\Microsoft\WindowsApps\python.exe";
            start.Arguments = "app.py";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            Task.Run(() => Process.Start(start));
           
        }

    }
}
