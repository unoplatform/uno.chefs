using Uno.UI.Hosting;

namespace Chefs;

public class Program
{
#if IS_WASM_SKIA
		static async Task Main(string[] args)
#else
		static int Main(string[] args)
#endif
		{
			App.InitializeLogging();

#if !IS_WASM_SKIA
			Uno.UI.Xaml.Media.FontFamilyHelper.PreloadAsync("ms-appx:///Assets/Fonts/MaterialIcons-Regular.ttf#Material Symbols Outlined");
			Uno.UI.Xaml.Media.FontFamilyHelper.PreloadAsync("ms-appx:///Assets/Fonts/FontAwesome-Brands.otf#Font Awesome 6 Brands");
#endif

#if IS_WASM_SKIA
			var host = UnoPlatformHostBuilder.Create()
				.App(() => new App())
				.UseWebAssembly()
				.Build();

			await host.RunAsync();
#else
			Microsoft.UI.Xaml.Application.Start(_ => new App());
			return 0;
#endif
		}
}
