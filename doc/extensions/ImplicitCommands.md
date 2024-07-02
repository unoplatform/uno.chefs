---
uid: Uno.Recipes.ImplicitCommands
---

# How to Use Implicit Commands with MVUX

## Problem

Using commands in XAML often requires explicitly defining command properties for each control, which can lead to verbose and repetitive code. This approach can also make it difficult to manage and update command bindings consistently across the application.

## Solution

The `Uno.Extensions` library provides an elegant solution with implicit commands. This allows commands to be defined directly in XAML using the `Command` attached property, simplifying the command binding process and improving code maintainability. By default, a command property will be generated in the bindable proxy for each method on the Model that has no return value or is asynchronous (e.g. returns `ValueTask` or `Task`).

### Using Implicit Commands in the Login Page

In the Chefs app for the `Login` page, we use the Command property to bind the Login command directly to the Button control.

```xml
<Button Content="Login"
		Padding="24,20"
		CornerRadius="4"
		Command="{Binding Login}" />
```

For which the `LoginModel` contains the following method:

```csharp
public partial record LoginModel(IDispatcher Dispatcher, INavigator Navigator, IAuthenticationService Authentication)
{
    public async ValueTask Login(Credentials userCredentials)
    {
        var success = await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string>
        {
            { "Username", userCredentials?.Username ?? string.Empty },
            { "Password", userCredentials?.Password ?? string.Empty }
        });
        if (success)
        {
            await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack);
        }
    }
}
```

## Source Code

Chefs app

- [Login Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/LoginPage.xaml)

- [Login Model](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Presentation/LoginModel.cs)

## Documentation

- [MVUX Commands](xref:Uno.Extensions.Mvux.Advanced.Commands)