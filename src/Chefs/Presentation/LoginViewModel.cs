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
    private readonly IWritableOptions<Credentials> _credentialOptions;

    public LoginViewModel(
        INavigator navigator, 
        IUserService userService,
        IWritableOptions<Credentials> credentialOptions)
    {
        _navigator = navigator;
        _userService = userService;
        _credentialOptions = credentialOptions;
    }

    public IState<Credentials> Credentials => State<Credentials>.Async(this, async _ => new Credentials()
    {
        Email = _credentialOptions.Value != null 
            ? _credentialOptions.Value.Email! 
            : string.Empty,
        Password = string.Empty,
        SkipWelcome = false,
        SaveCredentials = _credentialOptions.Value != null
            ? _credentialOptions.Value.SaveCredentials!
            : false,
    });


    public ICommand Login => Command.Create(b => b.Given(Credentials).When(CanLogin).Then(DoLogin));

    private bool CanLogin(Credentials credentials)
        => credentials is { Email.Length: > 0 } and { Password.Length: > 0 };

    private async ValueTask DoLogin(Credentials credentials, CancellationToken ct)
    {
        //if(await _userService.BasicAuthenticate(credentials?.Email ?? "", credentials?.Password ?? "", ct))
        //{
        //    await _navigator.NavigateViewModelAsync<MainViewModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
        //}

        await _navigator.NavigateViewModelAsync<MainViewModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
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
