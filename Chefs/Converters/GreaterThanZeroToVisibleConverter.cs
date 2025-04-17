using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class GreaterThanZeroToVisibleConverter : IValueConverter
{

	public object? Convert(object value, Type targetType, object parameter, string language) => GetConversion(value);

	public object ConvertBack(object value, Type targetType, object parameter, string language) => GetConversion(value);

	private object GetConversion(object value)
	{
		if (value is int) return (int)value > 0
										? Visibility.Visible
										: Visibility.Collapsed;
		return Visibility.Collapsed;
	}
}
