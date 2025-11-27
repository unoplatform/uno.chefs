---
uid: Uno.Recipes.StorageFile
---

# Loading App Data

## Problem

You need to load app data files on all platforms. Traditional file access methods (`System.IO.File.Read*`, `EmbeddedResource`) do not work for Content files on WASM.

## Solution

Use the `Windows.Storage.StorageFile` API to read files from your app package. This works the same way on all Uno Platform targets, including WASM.

- Move your data files to be included as `Content` in your project's output directory, and use `StorageFile.GetFileFromApplicationUriAsync` to load them by path. To ensure that you will always have the latest changes included, set the appropriate Property: `CopyToOutputDirectory="PreserveNewest"`.

    ```xml
    <ItemGroup>
        <Content Include="AppData\*.json" CopyToOutputDirectory="PreserveNewest" />
    </ItemGroup>
    ```

    - If you want to not just have these files in your app's output directory with latest changes, then also have your IDE showing them in your **Solution Explorer** and enable you editing them easily as if those would be directly nested in your specific project, you may want to also set another property on the xml element we just added: `LinkBase="AppData"` (or any other Name you want it to show to you as folder name).

        So this will make the full Content Element look like this:

        ```xml
        <ItemGroup>
         <Content Include="AppData\*.json" CopyToOutputDirectory="PreserveNewest" LinkBase="AppData" />
        </ItemGroup>
        ```

        And in your Solution Explorer, this will show up like with the below shown grey text (VS Code) or with a Link Arrow like you may know it from .ink files on your Desktop (Visual Studio 2022).

        ```markdown
        root
        |-AppData
        | |-yourDataContainingFile.json
        |-ClientApp/
        | |-ClientApp.csproj
        | |-AppData/ *(Linked File)*
        |   |-yourDataContainingFile.json *(Linked File)*
        |-ClientApp.Api/
        | |-ClientApp.Api.csproj
        | |-AppData/ *(Linked File)*
        |   |-yourDataContainingFile.json *(Linked File)*
        |-ClientApp.IntegrationTests/...
        |-ClientApp.UITests/...
        |-ClientApp.UnitTests/...
        ```

        > [!TIP]
        > Another advantage of setting the LinkBase Property like shown above is, that you can do this on every project in your Solution you like to, without being required to:
        >
        > - duplicate the Folder or Files
        > - if you edit a file in there, its state will always be synced
        > [!TIP]
        > If you may not want to have those files in your output directory of the Project, but showing up in your Solution Explorer, you can set the `CopyToOutputDirectory` property shown before, to **Never**.

- Loading a JSON File Using StorageFile

    ```csharp
    public abstract class BaseMockEndpoint(ISerializer serializer, ILogger<BaseMockEndpoint> _logger)
    {
        protected async Task<T?> LoadData<T>(string fileName)
        {
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///AppData/{fileName}"));
                var json = await FileIO.ReadTextAsync(file);
                return serializer.FromString<T>(json);
            }
                catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load {FileName}", fileName);
                return default;
            }
        }
    }
    ```

- Using `LoadData` in `MockRecipeEndpoints`

    ```csharp
    public class MockRecipeEndpoints(string basePath, ISerializer serializer, ILogger<BaseMockEndpoint> logger) : BaseMockEndpoint(serializer, logger)
    {
        public async Task<string> HandleRecipesRequest(HttpRequestMessage request)
        {
            var savedList = await LoadData<List<Guid>>("SavedRecipes.json") ?? [];
            var allRecipes = await LoadData<List<RecipeData>>("Recipes.json") ?? [];
            ...
        }
    }
    ```

## Source Code

- [BaseMockEndpoint](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/Chefs/Services/MockEndpoints/BaseMockEndpoint.cs)
- [MockRecipeEndpoints](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/Chefs/Services/MockEndpoints/MockRecipeEndpoints.cs#L8)
- [Recipes.json](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/AppData/Recipes.json)

## Documentation

- [File Management documentation](xref:Uno.Features.FileManagement)
