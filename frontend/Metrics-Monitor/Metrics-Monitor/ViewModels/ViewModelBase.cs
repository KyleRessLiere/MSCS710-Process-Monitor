using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Avalonia;
using NP.Avalonia.Visuals.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Threading;
using log4net;
using Castle.Services.Logging.Log4netIntegration;
using NP.Avalonia.Visuals.Behaviors;

namespace MetricsMonitorClient.ViewModels {
    public abstract class ViewModelBase : ReactiveObject {

      
        public ViewModelBase() {
            AppDomain.CurrentDomain.UnhandledException += ViewModelBase_UnhandledException;
            _logger = log4net.LogManager.GetLogger(typeof(ViewModelBase));
        }

        private void ViewModelBase_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
           Exception ex = e.ExceptionObject as Exception ?? new Exception("An error occurred.");
           if (ex != null) {
               Error("An Uncaught error has occured, closing.", "Uncaught Error", ex);
               Application.Current.DestroyProcess();
            }
        }

        private Window _metricsMonitorMainWindow;
        public Window MetricsMonitorMainWindow {
            get {
                if(_metricsMonitorMainWindow == null) {
                    _metricsMonitorMainWindow = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                }
                return _metricsMonitorMainWindow;
            }
        }

        private ILog _logger;

        public void Alert(string message) {
            if (string.IsNullOrEmpty(message)) { return; }

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                       .GetMessageBoxStandardWindow("Alert", message, windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Warning);
           
            if(MetricsMonitorMainWindow != null) {
                messageBoxStandardWindow.Show(MetricsMonitorMainWindow);
                return;
            }
        }

        public void Alert(string message, string title) {
            if (string.IsNullOrEmpty(message)) { return; }

         

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                           .GetMessageBoxStandardWindow((title ?? "Alert"), message, windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Info);
          
            if (MetricsMonitorMainWindow != null) {
                messageBoxStandardWindow.Show(MetricsMonitorMainWindow);
                return;
            }
        }

        private void ShowError(string message, Exception ex) {
            if (string.IsNullOrEmpty(message)) { return; }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Metrics Monitor encountered an error.");
            sb.AppendLine(message);
            sb.AppendLine($"Error Text: {ex.Message}");
            sb.AppendLine(ex.ToString());

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Error", sb.ToString(), windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Error);

            if (MetricsMonitorMainWindow != null) {
                messageBoxStandardWindow.Show(MetricsMonitorMainWindow);
                return;
            }
        }

        private void ShowError(string message, string title, Exception ex) {
            if (string.IsNullOrEmpty(message)) { return; }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Metrics Monitor encountered an error.");
            sb.AppendLine(message);
            sb.AppendLine($"Error Text: {ex.Message ?? string.Empty}");
            sb.AppendLine(ex.ToString());
            
            if(MetricsMonitorMainWindow == null) { return; }



            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow((title ?? "Error"), sb.ToString(), windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Error);
          
            if (MetricsMonitorMainWindow != null) {
                messageBoxStandardWindow.Show(MetricsMonitorMainWindow);
                return;
            }
        }

        public void Error(string message, Exception ex) {
            Dispatcher.UIThread.InvokeAsync(() => ShowError(message, ex));
        }

        public void Error(string message, string title, Exception ex, bool shutdownOnConfirm = false) {
            if (shutdownOnConfirm) {
                var resp = Dispatcher.UIThread.InvokeAsync(() => ShowError(message, title, ex)).GetAwaiter();

                resp.OnCompleted(() => Application.Current.DestroyProcess());
            }else{
                Dispatcher.UIThread.InvokeAsync(() => ShowError(message, title, ex));
            }
        }

    }
}
