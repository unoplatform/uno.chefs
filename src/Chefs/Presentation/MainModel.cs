using Chefs.Business;

namespace Chefs.Presentation;

public partial class MainModel
{
    private readonly IUserService _userService;

    public string? Title { get; }

    private readonly INavigator _navigator;

    public MainModel(
        IUserService userService,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        Title = $"Main - {appInfo?.Value?.Title ?? "Chefs"}";
        _navigator = navigator;
        _userService = userService;
    }

    public async Task ShowProfile()
    {
        var response = await _navigator.NavigateRouteForResultAsync<IChefEntity>(this, "Profile", qualifier: Qualifiers.Dialog);
        var result = await response!.Result;

        await (result.SomeOrDefault() switch
        {
            UpdateCookbook updateCookbook => _navigator.NavigateViewModelAsync<UpdateCookbookModel>(this, data: updateCookbook.Cookbook, qualifier: Qualifiers.Nested),
            Cookbook cookbook when cookbook.Id == Guid.Empty => _navigator.NavigateViewModelAsync<CreateCookbookModel>(this, qualifier: Qualifiers.Nested),
            Cookbook cookbook => _navigator.NavigateViewModelAsync<CookbookDetailModel>(this, data: cookbook, qualifier: Qualifiers.Nested),
            object obj when obj is not null && obj.GetType() != typeof(object) => _navigator.NavigateDataAsync(this, obj, qualifier: Qualifiers.Nested),
            _ => Task.CompletedTask,
        });
    }

    public IFeed<User> UserProfile => _userService.UserFeed;
}
