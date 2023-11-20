using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class CookingTimeFormatter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is Time time)
		{
			string timeString = (time.ToString() ?? Time.Under15min.ToString()).Replace("Under", "");

			return timeString.Insert(2, " ");
		}

		return null;
	}

	public object? ConvertBack(object value, Type targetType, object parameter, string language)
		=> throw new NotSupportedException("Only one-way conversion is supported.");
}
