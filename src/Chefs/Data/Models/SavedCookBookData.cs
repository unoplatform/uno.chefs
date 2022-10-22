namespace Chefs.Data.Models;

public class SavedItemsData
{
    public int UserId { get; set; }
    public Guid[]? SavedCookbooks { get; set; }
    public Guid[]? SavedRecipes { get; set; }
}
