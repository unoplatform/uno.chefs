namespace Chefs.Presentation;

public partial class NotificationsViewModel
{
    private readonly INotificationService _notificationService;
    private readonly INavigator _navigator;

    public NotificationsViewModel(INavigator navigator, INotificationService notificationService) 
    {
        _navigator = navigator;
        _notificationService = notificationService;
    }

    IListFeed<Notification> Notifications => ListFeed<Notification>.Async(async ct => await _notificationService.GetAll(ct));

    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }
}
