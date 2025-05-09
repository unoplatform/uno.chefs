using System.Diagnostics;
using System.Text.Json;
using Chefs.Services;
using Chefs.Services.Clients;
using Chefs.Services.Settings;

#if __IOS__
using Foundation;
#endif
using LiveChartsCore;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Uno.Extensions.Http.Kiota;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace Chefs;

public partial class App : Application
{
	/// <summary>
	/// Initializes the singleton application object. This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	public static Window? MainWindow;

	public static ShellControl? Shell;
	public static IHost? Host { get; private set; }

	protected override async void OnLaunched(LaunchActivatedEventArgs args)
	{
#if __IOS__ && !__MACCATALYST__ && USE_UITESTS
		//Xamarin.Calabash.Start();
#endif

		var builder = this.CreateBuilder(args);
		ConfigureAppBuilder(builder);
		MainWindow = builder.Window;


#if DEBUG
		MainWindow.UseStudio();
#endif

		Host = await builder.NavigateAsync<ShellControl>();
		Shell = MainWindow.Content as ShellControl;
		await InitializeUserSettings();
	}

	private async Task InitializeUserSettings()
	{
		if (Host is null || MainWindow is null)
		{
			return;
		}

		var config = Host.Services.GetRequiredService<IOptions<AppConfig>>();
		var settingsService = Host.Services.GetRequiredService<ISettingsService>();
		var themeService = MainWindow.GetThemeService();
		var appTheme = config.Value?.IsDark switch
		{
			true => AppTheme.Dark,
			false => AppTheme.Light,
			_ => AppTheme.System
		};

		await settingsService.UpdateSettings(CancellationToken.None, isDark: themeService.IsDark);
		await themeService.SetThemeAsync(appTheme);
	}

	/// <summary>
	/// Configures global Uno Platform logging
	/// </summary>
	public static void InitializeLogging()
	{
		//-:cnd:noEmit
#if true // DEBUG
		// Logging is disabled by default for release builds, as it incurs a significant
		// initialization cost from Microsoft.Extensions.Logging setup. If startup performance
		// is a concern for your application, keep this disabled. If you're running on the web or
		// desktop targets, you can use URL or command line parameters to enable it.
		//
		// For more performance documentation: https://platform.uno/docs/articles/Uno-UI-Performance.html

		var factory = LoggerFactory.Create(builder =>
		{
#if __WASM__
			builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__ || __MACCATALYST__
			builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
			builder.AddConsole();
#else
			builder.AddConsole();
#endif

			// Exclude logs below this level
			builder.SetMinimumLevel(LogLevel.Information);

			// Default filters for Uno Platform namespaces
			builder.AddFilter("Uno", LogLevel.Information);
			builder.AddFilter("Windows", LogLevel.Information);
			builder.AddFilter("Microsoft", LogLevel.Information);

			// Generic Xaml events
			// builder.AddFilter("Microsoft.UI.Xaml", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.VisualStateGroup", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.StateTriggerBase", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.UIElement", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.FrameworkElement", LogLevel.Trace );

			// Layouter specific messages
			// builder.AddFilter("Microsoft.UI.Xaml.Controls", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.Controls.Layouter", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.Controls.Panel", LogLevel.Debug );

			// builder.AddFilter("Windows.Storage", LogLevel.Debug );

			// Binding related messages
			// builder.AddFilter("Microsoft.UI.Xaml.Data", LogLevel.Debug );
			// builder.AddFilter("Microsoft.UI.Xaml.Data", LogLevel.Debug );

			// Binder memory references tracking
			// builder.AddFilter("Uno.UI.DataBinding.BinderReferenceHolder", LogLevel.Debug );

			// DevServer and HotReload related
			// builder.AddFilter("Uno.UI.RemoteControl", LogLevel.Information);

			// Debug JS interop
			// builder.AddFilter("Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug );
		});

		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
		global::Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif
#endif
		//+:cnd:noEmit
	}
#if USE_UITESTS

#if __IOS__
	[Export("getCurrentPage:")]
	public NSString GetCurrentPageBackdoor(NSString value) => new NSString(App.GetCurrentPage());
#endif

#if __WASM__
	[System.Runtime.InteropServices.JavaScript.JSExport]
#endif
	public static string GetCurrentPage() => Shell?.RootFrame?.CurrentSourcePageType.ToString() ?? string.Empty;
#endif
}
