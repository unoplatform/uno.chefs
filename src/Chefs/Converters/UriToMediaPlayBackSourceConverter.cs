using Microsoft.UI.Xaml.Data;
using Windows.Media.Core;

namespace Chefs.Converters;

public class StringToMediaPlayBackSourceConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is string uriString && !uriString.IsNullOrEmpty())
		{
			return MediaSource.CreateFromUri(new Uri(uriString));
		}
		
		return MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Videos/CookingVideo.mp4"));
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
