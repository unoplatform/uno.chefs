using Chefs.Business;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class CookbookDetailViewModel
{
    private readonly INavigator _navigator;

    public CookbookDetailViewModel(INavigator navigator, Cookbook cookbook)
    {
        _navigator = navigator;
        Cookbook = State.Value(this, () => cookbook);
    }
    public IState<Cookbook> Cookbook { get; set; }

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
        var cookbook = await Cookbook.Value(ct);
        var result = await _navigator.GetDataAsync<UpdateCookbookViewModel, Cookbook>(this, data: cookbook, cancellation: ct);

        if(result is not null)
        {
            await Cookbook.Update(_ => result, ct);
        }
    }

    public async ValueTask Exit(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(this, cancellation: ct);
    }
}
