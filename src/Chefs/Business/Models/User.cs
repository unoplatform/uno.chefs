using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public record User
{
    internal User(UserData user)
    {
        Id = user.Id;
        UrlProfileImage = user.UrlProfileImage;
        FullName = user.FullName;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
    }

    public Guid? Id { get; init; }
    public string? UrlProfileImage { get; init; }
    public string? FullName { get; init; }
    public string? Description { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public long? Followers { get; init; }
    public long? Following { get; init; }
}
