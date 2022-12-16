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

    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public string? UrlAuthorImage { get; set; }
    public Guid CreatedBy { get; set; }
    public string? PublisherName { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }

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

