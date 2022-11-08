using Chefs.Business;

namespace Chefs.Presentation;

public partial class CookbookDetailSavedViewModel
{
    public CookbookDetailSavedViewModel(Cookbook cookbook)
    {
        Cookbook = State.Value(this, () => cookbook);
    }

    public IFeed<Cookbook> Cookbook { get; }

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {

    }
}
