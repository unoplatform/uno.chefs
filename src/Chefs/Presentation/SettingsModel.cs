using Chefs.Business;
using Uno.Extensions.Toolkit;
using AppTheme = Uno.Extensions.Toolkit.AppTheme;

namespace Chefs.Presentation;

public partial class SettingsModel
{
    private readonly INavigator _navigator;
    private readonly IUserService _userService;
    private readonly IThemeService _themeService;

    public SettingsModel(
        IThemeService themeService, 
        INavigator navigator, 
        IUserService userService, 
        User user)
    {
        _navigator = navigator;
        _userService = userService;
        _themeService = themeService;
        Profile = State.Value(this, () => user);
    }

    public IState<User> Profile { get; }

    public IState<AppConfig> Settings => State<AppConfig>.Async(this, async (ct) => await _userService.GetSettings(ct));

    public async ValueTask Update(User profile, CancellationToken ct)
    {
        var settings = await Settings;
        if(settings?.IsDark ?? false) await _themeService.SetThemeAsync(AppTheme.Dark);
        else await _themeService.SetThemeAsync(AppTheme.Light);
        await _userService.Update(profile, ct);
        await _userService.SetSettings((await Settings)!, ct);
        await _navigator.NavigateBackAsync(this);
    }
}
