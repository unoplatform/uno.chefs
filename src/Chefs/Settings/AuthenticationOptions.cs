namespace Chefs.Settings;

public record AuthenticationOptions
{
    public string? UserName { get; init; }
    public bool SaveCredentials { get; init; }
}
