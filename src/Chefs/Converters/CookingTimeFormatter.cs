using Microsoft.UI.Xaml.Data;
using System.Globalization;

namespace Chefs.Converters;

public class CookingTimeFormatter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value.GetType() == typeof(Time)) {
            string timeString = (value.ToString() ?? Time.Under15min.ToString()).Replace("Under", "");

            return timeString.Insert(2, " ");
        }
        else
        {
            return null;
        }
    }

    public object? ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException("Only one-way conversion is supported.");
}
