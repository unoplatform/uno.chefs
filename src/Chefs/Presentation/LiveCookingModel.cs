using System;

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
	
	public IState<Step> CurrentStep => State.Value(this, () => Steps.FirstOrDefault() ?? new Step(new StepData()));

	public IState<int> SelectedIndex => State.Value(this, () => 0);

	public IFeed<bool> CanFinish => SelectedIndex.Select(x => x == Steps.Count - 1);

	public IFeed<bool> CanGoNext => SelectedIndex.Select(x => (x + 1) < Steps.Count);

	public IFeed<bool> CanGoBack => SelectedIndex.Select(x => (x - 1) >= 0);

	public IState<bool> Completed => State.Value(this, () => false);

	public Recipe Recipe { get; }

	public async ValueTask ChangeStep(CancellationToken ct)
	{
		var index = await SelectedIndex + 1;
		await CurrentStep.UpdateAsync(s => s = Steps.FirstOrDefault(s => s.Number == index));
	}

	public async ValueTask Complete(CancellationToken ct)
	{
		await Completed.Set(true, ct);
	}

	public async ValueTask BackToLastStep(CancellationToken ct)
	{
		await Completed.Set(false, ct);
	}

	public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Save(recipe, ct);
}
