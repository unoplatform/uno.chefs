using System;
using Microsoft.UI.Xaml.Data;

namespace Chefs.Converters;
public class FromBoolToValueConverter : IValueConverter
{
	public object NullValue { get; set; }

	public object FalseValue { get; set; }

	public object TrueValue { get; set; }

	public object NullOrFalseValue
	{
		get => FalseValue;
		set => FalseValue = NullValue = value;
	}

	public object NullOrTrueValue
	{
		get => TrueValue;
		set => TrueValue = NullValue = value;
	}

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is null)
		{
			return NullValue;
		}

		if (System.Convert.ToBoolean(value))
		{
			return TrueValue;
		}
		else
		{
			return FalseValue;
		}
	}

	//Issue 642:[Windows] Binding not properly setting value to ToggleButton
	//https://github.com/unoplatform/uno.chefs/issues/642
	//We needed to use two-way binding to prevent the {Binding} from being cleared when modified by the control (ex: ToggleButton's IsChecked when pressed) and with that, it should no longer throw not-suppported for ConvertBack.

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		return value;
	}
}
