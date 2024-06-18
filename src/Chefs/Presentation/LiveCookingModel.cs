namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel
{
	public IState<StepIterable> Steps { get; }
	public Uri VideoSource { get; set; } = new("ms-appx:///Assets/Videos/CookingVideo.mp4");
	public IState<bool> Completed { get; }
	public Recipe Recipe { get; }
	
	private readonly IRecipeService _recipeService;
	
	public LiveCookingModel(LiveCookingParameter parameter, IRecipeService recipeService)
	{
		Recipe = parameter.Recipe;
		_recipeService = recipeService;
		
		Steps = State.Value(this, () => new StepIterable(parameter.Steps.ToImmutableList()));
		Completed = State.Value(this, () => false);
	}
	
	public async ValueTask Complete()
	{
		await Completed.SetAsync(true);
	}
	
	public ValueTask Previous()
		=> Steps.UpdateAsync(steps => steps!.MovePrevious() as StepIterable);
	
	public ValueTask Next()
		=> Steps.UpdateAsync(steps => steps!.MoveNext() as StepIterable);
	
	public async ValueTask BackToLastStep(CancellationToken ct)
	{
		await Completed.Set(false, ct);
	}
	public async ValueTask Save(Recipe recipe, CancellationToken ct)
	{
		await _recipeService.Save(recipe, ct);
	}
}
public record StepIterable(IImmutableList<Step> Items) : Iterator<Step>(Items);
