using Chefs.Business;

namespace Chefs.Presentation;

public partial class CookbookDetailModel
{
    public CookbookDetailModel(
        Cookbook cookbook)
    {
        Cookbook = State<Cookbook>.Value(this, () => cookbook ?? new Cookbook());
    }

    public IState<Cookbook> Cookbook { get; }
}