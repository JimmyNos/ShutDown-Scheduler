using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShutDown_Scheduler.MVVM.View
{
    /// <summary>
    /// Interaction logic for Countdown.xaml
    /// </summary>
    public partial class Countdown : UserControl
    {
        public int CZIndex
        {
            get => (int)GetValue(CZIndexProperty);
            set => SetValue(CZIndexProperty, value);
        }
        public static readonly DependencyProperty CZIndexProperty =
            DependencyProperty.Register(
                nameof(CZIndex),
                typeof(int),
                typeof(Countdown),
                new PropertyMetadata(0, OnCountHoursChanged));

        public ICommand CountHoursUp
        {
            get => (ICommand)GetValue(CountHoursUpProperty);
            set => SetValue(CountHoursUpProperty, value);
        }

        public static readonly DependencyProperty CountHoursUpProperty =
            DependencyProperty.Register(
                nameof(CountHoursUp),
                typeof(ICommand),
                typeof(Countdown),
                new PropertyMetadata(null)
            );

        public ICommand CountHoursDw
        {
            get => (ICommand)GetValue(CountHoursDwProperty);
            set => SetValue(CountHoursDwProperty, value);
        }
        public static readonly DependencyProperty CountHoursDwProperty =
            DependencyProperty.Register(
                nameof(CountHoursDw),
                typeof(ICommand),
                typeof(Countdown),
                new PropertyMetadata(null));

        public ICommand CountMinutesUp
        {
            get => (ICommand)GetValue(CountMinutesUpProperty);
            set => SetValue(CountMinutesUpProperty, value);
        }
        public static readonly DependencyProperty CountMinutesUpProperty =
            DependencyProperty.Register(
                nameof(CountMinutesUp),
                typeof(ICommand),
                typeof(Countdown),
                new PropertyMetadata(null));

        public ICommand CountMinutesDw
        {
            get => (ICommand)GetValue(CountMinutesDwProperty);
            set => SetValue(CountMinutesDwProperty, value);
        }
        public static readonly DependencyProperty CountMinutesDwProperty =
            DependencyProperty.Register(
                nameof(CountMinutesDw),
                typeof(ICommand),
                typeof(Countdown),
                new PropertyMetadata(null));

        public ICommand CountSecondsUp
        {
            get => (ICommand)GetValue(CountSecondsUpProperty);
            set => SetValue(CountSecondsUpProperty, value);
        }
        public static readonly DependencyProperty CountSecondsUpProperty =
            DependencyProperty.Register(
                nameof(CountSecondsUp),
                typeof(ICommand),
                typeof(Countdown),
                new PropertyMetadata(null));

        public ICommand CountSecondsDw
        {
            get => (ICommand)GetValue(CountSecondsDwProperty);
            set => SetValue(CountSecondsDwProperty, value);
        }
        public static readonly DependencyProperty CountSecondsDwProperty =
            DependencyProperty.Register(
                nameof(CountSecondsDw),
                typeof(ICommand),
                typeof(Countdown),
                new PropertyMetadata(null));

        public int CountIsUpDownEnable
        {
            get => (int)GetValue(CountIsUpDownEnableProperty);
            set => SetValue(CountIsUpDownEnableProperty, value);
        }
        public static readonly DependencyProperty CountIsUpDownEnableProperty =
            DependencyProperty.Register(
                nameof(CountIsUpDownEnable),
                typeof(bool),
                typeof(Countdown),
                new PropertyMetadata(true));

        public int CountHours
        {
            get => (int)GetValue(CountHoursProperty);
            set => SetValue(CountHoursProperty, value);
        }
        public static readonly DependencyProperty CountHoursProperty =
            DependencyProperty.Register(
                nameof(CountHours),
                typeof(int),
                typeof(Countdown),
                new PropertyMetadata(0, OnCountHoursChanged));

        public int CountMinutes
        {
            get => (int)GetValue(CountMinutesProperty);
            set => SetValue(CountMinutesProperty, value);
        }
        public static readonly DependencyProperty CountMinutesProperty =
            DependencyProperty.Register(
                nameof(CountMinutes),
                typeof(int),
                typeof(Countdown),
                new PropertyMetadata(0, OnCountHoursChanged));

        public int CountSeconds
        {
            get => (int)GetValue(CountSecondsProperty);
            set => SetValue(CountSecondsProperty, value);
        }
        public static readonly DependencyProperty CountSecondsProperty =
            DependencyProperty.Register(
                nameof(CountSeconds),
                typeof(int),
                typeof(Countdown),
                new PropertyMetadata(0, OnCountHoursChanged));

        private static void OnCountHoursChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Countdown countdown)
            {
                // Trigger Update whenever CountHours changes
                countdown.Update();
                // Raise PropertyChanged for UpdateLabel if you're binding to it
                //countdown.OnPropertyChanged(nameof(UpdateLabel));
            }
        }

        private void SecondsUp()
        {
            if (CountSeconds >= 0 && CountSeconds <= 60)
            {
                CountSeconds++;
            }

        }

        public string UpdateLabel => Update();

        private string Update()
        {
            if (CountSeconds == 60)
            {
                CountSeconds = 0;
                CountMinutes++;
                if (CountMinutes == 60)
                {
                    CountMinutes = 0;
                    if (CountHours <= 99)
                    {
                        CountHours++;
                    }
                }
            }
            else if (CountSeconds < 0)
                CountSeconds = 59;

            if (CountMinutes == 60)
            {
                CountMinutes = 0;
                if (CountHours <= 99) CountHours++;
            }
            else if (CountMinutes < 0)
                CountMinutes = 59;

            if (CountHours == 100)
            {
                CountHours = 0;
            }
            else if (CountHours == -1)
                CountHours = 99;

            return "Hold";
        }

        public Countdown()
        {
            InitializeComponent();
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
