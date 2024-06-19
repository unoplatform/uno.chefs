using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;
public class BooleanToHeartFontIconStyleConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is bool isSaved)
		{
			return isSaved ? (Style)Application.Current.Resources["FontAwesomeSolidFontIconStyle"] : (Style)Application.Current.Resources["FontAwesomeRegularFontIconStyle"];
		}

		return (Style)Application.Current.Resources["FontAwesomeRegularFontIconStyle"];
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
