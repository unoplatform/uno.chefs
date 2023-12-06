using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chefs.Presentation.Extensions;

public static class INavigatorExtensions
{
	public static async ValueTask NavigateToProfile(this INavigator navigator, object sender, User? profile = null)
	{
		var response = await navigator.NavigateRouteForResultAsync<Recipe?>(sender, "Profile", data: profile);
		var result = await response!.Result;

		//If a Recipe was selected, navigate to the RecipeDetails. Otherwise, do nothing
		await (result.SomeOrDefault() switch
		{
			Recipe recipe => navigator.NavigateDataAsync(sender, recipe),
			_ => Task.CompletedTask,
		});
	}
}
