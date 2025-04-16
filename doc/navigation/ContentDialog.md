---
uid: Uno.Recipes.ContentDialog
---

# How to use the ContentDialog control with Navigation Extensions

## Problem

[Navigation Extensions](xref:Uno.Extensions.Navigation.Overview) currently supports [displaying a `MessageDialog`](xref:Uno.Extensions.Navigation.HowToDisplayMessageDialog). However, `MessageDialog` cannot be styled and requires a bit more work to set the text of its actions.

## Solution

### Setting up a Generic Dialog Model

We can first create our own `DialogInfo` object that can hold whatever useful information we want to display:

[!code-csharp[](../../Chefs/Business/Models/DialogInfo.cs#L3-L13)]

We can then create a generic dialog that will have its own DialogInfo:

[!code-csharp[](../../Chefs/Presentation/GenericDialogModel.cs#L3)]

We should then create a GenericDialog.xaml file which will take care of the bindings. We will be able to re-use this `ContentDialog` throughout the app:

[!code-xml[](../../Chefs/Views/Dialogs/GenericDialog.xaml)]

### Using the GenericDialogModel with the Uno Navigation Extension

For the navigation to work, we first have to add a `ViewMap` and `RouteMap` to our `App.xaml.cs` file. You can find more information about registering routes [here](xref:Uno.Extensions.Navigation.HowToNavigateBetweenPages).

```csharp
public class App : Application
{
    ...

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            /* other ViewMaps */,
            new ViewMap<GenericDialog, GenericDialogModel>(Data: new DataMap<DialogInfo>())
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                Nested: new RouteMap[]
                {
                    /* other RouteMaps */,
                    new RouteMap("Dialog", View: views.FindByView<GenericDialog>())
                }
            )
        );
    }
}
```

We add our own `ShowDialog` method to `INavigatorExtensions`:

[code-csharp[](../../Chefs/Presentation/Extensions/INavigatorExtensions.cs#L11-L17)]

We are now ready to show a dialog with custom `DialogInfo` wherever we are using navigation. Here's an example where we show the user an error message in our dialog under a condition:

```csharp
public partial class MyViewModel
{
    private readonly INavigator _navigator;

    public async ValueTask Submit(CancellationToken ct)
    {
        if (...)
        {
            var response = await ...
            await _navigator.NavigateBackWithResultAsync(this, data: response);
        }
        else
        {
            await _navigator.ShowDialog(this, new DialogInfo("Error", "Please write a cookbook name and select one recipe."), ct);
        }
    }
}
```

## Source Code

Chefs app

- [CreateUpdateCookbookModel code](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L85)
- [Navigation extension method](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/Extensions/INavigatorExtensions.cs#L15)
- [App.xaml.cs setup (1)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L185)
- [App.xaml.cs setup (2)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L232)
- [GenericDialogModel](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/GenericDialogModel.cs)
- [GenericDialog.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Dialogs/GenericDialog.xaml)

## Documentation

- [ContentDialog Documentation](xref:Uno.Controls.ContentDialog)
