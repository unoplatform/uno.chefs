using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.UITest.Helpers.Queries;

namespace Chefs.UITests;

public class Constants
{
	public readonly static string ApplicationId = 
#if HAS_SKIA_RENDERER
		"uno.platform.chefs.skia";
#else
		"uno.platform.chefs";
#endif

	public readonly static string WebAssemblyDefaultUri = "http://localhost:51480/";
	public readonly static string iOSAppName = ApplicationId;
	public readonly static string AndroidAppName = ApplicationId;
	public readonly static string iOSDeviceNameOrId = "5998761C-5F52-4209-8EF6-FFAF2A76F393";

	public readonly static Platform CurrentPlatform =
#if TARGET_FRAMEWORK_OVERRIDE_ANDROID
			Platform.Android;
#elif TARGET_FRAMEWORK_OVERRIDE_IOS
			Platform.iOS;
#else
			Platform.Browser;
#endif
}
