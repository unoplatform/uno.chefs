using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Chefs.Converters
{
    public class BoolToObjectConverter : IValueConverter
    {
        public object TrueValue { get; set; }

        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language) => (bool)value ? TrueValue : FalseValue;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
    }
}
#nullable enable
