using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    private readonly IRecipeEndpoint _recipeEndpoint;

    public RecipeService(IRecipeEndpoint recipeEndpoint)
    {
        _recipeEndpoint = recipeEndpoint;
    }

    public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> GetByCategory(Category category, CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Category?.Id == category.Id)
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetCategories(ct))
                   .Select(c => new Category(c))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Cookbook>> GetCookbooks(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetCookbooks(ct))
                   .Select(c => new Cookbook(c))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetPopularCreators(ct))
                   .Select(u => new User(u))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Date == DateTime.Now)
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Date == DateTime.Now && r.Reviews?.Count > 0)
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> Search(SearchFilter search, CancellationToken ct)
    {
        var recipes = (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();
        return GetFilterRecipes(recipes.ToList(), search);
    }

    private IImmutableList<Recipe> GetFilterRecipes(List<Recipe> recipes, SearchFilter searchFilter)
    {
        TimeSpan? time = GetTime(searchFilter.Times);
        if (searchFilter.OrganizeCategories > 0)
        {
            switch (searchFilter.OrganizeCategories)
            {
                case OrganizeCategories.Recommended:
                    return GetRecipesByText(recipes
                        .Where(r => (r.Category == null || r.Category?.Id == searchFilter.Category?.Id)
                        && (r.Difficulty == 0 || r.Difficulty == searchFilter.Difficulty)
                        && (time == null || r.CookTime == time)).ToList(), searchFilter.TextFilter);
                case OrganizeCategories.Popular:
                    return GetRecipesByText(recipes
                        .Where(r => (r.Category == null || r.Category?.Id == searchFilter.Category?.Id)
                        && (r.Difficulty == 0 || r.Difficulty == searchFilter.Difficulty)
                        && (time == null || r.CookTime == time)).ToList(), searchFilter.TextFilter);
                case OrganizeCategories.Recent:
                    return GetRecipesByText(recipes
                        .Where(r => (r.Category == null || r.Category?.Id == searchFilter.Category?.Id)
                        && (r.Difficulty == 0 || r.Difficulty == searchFilter.Difficulty)
                        && (time == null || r.CookTime == time)).ToList(), searchFilter.TextFilter);
                default:
                    return ImmutableList<Recipe>.Empty;
            }

        }
        return ImmutableList<Recipe>.Empty;
    }

    private TimeSpan? GetTime(Times time)
    {
        if (time > 0)
        {
            switch (time)
            {
                case Times.Under15min:
                        return new TimeSpan(0, 15, 00);
                case Times.Under30min:
                    return new TimeSpan(0, 30, 00);
                case Times.Under60min:
                    return new TimeSpan(0, 60, 00);
                default:
                    return null;
            }
        }
        return null;
    }

    private IImmutableList<Recipe> GetRecipesByText(List<Recipe> recipes, string? text)
    {
        return recipes
            .Where(r => (text == null || r.Name?.ToLower() == text.ToLower()) 
            && (text == null || r.Category?.Name?.ToLower() == text.ToLower())).ToImmutableList();
    }
}
