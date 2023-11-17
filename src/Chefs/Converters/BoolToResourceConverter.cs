using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters
{
    class BoolToResourceConverter : IValueConverter
    {
        public string? TrueValue { get; set; } = null;
        public string? FalseValue { get; set; } = null;
        public string? NullValue { get; set; } = null;

        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool dValue
                ? dValue ? ResolveResource(TrueValue) : ResolveResource(FalseValue)
                : ResolveResource(NullValue);
        }

        private object? ResolveResource(string? name)
            => Application.Current.Resources.TryGetValue(name, out object? value) ? value : null;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException("Only one-way conversion is supported.");
    }
}
