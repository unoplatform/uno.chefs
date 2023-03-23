#if WINDOWS_WINUI
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif

namespace Chefs;

public sealed partial class App : Application
{
    private Window? _window;

    public App()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {

        var appBuilder = this.CreateBuilder(args)
                                .ConfigureApp()
                                .UseToolkitNavigation();
        _window = appBuilder.Window;

#if WINDOWS_WINUI
        var m_AppWindow = GetAppWindowForCurrentWindow();

        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            // Hide default title bar.
            titleBar.ExtendsContentIntoTitleBar = true;
        }
#endif

        var host = await appBuilder.NavigateAsync<ShellControl>(
        // Uncomment this block to see the splashscreen for an extended time
        // Note: This will prevent navigation to first page of app, so is just
        // for validating the splashscreen UI
        // See Style in App.Xaml for ExtendedSplashScreen to change splashscreen layout
        //    initialNavigate: async (sp, nav) =>
        //{
        //    await Task.Delay(10000);
        //}
        );

#if WINDOWS_WINUI
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            // Hide default title bar.
            titleBar.ExtendsContentIntoTitleBar = false;
        }
#endif
    }

#if WINDOWS_WINUI
    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(_window);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }
#endif
}