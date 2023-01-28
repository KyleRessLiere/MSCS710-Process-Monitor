using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using MetricsMonitor.Views;

namespace MetricsMonitor {
    public partial class App : PrismApplication {

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton<Services.ICPUDataFactory, Services.CPUDataFactory>();
        }
        protected override Window CreateShell() {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

    }
}