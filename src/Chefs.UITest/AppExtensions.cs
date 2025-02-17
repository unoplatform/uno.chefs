namespace Chefs.UITest;

public static class AppExtensions
{
	public const int UIWaitTimeInMilliseconds = 1000;

	public static IAppResult[] WaitElement(this IApp app, string marked, string timeoutMessage = "Timed out waiting for element '{0}'...", TimeSpan? timeout = null, TimeSpan? retryFrequency = null, TimeSpan? postTimeout = null)
	{
		Console.WriteLine($"Waiting for '{marked}'");
		return app.WaitForElement(marked, string.Format(timeoutMessage, marked), timeout, retryFrequency, postTimeout);
	}

	public static async Task TapAndWait(this IApp app, string elementToTap, string elementToWaitFor )
	{
		app.WaitElement(elementToTap);
		await Task.Delay(UIWaitTimeInMilliseconds);

		app.Tap(elementToTap);
		app.WaitElement(elementToWaitFor);

		await Task.Delay(UIWaitTimeInMilliseconds);
	}

	public static Func<IAppQuery, IAppQuery> WaitThenTap(this IApp app, string marked, TimeSpan? timeout = null)
	{
		Console.WriteLine($"Waiting (before Tap) for '{marked}'");
		return WaitThenTap(app, q => q.All().Marked(marked), timeout);
	}

	public static Func<IAppQuery, IAppQuery> WaitThenTap(this IApp app, Func<IAppQuery, IAppQuery> query, TimeSpan? timeout = null)
	{
		app.WaitForElement(query, timeout: timeout);
		Console.WriteLine("Tapping element");
		app.Tap(query);

		return query;
	}

	public static async Task SelectListViewIndexAndWait(this IApp app, string listName, string indexToSelect, string elementToWaitFor)
	{
		app.WaitElement(listName);
		await Task.Delay(UIWaitTimeInMilliseconds);

		var list = app.Marked(listName);
		list.SetDependencyPropertyValue("SelectedIndex", indexToSelect);
		await Task.Delay(UIWaitTimeInMilliseconds);

		app.WaitElement(elementToWaitFor);
		await Task.Delay(UIWaitTimeInMilliseconds);
	}
}
