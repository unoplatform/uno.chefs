
using System.Collections.Immutable;
using Chefs.Business;
using Chefs.Data;

namespace Chefs.Presentation;

public partial class ProfileViewModel
{
    private readonly IUserService _userService;
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private User? _user;

    public ProfileViewModel(INavigator navigator,
        ICookbookService cookbookService,
        IRecipeService recipeService,
        IUserService userService,
        User user)
    {
        _navigator = navigator;
        _cookbookService = cookbookService;
        _recipeService = recipeService;
        _userService = userService;
        _user = user;
    }

    public IState<bool> IsMyProfile => State<bool>.Value(this, () => _user is null);

    public IFeed<User> Profile => Feed<User>.Async(async ct => await Task.Run(() => new User(new UserData()
    {
        UrlProfileImage = "https://picsum.photos/60/60",
        FullName = "James Bondi",
        Description = "Passionate about food and life",
        Followers = 450,
        Following = 124
    })));

    //public IFeed<User> Profile => Feed<User>.Async(async ct => _user ?? await _userService.GetCurrent(ct));

    //public IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetByUser(_user?.Id ?? new Guid(), ct));

    // public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetByUser(_user?.Id ?? new Guid(), ct));

    private IImmutableList<Cookbook> GetCookbooksMock(CancellationToken ct) => (new List<Cookbook>()
            {
                new Cookbook(new CookbookData()
                {
                    Name = "BreakFast",
                    PinsNumber = 3
                }),
                new Cookbook(new CookbookData()
                {
                    Name = "Easy",
                    PinsNumber = 10
                })
            }).ToImmutableList();

    public IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await Task.Run(() => GetCookbooksMock(ct)));

    private IImmutableList<Recipe> GetRecipesMock(CancellationToken ct) => (new List<Recipe>()
            {
                new Recipe(new RecipeData()
                {
                    ImageUrl = "https://picsum.photos/70/146",
                    Name = "Grilled Tofu Sandwich",
                    Calories = "268 kcal",
                    CookTime = new TimeSpan(0, 10, 0),
                })
            }).ToImmutableList();

    public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await Task.Run(() => GetRecipesMock(ct)));

    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }

    public async ValueTask DoSettingsNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<SettingsViewModel>(this, cancellation: ct);
    }

    public async ValueTask DoRecipeNavigation(Recipe recipe, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe, cancellation: ct);
    }

    public async ValueTask DoCookbookNavigation(Cookbook cookbook, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: cookbook, cancellation: ct);
    }
}
