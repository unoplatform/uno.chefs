using Chefs.Business;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Immutable;
using Uno.Extensions.Reactive.Messaging;

namespace Chefs.Presentation;

public record ReviewParameter(Guid recipeId, IImmutableList<Review> reviews);

public partial class ReviewsModel
{
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private readonly Guid _recipeId;
    private readonly IMessenger _messenger;

    public ReviewsModel(INavigator navigator, IRecipeService recipeService, ReviewParameter reviewParameter, IMessenger messenger)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _recipeId = reviewParameter.recipeId;
        _messenger = messenger;

        Reviews = ListState.Value(this, () => reviewParameter.reviews);
    }

    public IListState<Review> Reviews { get; }

    public IState<string> Comment => State<string>.Empty(this);

    public async ValueTask Like(Review review, CancellationToken ct)
    {
        var reviews = await _recipeService.LikeReview(review, ct);
        await Reviews.Update(_ => reviews, ct);
    }

    public async ValueTask Dislike(Review review, CancellationToken ct)
    {
        var reviews = await _recipeService.DislikeReview(review, ct);
        await Reviews.Update(_ => reviews, ct);
    }

    public ICommand AddReview => Command.Create(b => b.Given(Comment).When(CanComment).Then(Review));

    private bool CanComment(string comment) =>
        !string.IsNullOrEmpty(comment);

    public async ValueTask Exit(CancellationToken ct) => await _navigator.NavigateBackWithResultAsync(this);

    private async ValueTask Review(string comment, CancellationToken ct)
    {
        var review = await _recipeService.CreateReview(_recipeId, comment, ct);
        _messenger.Send(new EntityMessage<Review>(EntityChange.Created, review));

        await Reviews.AddAsync(review, ct);
        await Comment.Set(string.Empty, ct);
    }
}
