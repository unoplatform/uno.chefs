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
        Likes = reviewData.Likes;
        Dislikes = reviewData.Dislikes;
        Description = reviewData.Description;
        UrlAuthorImage = reviewData.UrlAuthorImage;
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
    public int Likes { get; init; }
    public int Dislikes { get; init; }

    internal ReviewData ToData() => new()
    {
        Id = Id,
        RecipeId = RecipeId,
        CreatedBy = CreatedBy, 
        Date = Date,
        Likes = Likes,
        Dislikes = Dislikes,
        Description = Description,
        UrlAuthorImage = UrlAuthorImage,
    };
}

