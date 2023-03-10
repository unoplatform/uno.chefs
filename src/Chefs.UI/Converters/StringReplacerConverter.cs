using Microsoft.UI.Xaml.Data;
namespace Chefs.Converters;

public class StringReplacerConverter : IValueConverter
{
   public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            string[] stringParams = ((string)parameter).Split(',');
            if(stringParams.Length == 1) return ((string)value).Replace(stringParams[0], "\n");
            return ((string)value).Replace(stringParams[0], stringParams[1]);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
