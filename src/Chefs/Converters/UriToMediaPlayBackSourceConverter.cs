using Microsoft.UI.Xaml.Data;
using Windows.Media.Core;

namespace Chefs.Converters;

public class UriToMediaPlayBackSourceConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		return MediaSource.CreateFromUri((Uri)value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
