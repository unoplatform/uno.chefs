using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public record User
{
    internal User(UserData user)
    {
        UrlProfileImage = user.UrlProfileImage;
        FullName = user.FullName;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        Recipes = user.Recipes?
            .Select(r => new Recipe(r))
            .ToImmutableList();
        Cookbooks = user.Cookbooks?
            .Select(c => new Cookbook(c))
            .ToImmutableList();
    }

    public string? UrlProfileImage { get; init; }
    public string? FullName { get; init; }
    public string? Description { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public int Followers { get; init; }
    public int Following { get; init; }
    public IImmutableList<Recipe>? Recipes { get; init; }
    public IImmutableList<Cookbook>? Cookbooks { get; init; }
}
