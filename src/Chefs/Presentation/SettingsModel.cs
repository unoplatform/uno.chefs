using Chefs.Business;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class SettingsModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    public SettingsModel(INavigator navigator, IUserService userService, User user)
    {
        _navigator = navigator;
        _userService = userService;
        UserInfo = State.Value(this, () => user);
    }

	public IState<User> UserInfo { get; }

    // DR_REV: Useless, keep only one UserInfo or Profile
    public IFeed<User> Profile => UserInfo;

    public IState<AppConfig> Settings => State<AppConfig>.Async(this, async (ct) => await _userService.GetSettings(ct));

    // DR_REV: Use implicit command parameters
    public async ValueTask Update(User profile, CancellationToken ct)
    {
        await _userService.Update(profile, ct);
        await _userService.SetSettings((await Settings)!, ct);

        await Exit(ct);
    }

    public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackAsync(this);
    
}
