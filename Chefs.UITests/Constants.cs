using Uno.UITest.Helpers.Queries;

namespace Chefs.UITests;

public class Constants
{
	public static readonly string ApplicationId =
#if HAS_SKIA_RENDERER
		"uno.platform.chefs.skia";

#else
		"uno.platform.chefs";
#endif

	public static readonly string WebAssemblyDefaultUri = "http://localhost:51480/";
	public static readonly string IOSAppName = ApplicationId;
	public static readonly string AndroidAppName = ApplicationId;
	public static readonly string IOSDeviceNameOrId = "5998761C-5F52-4209-8EF6-FFAF2A76F393";

	public static readonly Platform CurrentPlatform =
#if TARGET_FRAMEWORK_OVERRIDE_ANDROID
			Platform.Android;
#elif TARGET_FRAMEWORK_OVERRIDE_IOS
			Platform.iOS;
#else
			Platform.Browser;

#endif
}
