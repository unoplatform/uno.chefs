using NUnit.Framework;

namespace Chefs.UITest;

public class Given_WelcomePage : TestBase
{
	[Test]
	public async Task When_Welcome_FlipView()
	{
		App.WaitElement("WelcomePageFirstOnboardingTextBlock");
		// Wait for page to load
		await Task.Delay(1000);
		var firstPage = TakeScreenshot("When_Welcome_FlipView_FirstPage");

		PressNextButton();
		App.WaitElement("WelcomePageSecondOnboardingTextBlock");
		var secondPage = TakeScreenshot("When_Welcome_FlipView_SecondPage");

		ImageAssert.AreNotEqual(firstPage, secondPage);

		PressNextButton();
		App.WaitElement("WelcomePageThirdOnboardingTextBlock");
		var thirdPage = TakeScreenshot("When_Welcome_FlipView_ThirdPage");

		ImageAssert.AreNotEqual(secondPage, thirdPage);
		ImageAssert.AreNotEqual(firstPage, thirdPage);

		PressPreviousButton();
		var secondPageAfter = TakeScreenshot("When_Welcome_FlipView_SecondPage_After");

		ImageAssert.AreEqual(secondPage, secondPageAfter, tolerance: PixelTolerance.Exclusive(Constants.DefaultPixelTolerance));

		PressPreviousButton();
		var firstPageAfter = TakeScreenshot("When_Welcome_FlipView_FirstPage_After");

		ImageAssert.AreEqual(firstPage, firstPageAfter, tolerance: PixelTolerance.Exclusive(Constants.DefaultPixelTolerance));
	}

	#region Helper methods
	private void PressPreviousButton()
	{
		App.WaitThenTap("WelcomePagePreviousButton");
	}

	private void PressNextButton()
	{
		App.WaitThenTap("WelcomePageNextButton");
	}
	#endregion
}
