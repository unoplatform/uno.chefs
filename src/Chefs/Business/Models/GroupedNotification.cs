namespace Chefs.Business.Models;

public partial record GroupedNotification
{
	private readonly IImmutableList<Notification> _all;

	public GroupedNotification(IEnumerable<Notification> notifications)
	{
		_all = notifications.ToImmutableList();
		Today = _all.Where(x => x.Date.Equals(DateTime.Today)).ToImmutableList();
		Yesterday = _all.Where(x => x.Date.Equals(DateTime.Now.AddDays(-1))).ToImmutableList();
		Older = _all.Where(x => x.Date < DateTime.Now.AddDays(-1)).ToImmutableList();
	}

	public IImmutableList<Notification> Today { get; }
	public bool HasTodayNotifications => Today.Any();
	public IImmutableList<Notification> Yesterday { get; }
	public bool HasYesterdayNotifications => Yesterday.Any();
	public IImmutableList<Notification> Older { get; }
	public bool HasOlderNotifications => Older.Any();

	public IImmutableList<Notification> GetAll() => _all;
}
