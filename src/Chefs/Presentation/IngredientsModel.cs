using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record IngredientsParameter(Recipe Recipe, IImmutableList<Ingredient> Ingredients, IImmutableList<Step> Steps);

public partial class IngredientsModel
{
    private readonly IUserService _userService;

    public IngredientsModel(IngredientsParameter parameter, IUserService userService)
    {
        _userService = userService;

        Ingredients = parameter.Ingredients;
        Steps = parameter.Steps;
        Recipe = parameter.Recipe;
    }

    public IEnumerable<Ingredient> Ingredients { get; }

    public IEnumerable<Step> Steps { get; }

    public Recipe Recipe { get; }

    public IFeed<User> User => Feed.Async(async ct => await _userService.GetById(Recipe.UserId, ct));
}
