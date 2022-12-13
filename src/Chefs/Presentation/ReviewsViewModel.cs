using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record ReviewParameter(Guid recipeId, IImmutableList<Review> reviews);

public partial class ReviewsViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly IRecipeService _recipeService;
    private readonly Guid _recipeId;

    public ReviewsViewModel(IRecipeService recipeService, ReviewParameter reviewParameter)
    {
        _recipeService = recipeService;

        _recipeId = reviewParameter.recipeId;
        Reviews = ListState.Value(this, () => reviewParameter.reviews);
    }

    public IListState<Review> Reviews { get; }

    public IState<string> Comment => State<string>.Empty(this);

    // DR_REV: Alignment: When using method body, we keep properties on a single line while method are on 2 lines
    public ICommand AddReview => Command.Create(b => b.Given(Comment).When(CanComment).Then(Review));

    private bool CanComment(string comment) =>
        !string.IsNullOrEmpty(comment);

    private async ValueTask Review(string comment, CancellationToken ct)
    {
        var review = await _recipeService.CreateReview(_recipeId, comment, ct);
        await Reviews.InsertAsync(review, ct);
        await Comment.Set(string.Empty, ct);
    }
}
