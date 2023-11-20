namespace Chefs.Presentation;

public partial class CreateUpdateCookbookModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly ICookbookService _cookbookService;
	private readonly Cookbook? _cookbook;

	public CreateUpdateCookbookModel(
		INavigator navigator,
		IRecipeService recipeService,
		ICookbookService cookbookService,
		Cookbook? cookbook)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_cookbookService = cookbookService;
		if (cookbook is not null)
		{
			_cookbook = cookbook;
			Title = "Update cookbook";
			SubTitle = "Manage cookbook's recipes";
			SaveButtonContent = "Apply change";
			IsCreate = false;
		}
		else
		{
			Title = "Create cookbook";
			SubTitle = "Add recipes";
			SaveButtonContent = "Create cookbook";
			IsCreate = true;
		}
	}
	public bool IsCreate { get; }

	public string Title { get; }

	public string SubTitle { get; }

	public string SaveButtonContent { get; }

	public IState<Cookbook> Cookbook => State.Value(this, () => _cookbook ?? new Cookbook());

	public IListState<Recipe> Recipes => ListState.Async(this, async ct =>
	{
		var cookbook = await Cookbook;

		var recipes = await _recipeService.GetSaved(ct);
		var recipesExceptCookbook = cookbook?.Recipes is null
			? recipes
			: recipes.Select(r =>
			{
				if (cookbook.Recipes.Any(cbR => r.Id == cbR.Id)) r = r with { Selected = true };
				return r;
			})
			.ToImmutableList();

		return recipesExceptCookbook;
	});

	public async ValueTask Submit(CancellationToken ct)
	{
		var selectedRecipes = (await Recipes).Where(x => x.Selected).ToImmutableList();
		var cookbook = await Cookbook;

		if (selectedRecipes is not null && cookbook is not null && selectedRecipes.Count > 0)
		{

			var response = IsCreate ? await _cookbookService.Create(cookbook.Name!, selectedRecipes.ToImmutableList(), ct)
				: await _cookbookService.Update(cookbook!, selectedRecipes, ct);

			await _navigator.NavigateBackWithResultAsync(this, data: response);
		}
		else
		{
			await _navigator
				.ShowMessageDialogAsync(this, content: "Please write a cookbook name and select one recipe", title: "Error");
		}
	}
}
