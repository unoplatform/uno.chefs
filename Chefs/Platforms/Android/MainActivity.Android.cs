using Android.App;
using Android.OS;
using Android.Views;
using Java.Interop;

namespace Chefs.Droid;

[Activity(
	MainLauncher = true,
	ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
	WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden
)]
public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
{
#if USE_UITESTS
	[Export("GetCurrentPage")]
	public string GetCurrentPage(string unused) => App.GetCurrentPage();
#endif
}
