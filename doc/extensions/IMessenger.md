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

:::code language="csharp" source="../../src/Chefs/App.xaml.cs" range="61":::

### Using IMessenger in Services

Inject the `IMessenger` into the `CookbookService`:

:::code language="csharp" source="../../src/Chefs/Services/Cookbooks/CookbookService.cs" range="6-12":::

When a cookbook is created, updated, or deleted, send a message to notify subscribers:

:::code language="csharp" source="../../src/Chefs/Services/Cookbooks/CookbookService.cs" range="40-44":::

### Reacting to Changes in ViewModels

Subscribe to messages in the ViewModel to react to changes in the service:

:::code language="csharp" source="../../src/Chefs/Presentation/CreateUpdateCookbookModel.cs" range="40-44":::

This pattern will automatically update the provided `IState` property when a message is received.

## Source Code

- [CreateUpdateCookbookModel](https://github.com/unoplatform/uno.chefs/blob/e9a6daf64b4db7eb51b905cf666f2fddcb3986c2/src/Chefs/Presentation/CreateUpdateCookbookModel.cs#L51-L53)
- [CookbookService](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Services/Cookbooks/CookbookService.cs#L49)