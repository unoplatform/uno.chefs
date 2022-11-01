
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

            var isSkipWelcome = await response!.Result;

            await _credentialsSettings.UpdateAsync(c => c with { SkipWelcome = isSkipWelcome!.SomeOrDefault() });
        }

        if (currentCredentials?.Email is { Length: > 0 })
        {
            await _navigator.NavigateDataAsync(this, currentCredentials, Qualifiers.ClearBackStack);
        }
        else
        {
            var response = await _navigator.NavigateForResultAsync<Credentials>(this, Qualifiers.ClearBackStack);

            if (response?.Result is null)
            {
                _ = Start();
                return;
            }

            var loginResult = await response.Result;
            if (loginResult.IsSome(out var creds) && creds?.Email is { Length: > 0 })
            {
                await _credentialsSettings.UpdateAsync(c => creds);

                _ = Start();
            }
        }

        await _navigator.NavigateViewModelAsync<MainViewModel>(this);
    }
}
