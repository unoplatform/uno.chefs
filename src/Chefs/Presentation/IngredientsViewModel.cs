using Chefs.Business;

namespace Chefs.Presentation;

public partial class IngredientsViewModel
{
    private INavigator _navigator;

    public IngredientsViewModel(Recipe recipe, INavigator navigator)
    {
        _navigator = navigator;
    }


}
