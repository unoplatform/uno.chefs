using Android.Views;

namespace Chefs.Droid
{
	[Activity(
		MainLauncher = true,
		ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
		WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden
	)]
	public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
	{
	}
}
