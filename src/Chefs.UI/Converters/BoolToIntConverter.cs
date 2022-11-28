using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chefs.Converters
{
    public class BoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => value is null ? false : value.Equals(parameter);

        public object ConvertBack(object value, Type targetType, object parameter, string language) 
        {
            if (value is bool boolean && parameter is string str)
            {
                return boolean ? int.Parse(str) : 0;
            }

            return 0;
        }
    }
}
