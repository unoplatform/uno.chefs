namespace Chefs.Business.Models;

public partial record GroupedNotification
{
	private readonly IImmutableList<Notification> _all;

	public GroupedNotification(IEnumerable<Notification> notifications)
	{
		_all = notifications.ToImmutableList();
		Today = _all.Where(x => IsSameDate(x.Date, DateTime.Today)).ToImmutableList();
		Yesterday = _all.Where(x => IsSameDate(x.Date, DateTime.Now.AddDays(-1))).ToImmutableList();
		Older = _all.Where(x => x.Date < DateTime.Now.AddDays(-1)).ToImmutableList();
	}

	public IImmutableList<Notification> Today { get; }
	public bool HasTodayNotifications => Today.Any();
	public IImmutableList<Notification> Yesterday { get; }
	public bool HasYesterdayNotifications => Yesterday.Any();
	public IImmutableList<Notification> Older { get; }
	public bool HasOlderNotifications => Older.Any();

	public IImmutableList<Notification> GetAll() => _all;

	private static bool IsSameDate(DateTime date1, DateTime date2)
	{
		// Accessing Date property makes sure we are ignoring the time component of DateTime
		return date1.Date.Equals(date2.Date);
	}
}
