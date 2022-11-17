namespace Chefs.Presentation;

public partial class UpdateCookbookViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    public UpdateCookbookViewModel(INavigator navigator,
                                   IRecipeService recipeService,
                                   ICookbookService cookbookService,
                                   Cookbook cookbook)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;

        Cookbook = State.Value(this, () => cookbook);
    }

    public IState<Cookbook> Cookbook { get; }

    public IListState<Recipe> Recipes => ListState.Async(this, async ct =>
    {
        var cookbook = await Cookbook;
        return (await _recipeService.GetSaved(ct))
        .RemoveAll(r1 => cookbook?.Recipes!.ToList()
        .Exists(r2 => r1.Id == r2.Id) ?? false);
    });

    public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackAsync(this, cancellation: ct);

    public async ValueTask Done(CancellationToken ct)
    {
        var selectedRecipes = (await Recipes).Where(x => x.Selected).ToImmutableList();
        var cookbook = await Cookbook;

        if (selectedRecipes is not null)
        {
            var response = await _cookbookService.Update(cookbook!, selectedRecipes, ct);
            await _navigator.NavigateBackWithResultAsync(this, data: response);
        }
        else
        {
            await _navigator
                .ShowMessageDialogAsync(this, content: "Please write a cookbook name and select one recipe", title: "Error");
        }
    }
}
