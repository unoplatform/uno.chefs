using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is TimeSpan ts)
        {
            var ftm = string.Format("{0} {1}",
                ts.Hours > 0 ? ts.ToString(@"%h' hour.'") : string.Empty,
                ts.Minutes > 0 ? ts.ToString(@"%m' mins'") : string.Empty);

            return ftm;

        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
