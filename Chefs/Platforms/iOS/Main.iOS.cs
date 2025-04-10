using UIKit;

namespace Chefs.iOS;

public class EntryPoint
{
	// This is the main entry point of the application.
	public static void Main(string[] args)
	{
		App.InitializeLogging();

#if !IS_UIKIT_SKIA
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(App));
#else
		var host = new Uno.UI.Runtime.Skia.AppleUIKit.AppleUIKitHost(() => new App());
		host.Run();
#endif
	}
}
