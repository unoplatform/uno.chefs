using Chefs.Business;
using Windows.Devices.Input.Preview;

namespace Chefs.Presentation;

public partial class CookbookDetailSavedViewModel
{
    private readonly INavigator _navigator;
    private Cookbook _cookbook;

    public CookbookDetailSavedViewModel(INavigator navigator, Cookbook cookbook)
    {
        _navigator = navigator;
        _cookbook = cookbook;
    }
    public IState<Cookbook> Cookbook => State.Value(this, () => _cookbook);

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
        var cookbook = await Cookbook.Value(ct);
        var result = await _navigator.GetDataAsync<AddRecipesSavedCookbookViewModel, Cookbook>(this, data: cookbook, cancellation: ct);

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
