using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business.Models;

public class CookbookImages
{
    public CookbookImages(ImmutableList<RecipeData> recipesData)
    {
        FirstImage = recipesData[0].ImageUrl;
        SecondImage = recipesData[1].ImageUrl;
        ThirdImage = recipesData[2].ImageUrl;
    }

    public string? FirstImage { get; set; }

    public string? SecondImage { get; set; }

    public string? ThirdImage { get; set; }
}
