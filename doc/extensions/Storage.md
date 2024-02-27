---
uid: Uno.Recipes.Storage
---

# How to persist or access serialized data into your application

## Problem

When working with serialization in applications, managing the storage of serialized data across various platforms can prove challenging. Without a unified solution, developers struggle with inconsistencies in APIs and the lack of seamless integration between serialization and storage functionalities.

## Solution

The **Uno.Extensions** library addresses this problem by providing a cohesive solution that integrates file storage and serialization. The `IStorage` interface encapsulates common file storage operations, and the `StorageExtensions` class extends this functionality by allowing you to deserialize file contents to a specified type using a provided serializer. See the [Serialization](xref:Uno.Recipes.Serialization) recipe for more information on serialization.

### App Startup Configuration for Serialization

```csharp
public class App : Application
{
  // Code omitted for brevity

  protected async override void OnLaunched(LaunchActivatedEventArgs args)
  {
    var builder = this.CreateBuilder(args)
      .Configure(host => host
        // Code omitted for brevity
        
        // Register Json serializers (ISerializer and ISerializer)
        .UseSerialization()

        // Code omitted for brevity
    );
  
  // Code omitted for brevity
  }
}
```

### Consume `IStorage` as dependency in data services

```csharp
public class RecipeEndpoint : IRecipeEndpoint
{
  private readonly IStorage _dataService;
  private readonly ISerializer _serializer;
  private readonly IUserEndpoint _userEndpoint;

  private List<RecipeData>? _recipes;

  public RecipeEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
    => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

  public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => (await Load()).ToImmutableList()
    ?? ImmutableList<RecipeData>.Empty;

  private async ValueTask<IList<RecipeData>> Load()
  {
    if (_recipes == null)
    {
      _recipes = (await _dataService.ReadPackageFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));
    }

    return _recipes ?? new List<RecipeData>();
  }
}
```

### Recipe JSON data (Recipes.json)

```json
[
  {
    "Name": "Avocado Toast",
    "UserId": "bb708644-d5cd-45ad-b565-07a0c4d0b320",
    "Id": "0dc51562-67b7-4de8-91fa-a0a4a538d919",
    "Steps": [
      {
        "Name": "Getting started",
        "CookTime": "00:05:00",
        "Cookware": [ "Fork", "Knife", "Skillet", "Spatula" ],
        "Description": "Melt butter in a skillet over medium-low heat.\n\nCrack eggs into the skillet side by side and cook until eggs are white on the bottom layer and firm enough to flip, 2 to 3 minutes.",
        "Ingredients": [ "Eggs", "Olive oil" ],
        "Number": 1,
        "UrlVideo": ""
      },
      {
        "Name": "Next step",
        "CookTime": "00:04:00",
        "Cookware": [ "Skillet", "Spatula" ],
        "Description": "Flip eggs, trying not to crack the yolk, and cook until the egg reaches desired doneness, 2 to 4 minutes more.",
        "Ingredients": [ "Bread" ],
        "Number": 2,
        "UrlVideo": ""
      },
      {
        "Name": "Final step",
        "CookTime": "00:01:00",
        "Cookware": [ "Fork", "Knife", "Bowl", "Cutting Boards" ],
        "Description": "Meanwhile, toast bread slices to desired doneness, 3 to 4 minutes.",
        "Ingredients": [ "Avocado", "Lemon juice", "Salt and pepper" ],
        "Number": 3,
        "UrlVideo": ""
      }
    ],
    "ImageUrl": "ms-appx:///Chefs/Assets/Recipes/avocado_toast.png",
    "Serves": 1,
    "CookTime": "00:10:00",
    "Difficulty": 1,
    "Ingredients": [
      {
        "UrlIcon": "ms-appx:///Chefs/Assets/Icons/avocado.png",
        "Name": "Avocado",
        "Quantity": "250 gr"
      },
      {
        "UrlIcon": "ms-appx:///Chefs/Assets/Icons/egg.png",
        "Name": "Eggs",
        "Quantity": "2"
      },
      {
        "UrlIcon": "ms-appx:///Chefs/Assets/Icons/baguette_bread.png",
        "Name": "Bread",
        "Quantity": "300 g"
      },
      {
        "UrlIcon": "ms-appx:///Chefs/Assets/Icons/lemon.png",
        "Name": "Lemon juice",
        "Quantity": "80 ml"
      },
      {
        "UrlIcon": "ms-appx:///Chefs/Assets/Icons/olive.png",
        "Name": "Olive oil",
        "Quantity": "50 ml"
      },
      {
        "UrlIcon": "ms-appx:///Chefs/Assets/Icons/salt.png",
        "Name": "Salt and pepper",
        "Quantity": "salt to taste"
      }
    ],
    "Calories": "250 kcal",
    "Reviews": [],
    "Details": "Details",
    "Creator": {
      "Id": "3c896419-e280-40e7-8552-240635566fed",
      "UrlProfileImage": "ms-appx:///Chefs/Assets/Profiles/james_bondi.png",
      "FullName": "James Bondi",
      "Description": "Passionate about food and life",
      "Email": "james.bondi@gmail.com",
      "Followers": 450,
      "Following": 124,
      "Recipes": 0
    },
    "Category": {
      "Id": 1,
      "UrlIcon": "ms-appx:///Chefs/Assets/Icons/pancakes.png",
      "Name": "Breakfast",
      "Color": "#7A67F8"
    },
    "Date": "2022-10-18T00:00:00Z",
    "Save": true
  },

  ...
]
```

### RecipeData model object

```csharp
public class RecipeData
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public List<StepData>? Steps { get; set; }
  public string? ImageUrl { get; set; }
  public string? Name { get; set; }
  public int Serves { get; set; }
  public TimeSpan CookTime { get; set; }
  public Difficulty Difficulty { get; set; }
  public List<IngredientData>? Ingredients { get; set; }
  public string? Calories { get; set; }
  public List<ReviewData>? Reviews { get; set; }
  public string? Details { get; set; }
  public CategoryData? Category { get; set; }
  public DateTime Date { get; set; }
  public bool Save { get; set; }
  public NutritionData? Nutrition { get; set; } = new(30, 101, 30, 110, 300, 75);
}
```

## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/App.cs#L43)
- [Recipe Data Service](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Services/Endpoints/RecipeEndpoint.cs#L5)
- [Recipe Data Model](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Data/Entities/RecipeData.cs)
- [JSON Data Files](https://github.com/unoplatform/uno.chefs/tree/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Data/AppData)

## Documentation

- [Uno.Extensions documentation](xref:Uno.Extensions.Overview)
- [Uno.Extensions Serialization documentation](xref:Uno.Extensions.Serialization.Overview)