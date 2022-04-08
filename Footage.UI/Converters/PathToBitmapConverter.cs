namespace Footage.UI.Converters;

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

public class PathToBitmapConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        if (value is string rawPath && targetType.IsAssignableFrom(typeof(Bitmap)))
        {
            return new Bitmap(rawPath);
        }

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}