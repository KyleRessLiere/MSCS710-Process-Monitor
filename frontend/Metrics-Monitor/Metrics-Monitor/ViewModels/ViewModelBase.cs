using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MetricsMonitorClient.ViewModels {
    public abstract class ViewModelBase : ReactiveObject {

        public void Alert(string message) {
            if (string.IsNullOrEmpty(message)) { return; }

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                       .GetMessageBoxStandardWindow("Alert", message, windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Warning);
            messageBoxStandardWindow.Show();
        }

        public void Alert(string message, string title) {
            if (string.IsNullOrEmpty(message)) { return; }

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                           .GetMessageBoxStandardWindow((title ?? "Alert"), message, windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Info);
            messageBoxStandardWindow.Show();
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
            ;
            messageBoxStandardWindow.Show();
        }

        public void Error(string message, string title, Exception ex) {
            if (string.IsNullOrEmpty(message)) { return; }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Metrics Monitor encountered an error.");
            sb.AppendLine(message);
            sb.AppendLine($"Error Text: {ex.Message}");
            sb.AppendLine(ex.ToString());

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                       .GetMessageBoxStandardWindow((title ?? "Error"), sb.ToString(), windowStartupLocation: Avalonia.Controls.WindowStartupLocation.CenterScreen, icon: MessageBox.Avalonia.Enums.Icon.Error);
            ;
            messageBoxStandardWindow.Show();
        }

    }
}
