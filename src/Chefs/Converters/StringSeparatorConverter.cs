using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters
{
	public class StringSeparatorConverter : IValueConverter
	{
		public string? Separator { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is IEnumerable<string> list)
			{
				return string.Join(Separator, list.ToArray());
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
