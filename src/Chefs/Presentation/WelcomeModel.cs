using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chefs.Presentation;

public partial class WelcomeModel(INavigator navigator) : INotifyPropertyChanged
{
	private readonly Iterable<int> _pages = new(Enumerable.Range(0, 3).ToList());
	
	public int CurrentIndex => _pages.CurrentIndex;
	
	public event PropertyChangedEventHandler? PropertyChanged;
	
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	
	public bool HasNext => _pages.HasNext;
	
	public async ValueTask Next()
	{
		if (HasNext)
		{
			_pages.Next();
			OnPropertyChanged(nameof(CurrentIndex));
		}
		else
		{
			await navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
	
	public void Previous()
	{
		if (!_pages.HasPrevious)
		{
			return;
		}
		
		_pages.Previous();
		OnPropertyChanged(nameof(CurrentIndex));
	}
}
