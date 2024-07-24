using Chefs.Presentation.Extensions;
using Windows.UI.Popups;

namespace Chefs.Presentation;

public partial class CreateUpdateCookbookModel
{
	const uint DefaultPageSize = 20;

	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly ICookbookService _cookbookService;
	private readonly IMessenger _messenger;
	private readonly Cookbook? _cookbook;

	public CreateUpdateCookbookModel(
		INavigator navigator,
		IRecipeService recipeService,
		ICookbookService cookbookService,
		Cookbook? cookbook,
		IMessenger messenger)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_cookbookService = cookbookService;
		_messenger = messenger;

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

		_messenger.Observe(Cookbook, cb => cb.Id);
	}
	public bool IsCreate { get; }

	public string Title { get; }

	public string SubTitle { get; }

	public string SaveButtonContent { get; }

	public IState<Cookbook> Cookbook => State.Value(this, () => _cookbook ?? new Cookbook());

	public IListFeed<Recipe> Recipes => ListFeed
		.PaginatedAsync(
			async (PageRequest pageRequest, CancellationToken ct) =>
				await _recipeService.GetFavoritedWithPagination(pageRequest.DesiredSize ?? DefaultPageSize, pageRequest.CurrentCount, ct)
		).Selection(SelectedRecipes);

	public IState<IImmutableList<Recipe>> SelectedRecipes => State.FromFeed(this, Cookbook.Select(c => c.Recipes));

	public async ValueTask Submit(CancellationToken ct)
	{
		var selectedRecipes = await SelectedRecipes;
		var cookbook = await Cookbook;

		if (selectedRecipes is { Count: > 0 } && cookbook is not null && cookbook.Name.HasValueTrimmed())
		{
			var response = IsCreate
				? await _cookbookService.Create(cookbook.Name!, selectedRecipes.ToImmutableList(), ct)
				: await _cookbookService.Update(cookbook, selectedRecipes, ct);

			if (IsCreate)
			{
				await _cookbookService.Save(response!, ct);
			}

			await _navigator.NavigateBackWithResultAsync(this, data: response);
		}
		else
		{
			await _navigator.ShowDialog(this, new DialogInfo("Error", "Please write a cookbook name and select one recipe."), ct);
		}
	}
}
