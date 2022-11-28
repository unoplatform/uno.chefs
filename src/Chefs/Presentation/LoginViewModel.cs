using Chefs.Business;
using Chefs.Settings;
using System.Windows.Input;
using Uno.Extensions;
using Uno.Extensions.Configuration;
using Windows.Media.Protection.PlayReady;

namespace Chefs.Presentation;

public partial class LoginViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;
    private readonly IWritableOptions<AuthenticationOptions> _authenticationOptions;

    public LoginViewModel(
        INavigator navigator, 
        IUserService userService,
        IWritableOptions<AuthenticationOptions> authenticationOptions)
    {
        _navigator = navigator;
        _userService = userService;
        _authenticationOptions = authenticationOptions;
    }

    public IState<AuthenticationOptions> AuthOptions => State<AuthenticationOptions>.Async(this, async _ => new AuthenticationOptions()
    {
        Email = _authenticationOptions.Value?.Email ?? string.Empty,
        SaveCredentials = _authenticationOptions.Value?.SaveCredentials ?? false
    });

    public IState<Credentials> Credentials => State<Credentials>.Async(this, async _ => new Credentials()
    {
        Email = _authenticationOptions.Value != null ? _authenticationOptions.Value.Email! : string.Empty,
        Password = string.Empty,
        SkipWelcome = false
    });


    public ICommand Login => Command.Create(b => b.Given(Credentials).When(CanLogin).Then(DoLogin));

    private bool CanLogin(Credentials credentials)
        => credentials is { Email.Length: > 0 } and { Password.Length: > 0 };

    private async ValueTask DoLogin(Credentials credentials, CancellationToken ct)
    {
        if(await _userService.BasicAuthenticate(credentials?.Email ?? "", credentials?.Password ?? "", ct))
        {
            await _navigator.NavigateViewModelAsync<MainViewModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
        }
    }
        
        
    public async ValueTask RegisterNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RegisterViewModel>(this, cancellation: ct);
    }

    public async ValueTask DoRegisterNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RegisterViewModel>(this, cancellation: ct);
    }
}
