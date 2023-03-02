using System.Collections.Immutable;
using System.Linq;
using Chefs.Business;
using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;

public class UserLikeColorConverter : IValueConverter
{
    public object? PressValue { get; set; }

    public object? UnpressValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is IImmutableList<Guid> && parameter is User)
        {
            var user = (User)parameter;
            var ids = (IImmutableList<Guid>)value;
            return ids.Where(id => id == user.Id).Count() > 0 
                ? PressValue! 
                : UnpressValue!;
        }
        return UnpressValue!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    
}
