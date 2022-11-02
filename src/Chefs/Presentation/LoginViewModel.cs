using Chefs.Business;
using Chefs.Settings;
using System.Windows.Input;
using Uno.Extensions;
using Windows.Media.Protection.PlayReady;

namespace Chefs.Presentation;

public partial class LoginViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    private LoginViewModel(
        INavigator navigator, 
        IUserService userService)
    {
        _navigator = navigator;
        _userService = userService;
    }

    public IState<Credentials> Credentials => State<Credentials>.Empty(this);

    public ICommand Login => Command.Create(b => b.Given(Credentials).When(CanLogin).Then(DoLogin));

    private bool CanLogin(Credentials credentials)
        => credentials is { Email.Length: > 0 } and { Password.Length: > 0 };

    private async ValueTask DoLogin(Credentials credentials, CancellationToken ct)
    {
        if(await _userService.BasicAuthenticate(
            credentials.Email ?? String.Empty, 
            credentials.Password ?? String.Empty, 
            ct))
        {
            await _navigator.NavigateViewModelAsync<HomeViewModel>(this, data: Option.Some(credentials), cancellation: ct);
        }
    }

    public async ValueTask DoRegisterNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RegisterViewModel>(this, cancellation: ct);
    }
}
