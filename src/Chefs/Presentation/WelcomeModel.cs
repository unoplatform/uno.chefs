using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chefs.Presentation;

public partial class WelcomeModel(INavigator navigator)
{
	private readonly Iterable<int> _pages = new(Enumerable.Range(0, 3).ToImmutableList());
	
	public int CurrentIndex => _pages.CurrentIndex;
	
	
	public bool HasNext => _pages.CanMoveNext;
	
	public async ValueTask Next()
	{
		if (HasNext)
		{
			_pages.MoveNext();
		}
		else
		{
			await navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
	
	public void Previous()
	{
		if (!_pages.CanMovePrevious)
		{
			return;
		}
		
		_pages.MovePrevious();
	}
}
