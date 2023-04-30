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
                       .GetMessageBoxStandardWindow("Alert", message);
            messageBoxStandardWindow.Show();
        }

        public void Alert(string message, string title) {
            if (string.IsNullOrEmpty(message)) { return; }

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                           .GetMessageBoxStandardWindow((title ?? "Alert"), message);
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
                       .GetMessageBoxStandardWindow("Error", sb.ToString());
            ;
            messageBoxStandardWindow.Show();
        }

    }
}
