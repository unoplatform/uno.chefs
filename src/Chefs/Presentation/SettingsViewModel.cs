using Chefs.Business;
using Uno.Extensions.Configuration;

namespace Chefs.Presentation;

public partial class SettingsViewModel
{
    private readonly IUserService _userService;
    private readonly INavigator _navigator;

    public SettingsViewModel(INavigator navigator,
        IUserService userService)
    {
        _navigator = navigator;
        _userService = userService;
    }

    IState<User> Profile => State<User>.Async(this, async (ct) => await _userService.GetUser(ct));

    IState<ChefApp> Settings => State<ChefApp>.Async(this, async (ct) => await _userService.GetChefSettings(ct));

    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }

    public async ValueTask DoUpdate(CancellationToken ct)
    {

        var settings = await Settings;
        var user = await Profile;

        await _userService.UpdateUserInfo(user!, ct);
        await _userService.SetCheffSettings(settings!, ct);
    }


}
