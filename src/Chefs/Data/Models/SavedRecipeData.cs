namespace Chefs.Data.Models;

public class SavedRecipeData
{
    public SavedRecipeData(int userId, Guid recipe)
    {
        UserId = userId;
        SavedRecipes = new Guid[] { recipe };
    }

    public int UserId { get; set; }
    public Guid[] SavedRecipes { get; set; }
}
