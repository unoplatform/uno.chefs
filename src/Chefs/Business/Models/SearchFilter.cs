using Chefs.Data;

namespace Chefs.Business;

public record SearchFilter(
    OrganizeCategories? OrganizeCategories, 
    Times? Time, 
    Difficulties? Difficulty, 
    Category? Category)
{
    public bool Match(Recipe recipe)
    {
        TimeSpan time = TimeSpan.Zero;

        if (Time is not null)
        {
            switch (Time)
            {
                case Times.Under15min:
                    time = new TimeSpan(0, 15, 00);
                    break;
                case Times.Under30min:
                    time=  new TimeSpan(0, 30, 00);
                    break;
                case Times.Under60min:
                    time = new TimeSpan(0, 60, 00);
                    break;
            }
        }

        if ((Difficulty == null || recipe.Difficulty == Difficulty) &&
            (Time == null || recipe.CookTime < time) &&
            (Category == null || recipe.Category.Id == Category.Id))
        {
            return true;
        }

        return false;
    }
}
