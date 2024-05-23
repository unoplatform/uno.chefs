using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel(LiveCookingParameter parameter, IRecipeService recipeService)
	: INotifyPropertyChanged
{
	private readonly Iterable<Step> _steps = new(parameter.Steps.ToList());
	
	public event PropertyChangedEventHandler? PropertyChanged;
	
	public IImmutableList<Step> Steps => _steps.Items.ToImmutableList();
	public Uri VideoSource { get; set; } = new("ms-appx:///Assets/Videos/CookingVideo.mp4");
	
	public IState<int> SelectedIndex => State.Value(this, () => _steps.CurrentIndex);
	
	public IFeed<bool> CanFinish => SelectedIndex.Select(x => x == Steps.Count - 1);
	public IFeed<bool> CanGoNext => SelectedIndex.Select(x => x < Steps.Count - 1);
	public IFeed<bool> CanGoBack => SelectedIndex.Select(x => x > 0);
	
	public IState<bool> Completed => State.Value(this, () => false);
	
	public Recipe Recipe { get; } = parameter.Recipe;
	
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public async ValueTask Complete()
	{
		await Completed.SetAsync(true);
	}
	
	public void Previous()
	{
		if (_steps.HasPrevious)
		{
			_steps.Previous();
			OnPropertyChanged(nameof(SelectedIndex));
		}
	}
	
	public async ValueTask Next(CancellationToken ct)
	{
		if (_steps.HasNext)
		{
			_steps.Next();
			OnPropertyChanged(nameof(SelectedIndex));
		}
		else
		{
			await Complete(ct);
		}
	}
	
	public async ValueTask Save(Recipe recipe, CancellationToken ct)
	{
		await recipeService.Save(recipe, ct);
	}
}
