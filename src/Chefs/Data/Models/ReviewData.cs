namespace Chefs.Data;

public class ReviewData
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public string? UrlAuthorImage { get; set; }
    public Guid CreatedBy { get; set; }
    public string? PublisherName { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public List<string>? Likes { get; set; }
    public List<string>? Dislikes { get; set; }
    public bool UserLike { get; set; }
}
