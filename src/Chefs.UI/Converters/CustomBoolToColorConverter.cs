using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class CustomBoolToColorConverter : IValueConverter
{
    public object? TrueColorValue { get; set; }

    public object? FalseColorValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language) => GetConversion(value);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => GetConversion(value);

    private object GetConversion(object value)
    {
        if (value is bool)
        {
            return (bool)value ? TrueColorValue! : FalseColorValue!;
        }

        return FalseColorValue!;
    }
}