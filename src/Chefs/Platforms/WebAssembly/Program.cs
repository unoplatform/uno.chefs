namespace Chefs;

public class Program
{
	private static App? _app;

	public static int Main(string[] args)
	{
		Uno.UI.Xaml.Media.FontFamilyHelper.PreloadAsync("ms-appx:///Assets/Fonts/MaterialIcons-Regular.ttf#Material Symbols Outlined");
		Uno.UI.Xaml.Media.FontFamilyHelper.PreloadAsync("ms-appx:///Assets/Fonts/FontAwesome-Brands.otf#Font Awesome 6 Brands");

		Microsoft.UI.Xaml.Application.Start(_ => _app = new App());

		return 0;
	}
}
