﻿namespace Chefs.Presentation;

public partial class RegisterViewModel
{
    private readonly INavigator _navigator;

    public RegisterViewModel(INavigator navigator) 
    {
        _navigator = navigator;
    }

    public async ValueTask NavigateBack(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(this, cancellation: ct);
    }
}
