namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel
{
	private readonly IRecipeService _recipeService;

	public LiveCookingModel(LiveCookingParameter parameter, IRecipeService recipeService)
	{
		Steps = parameter.Steps;
		Recipe = parameter.Recipe;
		VideoSource = new Uri("ms-appx:///Assets/Videos/CookingVideo.mp4");

		_recipeService = recipeService;
	}

	public IImmutableList<Step> Steps { get; }

	public Uri VideoSource { get; set; }

	public IState<int> SelectedIndex => State.Value(this, () => 0);

	public IFeed<bool> CanFinish => SelectedIndex.Select(x => x == Steps.Count - 1);

	public IFeed<bool> CanGoNext => SelectedIndex.Select(x => (x + 1) < Steps.Count);

	public IFeed<bool> CanGoBack => SelectedIndex.Select(x => (x - 1) >= 0);

	public IState<bool> Completed => State.Value(this, () => false);

	public Recipe Recipe { get; }

	public async ValueTask Complete()
	{
<<<<<<< HEAD
		await Completed.SetAsync(true);
=======
		await Completed.Set(true, CancellationToken.None);
>>>>>>> a82d2f01e7817dc65a3b294471943d2122924d23
	}

	public async ValueTask BackToLastStep()
	{
<<<<<<< HEAD
		await Completed.SetAsync(false);
	}

	public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Save(recipe, ct);
=======
		await Completed.Set(false, CancellationToken.None);
	}

	public async ValueTask Save(Recipe recipe) =>
		await _recipeService.Save(recipe, CancellationToken.None);
>>>>>>> a82d2f01e7817dc65a3b294471943d2122924d23
}
