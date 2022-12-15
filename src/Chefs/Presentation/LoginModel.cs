using Chefs.Business;
using Chefs.Settings;
using System.Windows.Input;
using Uno.Extensions;
using Uno.Extensions.Configuration;
using Windows.Media.Protection.PlayReady;

namespace Chefs.Presentation;

public partial class LoginModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;
    private readonly IWritableOptions<Credentials> _credentialOptions;

    public LoginModel(
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
        // DR_REV: Dead code
        //if(await _userService.BasicAuthenticate(credentials?.Email ?? "", credentials?.Password ?? "", ct))
        //{
        //    await _navigator.NavigateViewModelAsync<MainViewModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
        //}

        await _navigator.NavigateViewModelAsync<MainModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
    }
        
	// DR_REV: XAML only nav
    public async ValueTask RegisterNavigation(CancellationToken ct)
		=> await _navigator.NavigateViewModelAsync<RegisterModel>(this, cancellation: ct);

	// DR_REV: Duplicate method
	public async ValueTask DoRegisterNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RegisterModel>(this, cancellation: ct);
    }
}
