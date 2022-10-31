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

    IState<User> Profile => State<User>.Async(this, async ct => await _userService.GetCurrent(ct));

    IState<ChefApp> Settings => State<ChefApp>.Async(this, async (ct) => await _userService.GetSettings(ct));

    public async ValueTask DoUpdate(CancellationToken ct)
    {
        var settings = await Settings;
        var user = await Profile;

        await _userService.Update(user!, ct);
        await _userService.SetSettings(settings!, ct);
    }

    public async ValueTask DoExit(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }


}
