
using Uno.Extensions.Configuration;
using Uno.Extensions;

namespace Chefs.Presentation;

public class ShellViewModel
{
    private INavigator _navigator;
    private IWritableOptions<Credentials> _credentialsSettings;

    public ShellViewModel(
		INavigator navigator,
        IWritableOptions<Credentials> credentials)
	{
        _navigator = navigator;
        _credentialsSettings = credentials;

		_ = Start();
	}

	public async Task Start()
	{
        var currentCredentials = _credentialsSettings.Value;

        if (currentCredentials is null || !currentCredentials.SkipWelcome)
        {
            var response = await _navigator
                .NavigateViewModelForResultAsync<WelcomeViewModel, bool>(this, Qualifiers.ClearBackStack);

            await _credentialsSettings.UpdateAsync(c => c with { SkipWelcome = true });
        }
        else
        {
            await _navigator.NavigateViewModelAsync<LoginViewModel>(this, Qualifiers.ClearBackStack);
        }
    }
}
