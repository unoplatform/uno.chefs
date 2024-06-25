namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel
{
	private readonly IRecipeService _recipeService;

	public LiveCookingModel(LiveCookingParameter parameter, IRecipeService recipeService)
	{
		Steps = parameter.Steps;
		Recipe = parameter.Recipe;

		_recipeService = recipeService;
	}

	public IImmutableList<Step> Steps { get; }

	public IState<int> SelectedIndex => State.Value(this, () => 0);

	public IFeed<Step> CurrentStep => SelectedIndex.Select(x => (x >= 0 && x < Steps.Count) ? Steps[x] : Steps[0]);

	public IFeed<bool> CanFinish => SelectedIndex.Select(x => x == Steps.Count - 1);

	public IFeed<bool> CanGoNext => SelectedIndex.Select(x => (x + 1) < Steps.Count);

	public IFeed<bool> CanGoBack => SelectedIndex.Select(x => (x - 1) >= 0);

	public IState<bool> Completed => State.Value(this, () => false);

	public Recipe Recipe { get; }

	public async ValueTask Complete()
	{
		await Completed.SetAsync(true);
	}

	public async ValueTask BackToLastStep()
	{
		await Completed.SetAsync(false);
	}

	public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Favorite(recipe, ct);
}
