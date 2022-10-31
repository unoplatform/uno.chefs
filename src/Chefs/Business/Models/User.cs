using System.Collections.Immutable;
using System.Net;
using System.Xml.Linq;
using Chefs.Data;
using Windows.System;

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

    public Guid Id { get; init; }
    public string? UrlProfileImage { get; init; }
    public string? FullName { get; init; }
    public string? Description { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public long? Followers { get; init; }
    public long? Following { get; init; }

    internal UserData ToData() => new()
    {
        Id = Id,
        UrlProfileImage = UrlProfileImage,
        FullName = FullName,
        Description = Description,
        Email = Email,
        PhoneNumber = PhoneNumber,
        Followers = Followers,
        Following = Following
    };
}
