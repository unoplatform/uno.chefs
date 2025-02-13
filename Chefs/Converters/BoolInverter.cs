using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class BoolInverter : IValueConverter
{

	public object? Convert(object value, Type targetType, object parameter, string language) => GetConversion(value);

	public object ConvertBack(object value, Type targetType, object parameter, string language) => GetConversion(value);

	private bool GetConversion(object value)
	{
		if (value is bool)
		{
			return !(bool)value;
		}

		return true;
	}
}
