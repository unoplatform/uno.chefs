---
uid: Uno.Recipes.IMessenger
---

# MVUX - How to update your Feeds with IMessenger

## Problem

In modern applications, it's essential to manage and propagate state changes across different parts of the application efficiently.

## Solution

The `IMessenger` interface provided by `Uno.Extensions` allows components to communicate effectively without tight coupling, using messages to signify state changes. This pattern is particularly useful in managing reactive updates across different layers of the application, such as services and view models.

### Setup

Ensure `IMessenger` is registered in your application's services during startup in the `ConfigureServices` method:

```csharp
host.ConfigureServices((context, services) =>
{
    services
        ... // other services
        .AddSingleton<IMessenger, WeakReferenceMessenger>();
})
```

### Using IMessenger in Services

Inject the `IMessenger` into the `CookbookService`:

```csharp
public class CookbookService(ChefsApiClient client, IMessenger messenger, IUserService userService): ICookbookService
{
    ...
}
```

When a cookbook is created, updated, or deleted, send a message to notify subscribers:

```csharp
public async ValueTask Update(Cookbook cookbook, CancellationToken ct)
{
    var cookbookData = cookbook.ToData();

    await client.Api.Cookbook.PutAsync(cookbookData, cancellationToken: ct);
    messenger.Send(new EntityMessage<Cookbook>(EntityChange.Updated, cookbook));
}
```

### Reacting to Changes in ViewModels

Subscribe to messages in the ViewModel to react to changes in the service:

```csharp
public IState<Cookbook> Cookbook => State
    .Value(this, () => _cookbook ?? new Cookbook())
    .Observe(_messenger, cb => cb.Id);
```

This pattern will automatically update the provided `IState` property when a message is received.

## Source Code

- [CreateUpdateCookbookModel](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L54-L56)
- [CookbookService](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Services/Cookbooks/CookbookService.cs#L56)
