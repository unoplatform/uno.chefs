using System.Collections.Immutable;

namespace Chefs.Data;

public record UserData
{
    public string? UrlProfileImage { get; init; }
    public string? FullName { get; init; }
    public string? Description { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Password { get; init; }
    public long? Followers { get; init; }
    public long? Following { get; init; }
    public IImmutableList<RecipeData>? Recipes { get; init; }
    public IImmutableList<CookbookData>? Cookbooks { get; init; }
}
