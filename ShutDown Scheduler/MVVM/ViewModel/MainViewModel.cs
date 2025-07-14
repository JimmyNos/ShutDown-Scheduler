using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Windows;
using static ShutDown_Scheduler.App;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShutDown_Scheduler.MVVM.ViewModel
{
    partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateLabel))]
        private int seconds;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateLabel))]
        private int minutes;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateLabel))]
        private int hours;

        [ObservableProperty]
        private string countdownLabel;

        [ObservableProperty]
        private string shutdownlabel = "shutdown in..";

        [ObservableProperty]
        private bool isUpDownEnable;

        [ObservableProperty]
        private Visibility updateLabelVisibility;

        [ObservableProperty]
        private Visibility countdownLabelVisibility;



        public int remainingHours;

        public int remainingMinutes;

        public int remainingSeconds;

        public string UpdateLabel => Update();

        private string Update()
        {
            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes++;
                if (Minutes == 60)
                {
                    Minutes = 0;
                    if (Hours <= 99)
                    {
                        Hours++;
                    }
                }
            }
            else if (Seconds < 0)
                Seconds = 59;

            if (Minutes == 60)
            {
                Minutes = 0;
                if (Hours <= 99) Hours++;
            }
            else if (Minutes < 0)
                Minutes = 59;

            if (Hours == 100)
            {
                Hours = 0;
            }
            else if (Hours == -1)
                Hours = 99;

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

            return CountdownLabel;
        }


        public MainViewModel()
        {
            IsUpDownEnable = true;
            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

            UpdateLabelVisibility = Visibility.Visible;
            CountdownLabelVisibility = Visibility.Collapsed;

            WeakReferenceMessenger.Default.Register<AbortShutdownMessage>(this, (_, _) => SysTrayAbort());
            WeakReferenceMessenger.Default.Register<ExitAppMessage>(this, (_, _) => SysTrayExitApp());
        }

        [RelayCommand]
        private void SecondsUp()
        {
            if (Seconds >= 0 && Seconds <= 60)
            {
                Seconds++;
            }

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        }

        [RelayCommand]
        private void SecondsDown()
        {
            if (Seconds >= 1 && Seconds <= 60)
                Seconds--;
            else Seconds = 59;

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        }

        [RelayCommand]
        private void MinutesUp()
        {
            if (Minutes >= 0 && Minutes <= 60)
                Minutes++;

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        }

        [RelayCommand]
        private void MinutesDown()
        {
            if (Minutes >= 1 && Minutes <= 60)
                Minutes--;
            else Minutes = 59;

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        }

        [RelayCommand]
        private void HoursUp()
        {
            if (Hours >= 0 && Hours <= 100)
                Hours++;

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        }

        [RelayCommand]
        private void HoursDown()
        {
            if (Hours >= 0 && Hours <= 100)
                Hours--;
            else Hours = 99;

            CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        }


        public int countdown;
        public bool isAbort = false;

        // exit command from the system tray icon
        [RelayCommand]
        private void SysTrayExitApp()
        {
            //Abort();
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

        // abort shutdown command from the system tray icon
        [RelayCommand]
        private void SysTrayAbort()
        {
            Abort();
        }

        // abort shutdown command
        [RelayCommand]
        private void Abort()
        {
            //Process.Start("shutdown", "/a");
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/a",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(processStartInfo);
            if (process != null)
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                //process.WaitForExit();

                isAbort = true;
                IsUpDownEnable = true;
                CountdownLabel = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
                UpdateLabelVisibility = Visibility.Visible;
                CountdownLabelVisibility = Visibility.Collapsed;
                if (error != null)
                {
                    if(error.Contains("(1116)"))
                    {
                        Shutdownlabel = "No Shutdown Scheduled";
                    }
                    else
                        Shutdownlabel = "Shutdown Aborted";
                    isAbort = true;
                    //var result = MessageBox.Show
                    //(
                    //    error,
                    //    "Warning",
                    //    MessageBoxButton.OK,
                    //    MessageBoxImage.Exclamation,
                    //    MessageBoxResult.OK
                    //);

                    ////MessageBox.Show(result.ToString());

                    //if (result.ToString() == "OK")
                    //{
                    //    isAbort = true;
                    //}
                }
                //MessageBox.Show($"output: {output}\nerror{error}");
            }
            //MessageBox.Show($"aborting shutdown");

        }

        // shutdown command
        [RelayCommand]
        private void Shutdown() 
        {
            countdown = CountdownTime(Seconds, Minutes, Hours);
            //MessageBox.Show($"will shutdown in {Hours},{Minutes},{Seconds}");

            //Process.Start("shutdown", $"/s /f /t {countdown}");
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = $"/s /f /t {countdown}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(processStartInfo);
            if (process != null)
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                isAbort = false;
                IsUpDownEnable = false;
                UpdateLabelVisibility = Visibility.Collapsed;
                CountdownLabelVisibility = Visibility.Visible;
                Task.Run(Countdown);
                if (error != null)
                {
                    if (error.Contains("(1190)"))
                    {
                        Shutdownlabel = "Shutdown Already Scheduled";
                    }
                    else
                        Shutdownlabel = "Shutdown Scheduled";
                }
            }

        }

        // calculate the total countdown time in seconds
        public int CountdownTime(int seconds, int minutes, int hours)
        {
            hours = hours * 3600;
            minutes = minutes * 60;

            int totalSeconds = (hours + minutes + seconds);

            if (totalSeconds == 0)
            {
                return 500;
            }
            else
                return totalSeconds;
        }

        // countdown method
        public async Task Countdown()
        {
            for (int second = countdown; second >= 0; second--)
            {
                if (isAbort == true)
                {
                    break;
                }
                remainingHours = second / 3600;
                remainingMinutes = (second % 3600) / 60;
                remainingSeconds = second % 60;

                CountdownLabel = $"{remainingHours:D2}:{remainingMinutes:D2}:{remainingSeconds:00}";

                await Task.Delay(1000);
            }
        }

        
    }
}
