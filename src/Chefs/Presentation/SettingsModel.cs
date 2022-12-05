using Chefs.Business;

namespace Chefs.Presentation;

public partial class SettingsModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    public SettingsModel(INavigator navigator, IUserService userService, User user)
    {
        _navigator = navigator;
        _userService = userService;
        Profile = State.Value(this, () => user);
    }

    public IState<User> Profile { get; }

    public IState<AppConfig> Settings => State<AppConfig>.Async(this, async (ct) => await _userService.GetSettings(ct));

    public async ValueTask Update(User profile, CancellationToken ct)
    {
        await _userService.Update(profile, ct);
        await _userService.SetSettings((await Settings)!, ct);
        await _navigator.NavigateBackAsync(this);
    }
}
