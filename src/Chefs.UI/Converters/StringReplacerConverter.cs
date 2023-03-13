using Microsoft.UI.Xaml.Data;
namespace Chefs.Converters;

#nullable disable
public class StringReplacerConverter : IValueConverter
{
    public string OldValue { get; set; }
    public string NewValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is string s)
        {
            return s.Replace(OldValue, NewValue);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
#nullable enable