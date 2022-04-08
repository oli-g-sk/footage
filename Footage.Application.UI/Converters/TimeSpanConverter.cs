using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Footage.Application.Presentation;

namespace Footage.Application.UI.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public TimeSpanHoursDisplayMode HoursDisplayMode { get; set; } = TimeSpanHoursDisplayMode.Always;
        
        public bool ShowMilliseconds { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not TimeSpan timeSpan)
            {
                throw new ArgumentException(
                    $"TimeSpanConverter can only convert objects of type TimeSpan. Value provided: {value.GetType()}",
                    nameof(value));
            }

            string formatted = HoursDisplayMode switch
            {
                TimeSpanHoursDisplayMode.Always => $"{Pad(timeSpan.Hours)}:{Pad(timeSpan.Minutes)}:{Pad(timeSpan.Seconds)}",
                TimeSpanHoursDisplayMode.Never => $"{Pad(GetTotalMinutes(timeSpan))}:{Pad(timeSpan.Seconds)}",
                TimeSpanHoursDisplayMode.WhenNonZero => timeSpan.Hours > 0
                    ? $"{Pad(timeSpan.Hours)}:{Pad(timeSpan.Minutes)}:{Pad(timeSpan.Seconds)}"
                    : $"{Pad(timeSpan.Minutes)}:{Pad(timeSpan.Seconds)}",
                _ => string.Empty
            };

            if (ShowMilliseconds)
            {
                formatted += $".{timeSpan.Milliseconds.ToString().PadLeft(3, '0')}";
            }

            return formatted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static int GetTotalMinutes(TimeSpan timeSpan)
        {
            return timeSpan.Hours * 60 + timeSpan.Minutes;
        }

        private static string Pad(int value) => value.ToString().PadLeft(2, '0');
    }
}