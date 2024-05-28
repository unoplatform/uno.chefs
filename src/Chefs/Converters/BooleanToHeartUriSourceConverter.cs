using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

class BooleanToHeartUriSourceConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is bool isSaved)
		{
			return isSaved
				? "ms-appx:///Assets/Icons/full_heart.png"
				: "ms-appx:///Assets/Icons/favorite_heart.png";
		}

		return "ms-appx:///Assets/Icons/favorite_heart.png";
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
