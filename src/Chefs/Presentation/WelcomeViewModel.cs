using System.Net;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class WelcomeViewModel
{
    private readonly INavigator _navigator;
    private int pageIndex = 0;

    public IState<int> NextPage => State.Value(this, () => pageIndex);

    public WelcomeViewModel(INavigator navigator)
    {
        _navigator = navigator;
    }

    public async ValueTask Next(CancellationToken ct) 
    {
        if (pageIndex == 2) 
        {
            pageIndex = 0;
            await GoToLogin(ct);
        }
        await NextPage.Update(_ => ++pageIndex, ct);
    } 

    public async ValueTask GoToLogin(CancellationToken ct)
        => await _navigator.NavigateViewModelAsync<LoginViewModel>(this, Qualifiers.ClearBackStack);
}
