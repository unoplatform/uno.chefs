using System.Collections.Immutable;
using System.Linq;
using Chefs.Business;
using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class UserLikeColorConverter : IValueConverter
{
    public object? PressValue { get; set; }

    public object? UnpressValue { get; set; }

    public object? DefaultValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return DefaultValue!;

        var userLike = (bool)value;

        return userLike ? PressValue! : UnpressValue!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
