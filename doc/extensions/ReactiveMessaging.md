---
uid: Uno.Recipes.ReactiveMessaging
---

# How to notify state changes across the app

## Problem

Ensuring the model is aware of state changes initiated by the service while keeping the service and the model decoupled.

## Solution

[Uno Extensions Reactive Messaging](xref:Uno.Extensions.Mvux.Advanced.Messaging) along with the [Community Toolkit Messenger](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/messenger) allow us to use a messaging system where the service can send entity change messages to a central messenger, which then publishes these messages to subscribed models, letting them to update their feeds accordingly. Let's see how reactive messaging is used in Chefs:

### In the Model

Each `RecipeDetailsModel` has a ListState of `Reviews` that the user can like or dislike. When the user likes/dislikes a recipe, we want the corresponding count to be updated. The Reactive `Observe()` method subscribes the messenger to changes in Reviews, where each review is uniquely identified by its Id.

```csharp
public partial class RecipeDetailsModel
{
    private readonly IMessenger _messenger;

    public RecipeDetailsModel(..., IRecipeService recipeService, IMessenger messenger)
    {
        _recipeService = recipeService;
        _messenger = messenger;
    }

    public IListState<Review> Reviews => ListState
        .Async(this, async ct => await _recipeService.GetReviews(Recipe.Id, ct))
        .Observe(_messenger, r => r.Id);

    public async ValueTask Like(Review review, CancellationToken ct) =>
        await _recipeService.LikeReview(review, ct);

    public async ValueTask Dislike(Review review, CancellationToken ct) =>
        await _recipeService.DislikeReview(review, ct);
}
```

### In the Service

If we send an `EntityMessage` of `EntityChange.Updated`, the subscribed messenger in the model will be notified that a change has occured in Reviews and will update the modified review in its place.

```csharp
public class RecipeService : IRecipeService
{
    public async ValueTask LikeReview(Review review, CancellationToken ct)
    {
        var updatedReview = new Review(await _recipeEndpoint.LikeReview(review.ToData(), ct));
        _messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
    }

    public async ValueTask DislikeReview(Review review, CancellationToken ct)
    {
        var updatedReview = new Review(await _recipeEndpoint.DislikeReview(review.ToData(), ct));
        _messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
    }
}
```

### `EntityChange.Created` and `EntityChange.Deleted`

What if instead of updating an entity in a list we want to add or delete one? MVUX lists and feeds can handle this too, we can send `EntityChange.Created` to add an entity to the list, or `EntityChange.Deleted` to remove an entity.

## Source Code

- [RecipeDetailsModel](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Presentation/RecipeDetailsModel.cs)
- [RecipeService LikeReview and DislikeReview](https://github.com/unoplatform/uno.chefs/blob/9541aa5e0fbbc1c1598dfce4153a9a7fc4e95ccd/src/Chefs/Services/Recipes/RecipeService.cs#L141-L151)

## Documentation

- [Uno Extensions Reactive Messaging](xref:Uno.Extensions.Mvux.Advanced.Messaging)
