using Microsoft.UI.Xaml.Data;

#nullable disable
namespace Chefs.Converters
{
	public class BoolToObjectConverter : IValueConverter
	{
		public object TrueValue { get; set; }

		public object FalseValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language) => value != null ? ((bool)value ? TrueValue : FalseValue) : FalseValue;

		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
	}
}
#nullable enable
