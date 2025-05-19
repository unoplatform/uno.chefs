using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Uno.UITest.Selenium;
using Uno.UITests.Helpers;
using Xamarin.UITest.Shared.Extensions;
using Query = System.Func<Uno.UITest.IAppQuery, Uno.UITest.IAppQuery>;

namespace Chefs.UITests;

[AutoRetry]
public class Given_WelcomePage : TestBase
{
	[Test]
	public void When_SmokeTest()
	{
		Helpers.Wait(seconds: 3);

		PlatformHelpers.On(
			iOS: () => App.WaitForElement(q => q.Class("UnoSKMetalView")),
			Android: () => App.WaitForElement(q => q.Class("UnoSKCanvasView")),
			Browser: () => App.WaitForElement(q => q.Id("uno-canvas"))
		);

		TakeScreenshot("Launched");

		PlatformHelpers.On(
			// Cannot use backdoors on iOS, AppDelegate is inaccessible
			iOS: () => { },
			Android: () => AssertWelcomePage(),
			Browser: () => AssertWelcomePage()
		);

		TakeScreenshot("WelcomePage");
	}

	private void AssertWelcomePage() => App.WaitFor(() => GetCurrentPage().EndsWithIgnoreCase("WelcomePage"), timeoutMessage: "Timed out waiting for WelcomePage");

	private string GetCurrentPage() => (App.InvokeGeneric("browser:SampleRunner|GetCurrentPage", "") as string) ?? string.Empty;
}
