using System.Collections.Immutable;
using Chefs.Business;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class CookbookDetailModel
{
    private readonly INavigator _navigator;
    private Cookbook? _cookbook;

    public CookbookDetailModel(
        INavigator navigator, 
        Cookbook cookbook)
    {
        _navigator = navigator;
        _cookbook = cookbook;
    }

    public IState<Cookbook> Cookbook => State<Cookbook>.Value(this, () => _cookbook ?? new Cookbook());
}
