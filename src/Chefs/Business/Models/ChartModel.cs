using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore;

namespace Chefs.Business.Models
{
    public class ChartModel : ObservableObject
	{
		public IEnumerable<ISeries> Series { get; set; } =
			new[] { 2, 4, 1, 4, 3 }.AsPieSeries((value, series) =>
		{
			series.InnerRadius = 50;
		});
	}
}
