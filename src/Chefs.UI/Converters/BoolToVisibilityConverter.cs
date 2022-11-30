using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chefs.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return Visibility.Collapsed;

            if (value.GetType() == typeof(Visibility)) return (Visibility)value == Visibility.Visible 
                    ? Visibility.Collapsed 
                    : Visibility.Visible;

            return Invert ?
            (bool)value ? Visibility.Collapsed : Visibility.Visible :
            (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
    }
}
