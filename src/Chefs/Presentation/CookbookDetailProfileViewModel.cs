using Chefs.Business;

namespace Chefs.Presentation;

public partial class CookbookDetailProfileViewModel
{
    public CookbookDetailProfileViewModel(Cookbook cookbook)
    {
        PinNumbers = State.Value(this, () => cookbook.PinsNumber);
        Recipes = State.Value(this, () => cookbook.Recipes!).AsListFeed();
    }

    public IState<int> PinNumbers { get; }

    public IListFeed<Recipe> Recipes { get; }

    public async ValueTask CreateCookbookNavigation(Cookbook cookbook, CancellationToken ct)
    {

    }
}
