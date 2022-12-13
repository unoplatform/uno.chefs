using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record IngredientsParameter(Recipe Recipe, IImmutableList<Ingredient> Ingredients);

public partial class IngredientsViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    public IngredientsViewModel(IngredientsParameter parameter, INavigator navigator, IUserService userService)
    {
        _navigator = navigator;
        _userService = userService;

        Ingredients = ListState.Value(this, () => parameter.Ingredients);
        Recipe = State.Value(this, () => parameter.Recipe);
    }

	// DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a ListState, use a simple property instead:
	//          public IEnumerable<Ingredient> Ingredients { get; }
	public IListState<Ingredient> Ingredients { get; }

	// DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a State, use a simple property instead:
	//          public Recipe Recipe { get; }
    public IState<Recipe> Recipe { get; }

    public IFeed<User> User => Feed.Async(async ct => await _userService.GetById((await Recipe)!.UserId, ct));

    // DR_REV: XAML only nav
    private async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
