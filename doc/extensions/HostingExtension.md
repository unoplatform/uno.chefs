---
uid: Uno.Recipes.Hosting
---

# Building a Hosted Application

Using `Uno.Extensions.Hosting` enables registration of services via dependency injection.

In "`OnLaunched(LaunchActivatedEventArgs args)`", use `CreateBuilder()` and `.Configure()` to configure the host.

:::code language="csharp" source="../../Chefs/App.xaml.cs" range="19-24":::

## Configure the Host

The `IApplicationBuilder` has many extension methods to configure the host. Here are some of the most common ones:

:::code language="csharp" source="../../Chefs/App.xaml.cs" range="62-75":::


## Source Code

Chefs app

- [App.xaml.cs](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/App.cs#L13)

## Documentation

- [Hosting Documentation](xref:Uno.Extensions.Hosting.HowToHostingSetup)