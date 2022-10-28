using Chefs.Data;

namespace Chefs.Business;

public record Review
{
    public Review(ReviewData reviewData)
    {
        RecipeId = reviewData.RecipeId;
        PublisherName = reviewData.PublisherName; 
        Date = reviewData.Date;
        Likes = reviewData.Likes;
        Dislikes = reviewData.Dislikes;
        Description = reviewData.Description;
    }

    public Guid RecipeId { get; set; }
    public string? PublisherName { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }

    internal ReviewData ToData() => new()
    {
        RecipeId = RecipeId,
        PublisherName = PublisherName, 
        Date = Date,
        Likes = Likes,
        Dislikes = Dislikes,
        Description = Description
    };
}

