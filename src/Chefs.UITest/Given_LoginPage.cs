using NUnit.Framework;

namespace Chefs.UITest;

public class Given_LoginPage : TestBase
{
	[Test]
	public void When_Login()
	{
		App.WaitThenTap("WelcomePageLoginButton");

		App.WaitElement("LoginPageUsernameTextBox");
		App.EnterText("LoginPageUsernameTextBox", "chefs@platform.uno");

		App.WaitElement("LoginPagePasswordTextBox");
		App.EnterText("LoginPagePasswordTextBox", "uno123");

		App.WaitThenTap("LoginPageLoginButton");

		App.WaitElement("HomePageNavigationBar");
	}
}
