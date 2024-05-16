namespace Chefs.Presentation;

public partial record LoginModel(IDispatcher Dispatcher, INavigator Navigator, IAuthenticationService Authentication)
{
    public string Title { get; } = "Login";

    public IState<Credentials> UserCredentials => State<Credentials>.Value(this, () => new Credentials());

    public async ValueTask Login(CancellationToken token)
    {
        var credentials = await UserCredentials.Value(token);
        var username = credentials?.Username ?? string.Empty;
        var password = credentials?.Password ?? string.Empty;

        var success = await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string> { { "Username", username }, { "Password", password } });
        if (success)
        {
            await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack, cancellation: token);
        }
    }
}
