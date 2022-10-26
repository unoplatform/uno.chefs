namespace Chefs.Data.Models;

public class SavedItemsData
{
    public Guid UserId { get; set; }
    public Guid[]? SavedCookbooks { get; set; }
    public Guid[]? SavedRecipes { get; set; }
}
