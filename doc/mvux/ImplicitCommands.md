---
uid: Uno.Recipes.ImplicitCommands
---

# How to Use Implicit Commands with MVUX

## Problem

Implementing commands requires custom implementations of the ICommand interface as well as handling `INotifyPropertyChanged` and other events manually which adds boilerplate code and complexity. These challenges can make command handling cumbersome, especially for asynchronous operations.

## Solution

`Uno.Extensions.MVUX` provides a way to use define commands implicitly. By default, a command property will be generated in the ViewModel for each method on the Model that has no return value or is asynchronous (e.g. returns `ValueTask` or `Task`). See [implicit command generation](xref:Uno.Extensions.Mvux.Advanced.Commands#implicit-command-generation) for more information.

### Using Implicit Commands

As an example, in Chefs when a user submits a new cookbook they are invoking the `Submit` ICommand on its `DataContext`. This `ICommand` is implicitly created by MVUX from the `public async ValueTask Submit()` method defined in the `CreateUpdateCookbookModel`.

```csharp
public partial record CreateUpdateCookbookModel
{
    ...

    public async ValueTask Submit(CancellationToken ct)
    {
        ...
    }
}
```

In the XAML, we simply bind the submit button's `Command` property to the **Submit** command of the model.

[!code-xml[](../../Chefs/Views/CreateUpdateCookbookPage.xaml#L143-L146)]

## Source Code

Chefs app

- [CreateUpdateCookbookModel.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L65-L87)
- [CreateUpdateCookbookModel.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/CreateUpdateCookbookPage.xaml#L143-L146)

## Documentation

- [MVUX Commands](xref:Uno.Extensions.Mvux.Advanced.Commands#implicit-command-generation)
