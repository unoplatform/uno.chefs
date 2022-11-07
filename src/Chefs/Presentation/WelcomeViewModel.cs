using System.Net;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class WelcomeViewModel
{
    private readonly INavigator _navigator;

    public WelcomeViewModel(INavigator navigator)
    {
        _navigator = navigator;
    }

    public async ValueTask GoToLogin(CancellationToken ct)
        => await _navigator.NavigateViewModelAsync<LoginViewModel>(this, Qualifiers.ClearBackStack);
}
