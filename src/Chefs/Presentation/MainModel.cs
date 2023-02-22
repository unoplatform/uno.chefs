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

        ProfileResponse = State<IChefEntity>.Empty(this);
        ProfileResponse.ForEachAsync(async (obj, ct) =>
        {
            if (obj is not null && 
            obj.GetType() != typeof(object))
            {
                await _navigator.NavigateDataAsync(this, obj, qualifier:Qualifiers.Nested);
            }
        });
    }

    public IState<IChefEntity> ProfileResponse { get; }


    public IFeed<User> UserProfile => _userService.UserFeed;
}
