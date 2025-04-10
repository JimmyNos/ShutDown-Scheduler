using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShutDown_Scheduler.MVVM.ViewModel
{
    partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateSeconds))]
        [NotifyPropertyChangedFor(nameof(ResetSeconds))]
        private int seconds;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateMinutes))]
        [NotifyPropertyChangedFor(nameof(ResetMinutes))]
        private int minutes;

        [ObservableProperty]
        private int hours;

        public int countdown;

        [RelayCommand]
        private void Abort()
        {
            //Process.Start("shutdown", "/a");
            MessageBox.Show($"aborting shutdown");

        }

        [RelayCommand]
        private void Shutdown() 
        {
            countdown = Countdown(Seconds, Minutes, Hours);

            //Process.Start("shutdown", $"/s /f /t {countdown}");
            MessageBox.Show($"will shutdown in {Hours},{Minutes},{Seconds}");
        }

        public int Countdown(int seconds, int minutes, int hours)
        {
            hours = hours * 60 * 60;
            minutes = minutes * 60;

            int totalSeconds = (hours + minutes + seconds);

            if (totalSeconds == 0)
            {
                return 500;
            }
            else
                return totalSeconds;
        }


        [RelayCommand]
        private void SecondsUp()
        {
            if (Seconds >= 0 && Seconds <= 60) 
                Seconds++;

            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes++;
                if (Minutes == 60)
                {
                    Minutes = 0;
                    if (Hours <= 98) Hours++;
                }
            }
                
        }

        [RelayCommand]
        private void SecondsDown()
        {
            if (Seconds >= 1 && Seconds <= 60)
                Seconds--;
            else Seconds = 59;
        }

        [RelayCommand]
        private void MinutesUp()
        {
            if (Minutes >= 0 && Minutes <= 60)
                Minutes++;

            if (Minutes == 60)
            {
                Minutes = 0;
                if (Hours <= 98) Hours++;
            }
        }

        [RelayCommand]
        private void MinutesDown()
        {
            if (Minutes >= 1 && Minutes <= 60)
                Minutes--;
            else Minutes = 59;
        }

        [RelayCommand]
        private void HoursUp()
        {
            if (Hours >= 0 && Hours <= 98)
                Hours++;
        }

        [RelayCommand]
        private void HoursDown()
        {
            if (Hours >= 1 && Hours <= 99)
                Hours--;
        }


        public int UpdateMinutes => UpdateM();
        public int ResetMinutes => UpdateAboveH();
        public int UpdateSeconds => UpdateS();
        public int ResetSeconds => UpdateAboveM();

        public int UpdateM()
        {
            if (Minutes == 60)
            {
                Minutes = 0;
                return Minutes = 0;
            }
            else
                return Minutes;

        }

        public int UpdateS()
        {
            if (Seconds == 60)
            {
                return Seconds = 0;
            }
            else
                return Seconds;

        }

        public int UpdateAboveM()
        {
            if (Seconds == 60)
            {
                return Minutes + 1;
            }
            else
                return Minutes;
        }

        public int UpdateAboveH()
        {
            if (Minutes == 60)
            {
                return Hours + 1;
            }
            else
                return Hours;
        }
    }
}
