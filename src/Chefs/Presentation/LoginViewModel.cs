using Chefs.Settings;
using System.Windows.Input;
using Uno.Extensions;

namespace Chefs.Presentation;

public partial class LoginViewModel
{
    private readonly INavigator _navigator;

    private LoginViewModel(
        INavigator navigator)
    {
        _navigator = navigator;
    }

    public IState<Credentials> Credentials => State<Credentials>.Empty(this);

    public ICommand Login => Command.Create(b => b.Given(Credentials).When(CanLogin).Then(DoLogin));

    private bool CanLogin(Credentials credentials)
        => credentials is { Email.Length: > 0 } and { Password.Length: > 0 };

    private async ValueTask DoLogin(Credentials credentials, CancellationToken ct)
        => await _navigator.NavigateViewModelAsync<HomeViewModel>(this, data: Option.Some(credentials), cancellation: ct);
}
