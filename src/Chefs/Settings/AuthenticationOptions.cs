namespace Chefs.Settings;

public record AuthenticationOptions
{
    public string? Email { get; init; }
    public bool SaveCredentials { get; init; }
}
