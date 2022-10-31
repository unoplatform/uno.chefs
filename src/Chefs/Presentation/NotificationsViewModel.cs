using Chefs.Business;

namespace Chefs.Presentation;

public partial class NotificationsViewModel
{
    INotificationService _notificationService;

    public NotificationsViewModel(INotificationService notificationService) 
    { 
        _notificationService = notificationService;
    }

    IListFeed<Notification> Notifications => ListFeed<Notification>.Async(async ct => await _notificationService.GetAll(ct));
}
