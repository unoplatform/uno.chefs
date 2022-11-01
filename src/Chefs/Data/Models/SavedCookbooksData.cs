namespace Chefs.Data.Models;

public class SavedCookbooksData
{
    public Guid UserId { get; set; }
    public Guid[]? SavedCookbooks { get; set; }
}
