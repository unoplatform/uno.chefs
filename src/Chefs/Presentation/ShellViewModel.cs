
using Uno.Extensions.Configuration;
using Uno.Extensions;

namespace Chefs.Presentation;

public class ShellViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IWritableOptions<Credentials> _credentialsSettings;

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
        await _navigator.NavigateViewModelAsync<WelcomeViewModel>(this, Qualifiers.ClearBackStack);

        // DR_REV: Dead code
        //var currentCredentials = _credentialsSettings.Value;

        //if (currentCredentials is null || !currentCredentials.SkipWelcome)
        //{
        //    await _navigator.NavigateViewModelAsync<WelcomeViewModel>(this, Qualifiers.ClearBackStack);

        //    await _credentialsSettings.UpdateAsync(c => c with { SkipWelcome = true });
        //}
        //else
        //{
        //    await _navigator.NavigateViewModelAsync<LoginViewModel>(this, Qualifiers.ClearBackStack);
        //}
    }
}
