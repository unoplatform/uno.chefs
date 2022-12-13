using Chefs.Business;

namespace Chefs.Presentation;

public partial class NotificationsViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INotificationService _notificationService;
    private readonly INavigator _navigator;

    public NotificationsViewModel(INavigator navigator, INotificationService notificationService) 
    {
        _navigator = navigator;
        _notificationService = notificationService;
    }

	// DR_REV: Always specify the access modifier
	public IListFeed<Notification> Notifications => ListFeed<Notification>.Async(async ct => await _notificationService.GetAll(ct));

    // DR_REV: Do not prefix public method with "Do"
    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }
}
