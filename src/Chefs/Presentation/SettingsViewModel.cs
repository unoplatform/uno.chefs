using Chefs.Business;

namespace Chefs.Presentation;

public partial class SettingsViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    public SettingsViewModel(INavigator navigator, IUserService userService, User user)
    {
        _navigator = navigator;
        _userService = userService;
        UserInfo = State.Value(this, () => user);
    }

    public IState<User> UserInfo { get; }

    public IFeed<User> Profile => UserInfo;

    public IState<AppConfig> Settings => State<AppConfig>.Async(this, async (ct) => await _userService.GetSettings(ct));

    public async ValueTask Update(CancellationToken ct)
    {
        var settings = await Settings;
        var user = await Profile;

        await _userService.Update(user!, ct);
        await _userService.SetSettings(settings!, ct);

        await Exit(ct);
    }

    public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackWithResultAsync(this, data: await UserInfo);
    
}
