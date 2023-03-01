using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public partial record Review
{
    public Review(ReviewData reviewData)
    {
        Id = reviewData.Id;
        RecipeId = reviewData.RecipeId;
        CreatedBy = reviewData.CreatedBy; 
        PublisherName = reviewData.PublisherName;
        Date = reviewData.Date;
        Likes = reviewData.Likes!.Select(r => new User(r))
            .ToImmutableList() 
            ?? ImmutableList<User>.Empty;
        Dislikes = reviewData.Dislikes!.Select(r => new User(r))
            .ToImmutableList()
            ?? ImmutableList<User>.Empty;
        Description = reviewData.Description;
        UrlAuthorImage = reviewData.UrlAuthorImage;
        UserLike = reviewData.UserLike;
    }

    public Review(Guid recipeId, string text)
    {
        Id = Guid.NewGuid();
        RecipeId = recipeId;
        Description = text;
    }

    public Guid Id { get; init; }
    public Guid RecipeId { get; init; }
    public string? UrlAuthorImage { get; init; }
    public Guid CreatedBy { get; init; }
    public string? PublisherName { get; init; }
    public DateTime Date { get; init; }
    public string? Description { get; init; }
    public ImmutableList<User>? Likes { get; init; }
    public ImmutableList<User>? Dislikes { get; init; }
    public bool UserLike { get; init; }

    internal ReviewData ToData() => new()
    {
        Id = Id,
        RecipeId = RecipeId,
        CreatedBy = CreatedBy, 
        PublisherName = PublisherName,
        Date = Date,
        Likes = Likes?.Select(l => l.ToData())
            .ToList(),
        Dislikes = Dislikes?.Select(d => d.ToData())
            .ToList(),
        Description = Description,
        UrlAuthorImage = UrlAuthorImage,
        UserLike = UserLike,
    };
}

