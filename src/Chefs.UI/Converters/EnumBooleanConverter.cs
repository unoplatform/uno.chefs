using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chefs.Converters
{
    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return false;

            if (parameter is string parameterString)
            {
                if (parameterString == null)
                    return DependencyProperty.UnsetValue;

                if (Enum.IsDefined(value.GetType(), value) == false)
                    return DependencyProperty.UnsetValue;

                object parameterValue = Enum.Parse(value.GetType(), parameterString);

                return parameterValue.Equals(value);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string parameterString)
            {
                if (parameterString == null)
                    return DependencyProperty.UnsetValue;

                return Enum.Parse(targetType, parameterString);
            }

            return false;
        }
    }
}
