using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class CertainNumberToVisible : IValueConverter
{
	public int Number { get; set; }

	public object? Convert(object value, Type targetType, object parameter, string language)
	{
		if (value == null) return Visibility.Collapsed;

		return Number == (int)value
				? Visibility.Visible
				: Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
}
