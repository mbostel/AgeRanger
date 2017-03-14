using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DebitSuccess.AgeRanger.UI.Desktop {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {

            // This is for XAML Binding trace info. All errors/warning will break
            // DebugTraceListener.WriteLine. Just comment these 4 lines if it gets annoying.
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;

            base.OnStartup(e);

            // Create the object graph
            new ContainerBootstrap().Initialise();

        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
        }

        public class DebugTraceListener : TraceListener {
            public override void Write(string message) {
            }

            public override void WriteLine(string message) {
                // Debugger.Break();
            }
        }

    }
}
