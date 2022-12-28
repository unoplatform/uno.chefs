namespace Chefs.Presentation;

public partial class RegisterModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;

    public RegisterModel(INavigator navigator) 
    {
        _navigator = navigator;
    }

    // DR_REV: XAML only nav
    // We have utu:CommandExtensions.Command
    public async ValueTask NavigateBack(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(this, cancellation: ct);
    }
}
