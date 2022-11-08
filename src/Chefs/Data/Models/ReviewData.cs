namespace Chefs.Data;

public class ReviewData
{
    public Guid RecipeId { get; set; }
    public Guid CreatedBy { get; set; }
    public string? PublisherName { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}
