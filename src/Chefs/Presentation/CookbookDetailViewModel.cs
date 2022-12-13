using Chefs.Business;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class CookbookDetailViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;

    public CookbookDetailViewModel(INavigator navigator, Cookbook cookbook)
    {
        _navigator = navigator;
        Cookbook = State.Value(this, () => cookbook);
    }

    public IState<Cookbook> Cookbook { get; }

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
		// DR_REV: You are supposed to be able to data-bind 2-way to the Cookbook.Value do nav in XAML only

		var cookbook = await Cookbook.Value(ct);
        var result = await _navigator.GetDataAsync<UpdateCookbookViewModel, Cookbook>(this, data: cookbook, cancellation: ct);

        if(result is not null)
        {
            await Cookbook.Update(_ => result, ct);
        }
    }

    public async ValueTask Exit(CancellationToken ct)
    {
		// DR_REV: You should be able to use XAML nav only and remove this method

		await _navigator.NavigateBackAsync(this, cancellation: ct);
    }
}
