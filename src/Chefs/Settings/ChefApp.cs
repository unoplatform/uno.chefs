namespace Chefs.Settings;

public record ChefApp
{
    public bool? IsDark { get; init; }
    public bool? Notification { get; init; }
    public string? AccentColor { get; init; }
}
