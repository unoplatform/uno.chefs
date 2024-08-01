namespace Chefs.Presentation;

public partial record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel
{
	private readonly IRecipeService _recipeService;

	private readonly IImmutableList<Step> _steps;

	public Recipe Recipe { get; }

	public IState<StepIterator> Steps => State.Value(this, () => new StepIterator(_steps));

	public IState<bool> Completed => State.Value(this, () => false);

	public LiveCookingModel(LiveCookingParameter parameter, IRecipeService recipeService)
	{
		Recipe = parameter.Recipe;
		_recipeService = recipeService;
		_steps = parameter.Steps;
	}

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
