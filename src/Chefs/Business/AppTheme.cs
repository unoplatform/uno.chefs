using Microsoft.UI.Xaml;
using Uno.Toolkit.UI;

namespace Chefs.Business;

public class AppTheme : IAppTheme
{
    private readonly IDispatcher _dispatcher;
    public AppTheme(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    public bool IsDark => SystemThemeHelper.IsRootInDarkMode(Microsoft.UI.Xaml.Window.Current.Content.XamlRoot!);

    public async Task SetThemeAsync(bool darkMode)
    {
        await _dispatcher.ExecuteAsync(() =>
        {
            SystemThemeHelper.SetRootTheme(Microsoft.UI.Xaml.Window.Current.Content.XamlRoot!, darkMode);
        });
    }
}
