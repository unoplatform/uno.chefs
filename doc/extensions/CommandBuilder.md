---
uid: Uno.Recipes.CommandBuilder
---

# How to create commands using ICommandBuilder

## Problem

Sometimes a `Command` property's code can get lengthy and messy, especially when there are a lot of conditions involved. Some commands even implement callbacks. It can sometimes be hard to understand what a `Command` does just by looking at the code.

## Solution

The `ICommand.Create` factory method provides an `ICommandBuilder` parameter that serves as a way to tidily create a command on a single line. It makes commands feel more instinctive by needing at most three methods to build them (_Given_, _When_ and _Then_). You can find more information on these methods [here](xref:Uno.Extensions.Mvux.Advanced.Commands#create--createt). Let's take a look at how `ICommandBuilder` is used in Chefs:

```csharp
public partial record LoginModel(IDispatcher Dispatcher, INavigator Navigator, IAuthenticationService Authentication)
{
	...

	public IState<Credentials> UserCredentials => State<Credentials>.Value(this, () => new Credentials());

    public ICommand Login => Command.Create(b => b.Given(UserCredentials).When(CanLogin).Then(DoLogin));

	private bool CanLogin(Credentials userCredentials)
	{
		return userCredentials is not null &&
			   !string.IsNullOrWhiteSpace(userCredentials.Username) &&
			   !string.IsNullOrWhiteSpace(userCredentials.Password);
	}

	private async ValueTask DoLogin(Credentials userCredentials, CancellationToken ct)
	{
		await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string> { { "Username", userCredentials.Username! }, { "Password", userCredentials.Password! } });
		await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack);
	}
}
```

When the `Login` command is triggered, the __Given__ method will _get_ the current _UserCredentials_ and pass it to the __When__ method, which will check if the command _can_ be executed. The __Then__ method represents the callback to be invoked when the command is executed.

We can see in this example that firstly the Login command is created, then it picks up the credentials of the user _UserCredentials_ to check if they _CanLogin_. If successful, then _DoLogin_ the user.

## Source Code

Chefs app

- [LoginModel ICommand](https://github.com/unoplatform/uno.chefs/blob/655c4a8de96aee98659ec6c648d8e3ab09212ee9/src/Chefs/Presentation/LoginModel.cs#L9)
- [ReviewsModel ICommand](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/ReviewsModel.cs#L46)

## Documentation

- [Uno.Extensions ICommandBuilder Parameters](xref:Uno.Extensions.Mvux.Advanced.Commands#create--createt)