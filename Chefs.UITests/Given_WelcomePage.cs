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

[AutoRetry(5)]
public class Given_WelcomePage : TestBase
{
	[Test]
	public void When_SmokeTest()
	{
		Helpers.Wait(seconds: 3);

#if HAS_SKIA_RENDERER
		PlatformHelpers.On(
			iOS: () => App.WaitForElement(q => q.Class("UnoSKMetalView")),
			Android: () => App.WaitForElement(q => q.Class("UnoSKCanvasView")),
			Browser: () => App.WaitForElement(q => q.Id("uno-canvas"))
		);
#endif

		TakeScreenshot("Launched");

#if HAS_SKIA_RENDERER
		PlatformHelpers.On(
			// Cannot use backdoors on iOS, AppDelegate is inaccessible
			iOS: () => { },
			Android: () => AssertWelcomePage(),
			Browser: () => AssertWelcomePage()
		);

		TakeScreenshot("WelcomePage");
#else
		Login();
#endif

	}

	private void AssertWelcomePage()
	{
		App.WaitFor(() => GetCurrentPage().EndsWithIgnoreCase("WelcomePage"), timeoutMessage: "Timed out waiting for WelcomePage");
	}

	private string GetCurrentPage()
	{
		return (App.InvokeGeneric("browser:SampleRunner|GetCurrentPage", "") as string) ?? string.Empty;
	}

#if !HAS_SKIA_RENDERER
	private void Login()
	{
		var skipButton = new QueryEx(q => q.All().Marked("SkipButton"));
		var username = new QueryEx(q => q.All().Marked("LoginUsername"));
		var password = new QueryEx(q => q.All().Marked("LoginPassword"));
		var loginButton = new QueryEx(q => q.All().Marked("LoginButton"));
		var trendingNow = new QueryEx(q => q.All().Marked("TrendingNowFeed"));

		App.WaitForElement(skipButton, timeoutMessage: "Timed out waiting for WelcomePage");

		TakeScreenshot("WelcomePage");

		App.Tap(skipButton);
		
		App.WaitForElement(loginButton, timeoutMessage: "Timed out waiting for LoginPage");

		TakeScreenshot("LoginPage");
		
		App.EnterText(username, "testuser");
		App.EnterText(password, "testpassword");
		App.Tap(loginButton);

		App.WaitForElement(trendingNow);

		TakeScreenshot("HomePage");
	}
#endif
}
