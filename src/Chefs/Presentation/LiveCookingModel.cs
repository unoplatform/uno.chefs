namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel(LiveCookingParameter parameter, IRecipeService recipeService)

{
	private readonly Iterable<Step> _steps = new(parameter.Steps.ToImmutableList());
	public IState<Iterable<Step>> Steps => State<Iterable<Step>>.Value(this, () => _steps);
	public Uri VideoSource { get; set; } = new("ms-appx:///Assets/Videos/CookingVideo.mp4");
	
	// public int SelectedIndex => Steps.CurrentIndex;
	// public bool CanFinish => Steps.CurrentIsLast;
	// public bool CanGoNext => Steps.CanMoveNext;
	// public bool CanGoBack => Steps.CanMovePrevious;
	
	public IState<bool> Completed => State.Value(this, () => false);
	
	public Recipe Recipe { get; } = parameter.Recipe;


	public async ValueTask Complete()
	{
		await Completed.SetAsync(true);
	}
	
	public ValueTask Previous()
		=> Steps.UpdateAsync(steps => steps!.MovePrevious());
	
	public ValueTask Next()
		=> Steps.UpdateAsync(steps => steps!.MoveNext());
	
	public async ValueTask Save(Recipe recipe, CancellationToken ct)
	{
		await recipeService.Save(recipe, ct);
	}
}
