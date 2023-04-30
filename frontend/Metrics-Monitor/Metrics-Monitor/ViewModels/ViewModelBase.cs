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

namespace MetricsMonitorClient.ViewModels {
    public abstract class ViewModelBase : ReactiveObject {

        private Window _metricsMonitorMainWindow;
        public Window MetricsMonitorMainWindow {
            get {
                if(_metricsMonitorMainWindow == null) {
                    _metricsMonitorMainWindow = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                }
                return _metricsMonitorMainWindow;
            }
        }


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

        public void Error(string message, Exception ex) {
            if (string.IsNullOrEmpty(message)) { return; }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Metrics Monitor encountered an error.");
            sb.AppendLine(message);
            sb.AppendLine($"Error Text: {ex.Message}");
            sb.AppendLine(ex.ToString());

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                       .GetMessageBoxStandardWindow("Error", sb.ToString(), windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Error);

            if (MetricsMonitorMainWindow != null) {
                messageBoxStandardWindow.Show(MetricsMonitorMainWindow);
                return;
            }
        }

        public void Error(string message, string title, Exception ex) {
            if (string.IsNullOrEmpty(message)) { return; }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Metrics Monitor encountered an error.");
            sb.AppendLine(message);
            sb.AppendLine($"Error Text: {ex.Message ?? string.Empty}");
            sb.AppendLine(ex.ToString());

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                       .GetMessageBoxStandardWindow((title ?? "Error"), sb.ToString(), windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Error);
            if (MetricsMonitorMainWindow != null) {
                messageBoxStandardWindow.Show(MetricsMonitorMainWindow);
                return;
            }
        }

    }
}
