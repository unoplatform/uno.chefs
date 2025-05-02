using System.Text.Json.Serialization;
using Chefs.Api.Converters;

namespace Chefs.Api.Data;

public class StepData
{
	public string? UrlVideo { get; set; }
	public string? Name { get; set; }
	public int Number { get; set; }
	[JsonConverter(typeof(TimeSpanObjectConverter))]
	public TimeSpan CookTime { get; set; }
	public List<string>? Cookware { get; set; }
	public List<string>? Ingredients { get; set; }
	public string? Description { get; set; }
}
