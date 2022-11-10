using Chefs.Data;

namespace Chefs.Business;

public record Review
{
    public Review(ReviewData reviewData)
    {
        RecipeId = reviewData.RecipeId;
        CreatedBy = reviewData.CreatedBy; 
        PublisherName = reviewData.PublisherName;
        Date = reviewData.Date;
        Likes = reviewData.Likes;
        Dislikes = reviewData.Dislikes;
        Description = reviewData.Description;
    }

    public Review(Guid recipeId, string text)
    {
        RecipeId = recipeId;
        Description = text;
    }

    public Guid RecipeId { get; set; }
    public Guid CreatedBy { get; set; }
    public string? PublisherName { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }

    internal ReviewData ToData() => new()
    {
        RecipeId = RecipeId,
        CreatedBy = CreatedBy, 
        Date = Date,
        Likes = Likes,
        Dislikes = Dislikes,
        Description = Description
    };
}

