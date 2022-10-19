namespace Chefs.Settings;

public record ChefSettings
{
    public bool? IsDark { get; init; }
    public bool? Notification { get; init; }
    public string? AccentColor { get; init; }
}
