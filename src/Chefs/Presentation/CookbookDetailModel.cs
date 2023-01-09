﻿using System.Collections.Immutable;
using Chefs.Business;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class CookbookDetailModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private Cookbook? _cookbook;

    public CookbookDetailModel(INavigator navigator, Cookbook cookbook)
    {
        _navigator = navigator;
        _cookbook = cookbook;
    }

    public IState<Cookbook> Cookbook => State<Cookbook>.Value(this, () => _cookbook ?? new Cookbook());

    private IListState<Recipe> _recipes => ListState<Recipe>.Value(this, () => _cookbook?.Recipes ?? ImmutableList<Recipe>.Empty);

    public IListFeed<Recipe> Recipes => _recipes;

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
		// DR_REV: You are supposed to be able to data-bind 2-way to the Cookbook.Value do nav in XAML only

		var cookbook = await Cookbook.Value(ct);
        var result = await _navigator.GetDataAsync<UpdateCookbookModel, Cookbook>(this, data: cookbook, cancellation: ct);

        if(result is not null)
        {
            _cookbook = result;
            //await Cookbook.Update(_ => _cookbook, ct);
        }
    }

    public async ValueTask Exit(CancellationToken ct)
    {
		// DR_REV: You should be able to use XAML nav only and remove this method

		await _navigator.NavigateBackAsync(this, cancellation: ct);
    }
}
