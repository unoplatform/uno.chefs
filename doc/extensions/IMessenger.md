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

[!code-csharp[](../../Chefs/App.xaml.cs#L124)]

### Using IMessenger in Services

Inject the `IMessenger` into the `CookbookService`:

[!code-csharp[](../../Chefs/Services/Cookbooks/CookbookService.cs#L7)]

When a cookbook is created, updated, or deleted, send a message to notify subscribers:

[!code-csharp[](../../Chefs/Services/Cookbooks/CookbookService.cs#L38-L44)]

### Reacting to Changes in ViewModels

Subscribe to messages in the ViewModel to react to changes in the service:

[!code-csharp[](../../Chefs/Presentation/CreateUpdateCookbookModel.cs#L54-L56)]

This pattern will automatically update the provided `IState` property when a message is received.

## Source Code

- [CreateUpdateCookbookModel](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L54-L56)
- [CookbookService](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Services/Cookbooks/CookbookService.cs#L56)