using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public record Recipe
{
    internal Recipe(RecipeData recipeData)
    {
        Id = recipeData.Id;
        UserId = recipeData.UserId;
        ImageUrl = recipeData.ImageUrl;
        Name = recipeData.Name;
        Serves = recipeData.Serves;
        CookTime = recipeData.CookTime;
        Difficulty = recipeData.Difficulty;
        Ingredients = recipeData.Ingredients?
            .Select(i => new Ingredient(i))
            .ToImmutableList();
        Calories = recipeData.Calories;
        Details = recipeData.Details;
        Category = new Category(recipeData.Category);
        Date = recipeData.Date;
    }
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? ImageUrl { get; init; }
    public string? Name { get; init; }
    public int Serves { get; init; }
    public TimeSpan CookTime { get; init; }
    public Difficulties Difficulty { get; init; }
    public IImmutableList<Ingredient>? Ingredients { get; init; }
    public string? Calories { get; init; }
    public string? Details { get; init; }
    public Category Category { get; init; }
    public DateTime Date { get; init; }
    public bool Save { get; init; }
    public bool Selected { get; init; }
    public string TimeCal => CookTime > TimeSpan.FromHours(1) ?
        String.Format("{0:%h} hour {0:%m} mins • {1}", CookTime, Calories) :
        String.Format("{0:%m} mins • {1}", CookTime, Calories);

    internal RecipeData ToData() => new()
    {
        Id = Id,
        UserId = UserId,
        ImageUrl = ImageUrl,
        Name = Name,
        Serves = Serves,
        CookTime = CookTime,
        Difficulty = Difficulty,
        Ingredients = Ingredients?
            .Select(i => i.ToData())
            .ToImmutableList(),
        Calories = Calories,
        Details = Details,
        Category = Category.ToData(),
        Date = Date
    };
}
