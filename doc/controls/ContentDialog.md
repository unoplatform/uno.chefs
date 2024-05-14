---
uid: Uno.Recipes.ContentDialog
---

# How to use the ContentDialog control with NavigationExtensions
 
## Problem
 
[NavigationExtensions](xref:Uno.Extensions.Navigation.Overview) currently supports [displaying a `MessageDialog`](xref:Uno.Extensions.Navigation.HowToDisplayMessageDialog). However, `MessageDialog` cannot be styled and requires a bit more work to set the text of its actions.

## Solution

The [`ContentDialog`]((xref:Uno.Controls.ContentDialog)) control has Material styles available. This means that you can customize the usual properties and keep your dialog in line with the design of your app. The content of the dialog is also easier to set, whether it's in Xaml or C#.

### Simple ContentDialog

```xml
<ContentDialog Title="Food question of the day"
               Content="Does pineapple belong on pizza?"
               PrimaryButtonText="Yes"
               SecondaryButtonText="No"
			   CloseButtonText="Close"
               Background="{ThemeResource SurfaceBrush}"
			   Style="{StaticResource MaterialContentDialogStyle}" />
```

```csharp
private void ContentDialog_PrimaryButtonClick(object sender, ContentDialogButtonClickEventArgs args) {
    // primary button logic
}

private void ContentDialog_SecondaryButtonClick(object sender, ContentDialogButtonClickEventArgs args) {
    // secondary button logic
}
```

### With Navigation

#### Setting up a Generic Dialog Model

We can first create our own `DialogInfo` object that can hold whatever useful information we want to display:

```csharp
public partial record DialogInfo
{
	public DialogInfo(string title, string content)
	{
		Title = title;
		Content = content;
	}

	public string Title { get; init; }
	public string Content { get; init; }
}
```

We can then create a generic dialog that will have its own DialogInfo: 

```csharp
public partial class GenericDialogModel
{
	public GenericDialogModel(DialogInfo dialogInfo)
	{
		DialogInfo = dialogInfo;
	}

	public DialogInfo DialogInfo { get; }
}
```

We should then create a GenericDialog.xaml file which will take care of the bindings. We will be able to re-use this `ContentDialog` throughout the app:

```xml
﻿<ContentDialog Title="{Binding DialogInfo.Title}"
			   Background="{ThemeResource SurfaceBrush}"
			   CloseButtonText="Close"
			   Content="{Binding DialogInfo.Content}"
			   Style="{StaticResource MaterialContentDialogStyle}" />
```

#### Using the GenericDialogModel with the Uno Navigation Extension

For the navigation to work, we first have to add a `ViewMap` and `RouteMap` to our `App.cs` file. You can find more information about registering routes [here](xref:Uno.Extensions.Navigation.HowToNavigateBetweenPages).

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

```csharp
public static class INavigatorExtensions
{
    public static Task<NavigationResponse?> ShowDialog(this INavigator navigator, object sender, DialogInfo dialogInfo, CancellationToken ct)
	{
		return navigator.NavigateDataAsync(sender, new DialogInfo(dialogInfo.Title, dialogInfo.Content), cancellation: ct);
	}
}
```

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

- [CreateUpdateCookbookModel code](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/CreateUpdateCookbookModel.cs#L96)
- [Navigation extension method](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/Extensions/INavigatorExtensions.cs#L36-L39)
- [App.cs setup (1)](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/App.cs#L113)
- [App.cs setup (2)](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/App.cs#L177)
- [GenericDialogModel](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/GenericDialogModel.cs)
- [GenericDialog.xaml](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Views/Dialogs/GenericDialog.xaml)

## Documentation

- [ContentDialog Documentation](xref:Uno.Controls.ContentDialog)