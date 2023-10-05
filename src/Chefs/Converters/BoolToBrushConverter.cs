using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chefs.Converters
{
	public class BoolToBrushConverter : IValueConverter
	{
		public string? NullBrushKey { get; set; }

		public string? FalseBrushKey { get; set; }

		public string? TrueBrushKey { get; set; }

		public string? NullOrFalseBrushKey
		{
			get { return FalseBrushKey; }
			set { FalseBrushKey = NullBrushKey = value; }
		}

		public string? NullOrTrueBrushKey
		{
			get { return TrueBrushKey; }
			set { TrueBrushKey = NullBrushKey = value; }
		}

		public object? Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
			{
				return ResolveResource(NullBrushKey);
			}

			if (System.Convert.ToBoolean(value))
			{
				return ResolveResource(TrueBrushKey);
			}
			else
			{
				return ResolveResource(FalseBrushKey);
			}
		}

		public object? ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		private object? ResolveResource(string? name)
			=> Application.Current.Resources.TryGetValue(name, out object? value) ? value : null;
	}
}
