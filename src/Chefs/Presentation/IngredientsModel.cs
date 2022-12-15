using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record IngredientsParameter(Recipe Recipe, IImmutableList<Ingredient> Ingredients, IImmutableList<Step> Steps);

public partial class IngredientsModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    public IngredientsModel(IngredientsParameter parameter, INavigator navigator, IUserService userService)
    {
        _navigator = navigator;
        _userService = userService;

        Ingredients = parameter.Ingredients;
        Steps = parameter.Steps;
        Recipe = parameter.Recipe;
    }

    public IEnumerable<Ingredient> Ingredients { get; }

    public IEnumerable<Step> Steps { get; }

    public Recipe Recipe { get; }

    public IFeed<User> User => Feed.Async(async ct => await _userService.GetById(Recipe.UserId, ct));

    // DR_REV: XAML only nav
    private async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
