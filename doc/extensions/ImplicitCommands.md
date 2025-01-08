---
uid: Uno.Recipes.ImplicitCommands
---

# How to Use Implicit Commands with MVUX

## Problem

Implementing commands requires custom implementations of the ICommand interface and handling `INotifyPropertyChanged` manually, adding boilerplate code and complexity. These challenges can make command handling cumbersome, especially for asynchronous operations.

## Solution

`Uno.Extensions.MVUX` provides a way to use define commands implicitly. By default, a command property will be generated in the ViewModel for each method on the Model that has no return value or is asynchronous (e.g. returns `ValueTask` or `Task`). See [implicit command generation](xref:Uno.Extensions.Mvux.Advanced.Commands#implicit-command-generation) for more information.

### Using Implicit Commands

As an example, in Chefs when a user submits a new cookbook they are running the **Submit()** `ValueTask`. This `ValueTask` implicitly creates a **Submit** command for the model.

```csharp
public partial record CreateUpdateCookbookModel
{
    ...

    public async ValueTask Submit(CancellationToken ct)
    {
        var selectedRecipes = await SelectedRecipes;
        var cookbook = await Cookbook;

        if (selectedRecipes is { Count: > 0 } && cookbook is not null && cookbook.Name.HasValueTrimmed())
        {
            var response = IsCreate
                ? await _cookbookService.Create(cookbook.Name!, selectedRecipes.ToImmutableList(), ct)
                : await _cookbookService.Update(cookbook, selectedRecipes, ct);

            if (IsCreate)
            {
                await _cookbookService.Save(response!, ct);
            }

            await _navigator.NavigateBackWithResultAsync(this, data: response);
        }
        else
        {
            await _navigator.ShowDialog(this, new DialogInfo("Error", "Please write a cookbook name and select one recipe."), ct);
        }
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
