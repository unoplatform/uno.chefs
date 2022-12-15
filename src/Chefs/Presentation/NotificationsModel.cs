using Chefs.Business;

namespace Chefs.Presentation;

public partial class NotificationsModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INotificationService _notificationService;
    private readonly INavigator _navigator;

    public NotificationsModel(INavigator navigator, INotificationService notificationService) 
    {
        _navigator = navigator;
        _notificationService = notificationService;
    }

    public IListFeed<Notification> Notifications => ListFeed<Notification>.Async(async ct => await _notificationService.GetAll(ct));

    public IListFeed<Notification> Unread => Notifications.Where(x => !x.Read);

    public IListFeed<Notification> Read => Notifications.Where(x => x.Read);

    // DR_REV: Do not prefix public method with "Do"
    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }
}
