namespace Chefs.Business.Models;

public record AppConfig
{
	public string? Title { get; init; }
	public bool? IsDark { get; init; }
	public bool? Notification { get; init; }
	public string? AccentColor { get; init; }
}
