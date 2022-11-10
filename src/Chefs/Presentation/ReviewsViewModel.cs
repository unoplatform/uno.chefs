using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record ReviewParameter(Guid recipeId, IImmutableList<Review> reviews);

public partial class ReviewsViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly Guid _recipeId;

    public ReviewsViewModel(INavigator navigator, IRecipeService recipeService, ReviewParameter reviewParameter)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        _recipeId = reviewParameter.recipeId;
        Reviews = ListState.Value(this, () => reviewParameter.reviews);
    }

    public IListState<Review> Reviews { get; }

    public IState<string> Comment => State<string>.Empty(this);


    public ICommand AddReview => 
        Command.Create(b => b.Given(Comment).When(CanComment).Then(Review));

    private bool CanComment(string comment) =>
        !string.IsNullOrEmpty(comment);

    private async ValueTask Review(string comment, CancellationToken ct)
    {
        var review = await _recipeService.CreateReview(_recipeId, comment, ct);
        await Reviews.InsertAsync(review, ct);
        await Comment.Set(string.Empty, ct);
    }
}
