using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class CustomBackgroundColorConverter : IValueConverter
{

    public object? Convert(object value, Type targetType, object parameter, string language) => GetConversion(value);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => GetConversion(value);

    private string GetConversion(object value)
    {
        if (value is bool)
        {
            return (bool)value ? "#FFFFFF" : "#ED3F64";
        }

        return "#ED3F64";
    }
}