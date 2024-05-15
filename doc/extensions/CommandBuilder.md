---
uid: Uno.Recipes.CommandBuilder
---

# How to create commands using ICommandBuilder

## Problem

Sometimes a `Command` property's code can get lengthy and messy, especially when there are a lot of conditions involved. Some commands even implement callbacks. It can sometimes be hard to understand what a `Command` does just by looking at the code.

## Solution

The `ICommand.Create` factory method provides an `ICommandBuilder` parameter that serves as a way to tidily create a command on a single line. It makes commands feel more instinctive by needing at most three methods to build them (_Given_, _When_ and _Then_). You can find more information on these methods [here](xref:Uno.Extensions.Mvux.Advanced.Commands#create--createt). Let's take a look at how `ICommandBuilder` is used in Chefs:

```csharp
public partial class LoginModel
{
    ...

    public IState<Credentials> Credentials => ...

    public ICommand Login => Command.Create(b => b.Given(Credentials).When(CanLogin).Then(DoLogin));

    private bool CanLogin(Credentials credentials)
		=> credentials is { Username.Length: > 0 } and { Password.Length: > 0 };

	private async ValueTask DoLogin(Credentials credentials, CancellationToken ct)
	{
		await _navigator.NavigateViewModelAsync<MainModel>(this, Qualifiers.ClearBackStack, Option.Some(credentials), ct);
	}
}
```

When the `Login` command is triggered, the __Given__ method will _get_ the current _Credentials_ and pass it to the __When__ method, which will check if the command _can_ be executed. The __Then__ method represents the callback to be invoked when the command is executed.

We can see in this example that the Login command is created, then it uses the _Credentials_ of the user to check if they _CanLogin_. If successful, then _DoLogin_ the user.

## Source Code

Chefs app

- [LoginModel ICommand](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/LoginModel.cs#L34)
- [ReviewsModel ICommand](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/ReviewsModel.cs#L46)

## Documentation

- [Uno.Extensions ICommandBuilder Parameters](xref:Uno.Extensions.Mvux.Advanced.Commands#create--createt)