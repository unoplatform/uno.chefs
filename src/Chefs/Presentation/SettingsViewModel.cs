using Chefs.Business;

namespace Chefs.Presentation;

public partial class SettingsViewModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;

    public SettingsViewModel(INavigator navigator, IUserService userService)
    {
        _navigator = navigator;
        _userService = userService;
    }

    IState<User> Profile => State<User>.Async(this, async ct => await _userService.GetUser(ct));

    IState<ChefApp> Settings => State<ChefApp>.Async(this, async (ct) => await _userService.GetSettings(ct));

    public async ValueTask DoUpdate(CancellationToken ct)
    {
        var settings = await Settings;
        var user = await Profile;

        await _userService.UpdateUserInfo(user!, ct);
        await _userService.SetCheffSettings(settings!, ct);
    }

    public async ValueTask DoExit(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }


}
