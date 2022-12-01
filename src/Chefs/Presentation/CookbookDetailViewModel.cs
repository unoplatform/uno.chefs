using System.Collections.Immutable;
using Chefs.Business;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class CookbookDetailViewModel
{
    private readonly INavigator _navigator;
    private Cookbook? _cookbook;

    public CookbookDetailViewModel(INavigator navigator, Cookbook cookbook)
    {
        _navigator = navigator;
        _cookbook = cookbook;
    }

    //public IState<Cookbook> Cookbook => State<Cookbook>.Value(this, () => _cookbook ?? new Cookbook());

    private IListState<Recipe> _recipes => ListState<Recipe>.Value(this, () => _cookbook?.Recipes ?? ImmutableList<Recipe>.Empty);

    public IListFeed<Recipe> Recipes => _recipes;

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
        var cookbook = _cookbook;
        var result = await _navigator.GetDataAsync<UpdateCookbookViewModel, Cookbook>(this, data: _cookbook, cancellation: ct);

        if(result is not null)
        {
            //await Cookbook.Update(_ => result, ct);
        }
    }

    public async ValueTask Exit(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(this, cancellation: ct);
    }
}
