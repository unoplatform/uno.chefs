using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chefs.Views.Flyouts;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation.Extensions;

public static class INavigatorExtensions
{
	// TODO: Eventually this pattern should go away.
	// Instead of returning a value from the profile and then using that to navigate forward, the pattern should be:
	// - Navigate to "Recipe" and for navigation to work out that it needs to close the current dialog and then,
	// - Navigate to Recipe on the main window.
	// This is the blocker issue right now for this - https://github.com/unoplatform/uno.extensions/issues/1394
	//public static async ValueTask NavigateToProfile(this INavigator navigator, object sender, User? profile = null)
	//{
	//	var response = await navigator.NavigateRouteForResultAsync<Recipe?>(sender, "Profile", qualifier: Qualifiers.Dialog, data: profile);
	//	var result = await response!.Result;

	//	//If a Recipe was selected, navigate to the RecipeDetails. Otherwise, do nothing
	//	await (result.SomeOrDefault() switch
	//	{
	//		Recipe recipe => navigator.NavigateDataAsync(sender, recipe),
	//		_ => Task.CompletedTask,
	//	});
	//}

	public static Task NavigateToProfile(this INavigator navigator, object sender, User? profile = null)
	{
		return navigator.NavigateRouteAsync(sender, "Profile", qualifier: Qualifiers.Dialog, data: profile);
	}

	public static Task NavigateToNotifications(this INavigator navigator, object sender)
	{
		return navigator.NavigateRouteAsync(sender, "Notifications", qualifier: Qualifiers.Dialog);
	}

	public static Task<NavigationResponse?> ShowDialog(this INavigator navigator, object sender, DialogInfo dialogInfo, CancellationToken ct)
	{
		return navigator.NavigateDataAsync(sender, new DialogInfo(dialogInfo.Title, dialogInfo.Content), cancellation: ct);
	}
}
