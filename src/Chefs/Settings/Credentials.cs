namespace Chefs.Settings;

public record Credentials
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}

