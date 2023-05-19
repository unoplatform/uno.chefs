using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class FromNullToVisibilityConverter : IValueConverter
{
	public bool Invert { get; set; }

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (Invert)
		{
			return value == null ? Visibility.Visible : Visibility.Collapsed;
		}
		return value == null ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}

