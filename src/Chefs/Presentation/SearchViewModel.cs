using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class SearchViewModel
{
    private IRecipeService _recipeService;

    public SearchViewModel(SearchFilter filters, IRecipeService recipeService)
    {
        _recipeService = recipeService;

        Filter = State.Value(this, () => filters);
    }

    public IState<SearchFilter> Filter { get; }

    //private async ValueTask<IImmutableList<Recipe>> ApplyFilter((IImmutableList<Recipe> products, SearchFilter? filter) inputs)
    //    => await _recipeService.Search(inputs.filter?, new CancellationToken());
}
