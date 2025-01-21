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

```xml
<Button utu:AutoLayout.PrimaryAlignment="Stretch"
        Command="{Binding Submit}"
        Content="{Binding SaveButtonContent}"
        Style="{StaticResource ChefsPrimaryButtonStyle}" />
```

## Source Code

Chefs app

- [CreateUpdateCookbookModel.cs](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Presentation/CreateUpdateCookbookModel.cs#L64-L86)
- [CreateUpdateCookbookModel.xaml](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/CreateUpdateCookbookPage.xaml#L143-L146)

## Documentation

- [MVUX Commands](xref:Uno.Extensions.Mvux.Advanced.Commands#implicit-command-generation)
