using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chefs.Converters
{
    public class InflateDimensionConverter : IValueConverter
    {
        public double Inflation { get; set; }
        public double DefaultValue { get; set; } = 0d;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (double.TryParse(value.ToString(), out var dimension))
            {
                var result = dimension + Inflation;
                if (result >= 0)
                {
                    return result;
                }
            }

            return DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}