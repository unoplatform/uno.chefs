namespace Chefs.Presentation;

public record IngredientsParameter(Recipe Recipe, IImmutableList<Ingredient> Ingredients);

public partial class IngredientsViewModel
{
    private INavigator _navigator;
    private IUserService _userService;

    public IngredientsViewModel(IngredientsParameter parameter, INavigator navigator, IUserService userService)
    {
        _navigator = navigator;
        _userService = userService;

        Ingredients = ListState.Value(this, () => parameter.Ingredients);
        Recipe = State.Value(this, () => parameter.Recipe);
    }

    public IListState<Ingredient> Ingredients { get; }

    public IState<Recipe> Recipe { get; }

    public IFeed<User> User => Feed.Async(async ct => await _userService.GetById((await Recipe)!.UserId, ct));

    private async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
