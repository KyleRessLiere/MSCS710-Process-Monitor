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
using MessageBox.Avalonia.DTO;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.BaseWindows.Base;

namespace MetricsMonitorClient.ViewModels {
    public abstract class ViewModelBase : ReactiveObject {
        private ILog _logger;

        public ViewModelBase() {
            AppDomain.CurrentDomain.UnhandledException += ViewModelBase_UnhandledException;
            _logger = log4net.LogManager.GetLogger(typeof(ViewModelBase));
        }
     
        #region Unhandled Exception Handler
        private void ViewModelBase_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Exception ex = e.ExceptionObject as Exception ?? new Exception("An error occurred.");
            if (ex != null) {
                Error("An Uncaught error has occured, closing.", "Uncaught Error", ex, true);
                Application.Current.DestroyProcess();
            }
        }

        #endregion Unhandled Exception Handler
        #region Properties

        private Window _metricsMonitorMainWindow;
        public Window MetricsMonitorMainWindow {
            get {
                if(_metricsMonitorMainWindow == null) {
                    _metricsMonitorMainWindow = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                }
                return _metricsMonitorMainWindow;
            }
        }
        #endregion Properties
        #region Methdos
        public void Alert(string message) {
            if (string.IsNullOrEmpty(message)) { return; }
         
            var messageBox = GetMessageBox(message, false);

            Dispatcher.UIThread.InvokeAsync(() => ShowMessageBox(messageBox));
        }

        public void Alert(string message, string title) {
            if (string.IsNullOrEmpty(message)) { return; }

            var messageBox = GetMessageBox(message,title, false);

            Dispatcher.UIThread.InvokeAsync(() => ShowMessageBox(messageBox));
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
        #endregion Methods
        #region Private Methods

        private static IMsBoxWindow<ButtonResult> GetMessageBox(string message, bool isError = false) {

            var msgParams = new MessageBoxStandardParams {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                CanResize = true,
                Height = 250,
                Icon = (isError ? Icon.Error : Icon.Warning),
                ContentMessage = message,
                ContentTitle = isError ? "Error" : "Alert",
                ShowInCenter = true,
            };

            return MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(msgParams);

        }

        private static IMsBoxWindow<ButtonResult> GetMessageBox(string message, string title, bool isError = false) {
            var msgParams = new MessageBoxStandardParams {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                CanResize = true,
                Height = 250,
                Icon = (isError ? Icon.Error : Icon.Warning),
                ContentMessage = message,
                ContentTitle = title,
                ShowInCenter = true,
            };

            return MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(msgParams);

        }

        private void ShowMessageBox(IMsBoxWindow<ButtonResult> msgBox) {
            if (MetricsMonitorMainWindow != null) {
                msgBox.Show(MetricsMonitorMainWindow);
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

            var messageBox = GetMessageBox(message, true);

            ShowMessageBox(messageBox);
        }

        private void ShowError(string message, string title, Exception ex) {
            if (string.IsNullOrEmpty(message)) { return; }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Metrics Monitor encountered an error.");
            sb.AppendLine(message);
            sb.AppendLine($"Error Text: {ex.Message ?? string.Empty}");
            sb.AppendLine(ex.ToString());

            if (MetricsMonitorMainWindow == null) { return; }

            var messageBox = GetMessageBox(message, title, true);

            ShowMessageBox(messageBox);

        }
        #endregion Private Methods
    }
}
