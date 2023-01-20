using Chefs.Business;

namespace Chefs.Presentation;

public partial class MainModel
{
    private readonly IUserService _userService;

    public string? Title { get; }

	public MainModel(
        IUserService userService,
		IOptions<AppConfig> appInfo)
	{ 
        Title = $"Main - {appInfo?.Value?.Title ?? "Chefs"}";
        _userService = userService;
    }

    public IFeed<User> UserProfile => _userService.UserFeed;
}
