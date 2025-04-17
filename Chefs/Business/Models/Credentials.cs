namespace Chefs.Business.Models;

public record Credentials
{
	public string? Username { get; init; }
	public string? Password { get; init; }
	public bool SkipWelcome { get; init; }
	public bool SaveCredentials { get; init; }
}

