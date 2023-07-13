namespace Chefs.Presentation;

public partial class LoginModel
{
	private readonly INavigator _navigator;
	private readonly IUserService _userService;
	private readonly IWritableOptions<Credentials> _credentialOptions;
	//private bool isRegisterToggle = false;

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
		Username = _credentialOptions.Value != null
			? _credentialOptions.Value.Username!
			: string.Empty,
		Password = string.Empty,
		SkipWelcome = false,
		SaveCredentials = _credentialOptions.Value != null
			? _credentialOptions.Value.SaveCredentials!
			: false,
	});

	public IState<bool> IsRegisterToggle => State<bool>.Value(this, () => false);

	public ICommand Login => Command.Create(b => b.Given(Credentials).When(CanLogin).Then(DoLogin));


	public async ValueTask ToggleRegister(CancellationToken ct)
	{
		var isRegister = await IsRegisterToggle;
		await IsRegisterToggle.Update(_ => !isRegister, ct);
	}

	private bool CanLogin(Credentials credentials)
		=> credentials is { Username.Length: > 0 } and { Password.Length: > 0 };

	private async ValueTask DoLogin(Credentials credentials, CancellationToken ct)
	{
		await _navigator.NavigateViewModelAsync<MainModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
	}
}
