using CommunityToolkit.Mvvm.Messaging;
using Hardcodet.Wpf.TaskbarNotification;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ShutDown_Scheduler.App;

namespace ShutDown_Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<ShowMainWindowMessage>(this, (_, _) => ShowAndRestore());
        }
        
        // show the main window when the app is started or when the tray icon is clicked
        private void ShowAndRestore()
        {
            this.Show();
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;

            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        // drag the window around
        private void drag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // close the app
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            
            var result = MessageBox.Show
                (
                    "Closing the app will not cancel the shutdown and will not remember the coundtown time on reopen.",
                    "Warning",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.OK
                );

            //MessageBox.Show(result.ToString());

            if (result.ToString() == "OK")
            {
                Application.Current.Shutdown();
            }

        }

        // minimise the app
        private void btn_minimise_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // minimise the app to system tray
        private void btn_systemTray_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            Hide();

            
        }

        private static readonly Regex _regex = new Regex("^[0-9]+$");

        private void iup_seconds_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void iup_hours_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void iup_minutes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        
    }
}