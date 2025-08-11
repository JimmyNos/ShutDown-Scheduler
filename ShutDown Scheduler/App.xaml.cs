using CommunityToolkit.Mvvm.Messaging;
using Hardcodet.Wpf.TaskbarNotification;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ShutDown_Scheduler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon? _trayIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var contextMenu = new ContextMenu();

            var abortShutdownItem = new MenuItem
            {
                Header = "Abort Shutdown"
            };
            abortShutdownItem.Click += (_, _) => WeakReferenceMessenger.Default.Send(new AbortShutdownMessage());

            var exitItem = new MenuItem
            {
                Header = "Exit"
            };
            exitItem.Click += (_, _) => WeakReferenceMessenger.Default.Send(new ExitAppMessage());

            contextMenu.Items.Add(abortShutdownItem);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(exitItem);

            _trayIcon = new TaskbarIcon
            {
                Icon = new System.Drawing.Icon("./Resources/Icons/shutdown-icon-11823-Windows.ico"),
                ToolTipText = "My App",
                ContextMenu = contextMenu
            };

            _trayIcon.TrayLeftMouseUp += TrayIcon_TrayLeftMouseUp;
        }

        public record ShowMainWindowMessage;
        public record AbortShutdownMessage;
        public record ExitAppMessage;



        private void TrayIcon_TrayLeftMouseUp(object? sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new ShowMainWindowMessage());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }

}
