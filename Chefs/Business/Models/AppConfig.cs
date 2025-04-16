using System.Text.Json.Serialization;

namespace Chefs.Business.Models;

public record AppConfig
{
	public string? Title { get; init; }
	public bool? IsDark { get; init; }
	public bool? Notification { get; init; }
	public string? AccentColor { get; init; }
}

[JsonSerializable(typeof(AppConfig))]
[JsonSerializable(typeof(Dictionary<string, AppConfig>))]
public partial class AppConfigContext : JsonSerializerContext
{
}