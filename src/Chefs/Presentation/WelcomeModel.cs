using System.Net;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class WelcomeModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;

    // DR_REV: Do not clone the state value in your model. The state must stay enclose into the `State<int>` property.
    public IState<int> NextPage => State.Value(this, () => 0);

    public WelcomeModel(INavigator navigator)
    {
        _navigator = navigator;
    }

    public async ValueTask Next(int nextPage, CancellationToken ct) 
    {
        if (nextPage == 2) 
        {
			await GoToLogin(ct);
			await NextPage.Set(0, ct); // DR_REV: Is this really use-full ? Are we able to come back on this page?
        }
		else
		{
			await NextPage.Set(nextPage + 1, ct);
		}
	} 

    // DR_REV: XAML only nav
    public async ValueTask GoToLogin(CancellationToken ct)
        => await _navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
}
