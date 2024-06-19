namespace Chefs.Business.Models;

public record Iterator<T>(IImmutableList<T> Items)
{
	public int CurrentIndex { get; init; } = 0;
	public T CurrentItem => Items[CurrentIndex];
	public int Count => Items.Count;
	public bool CanMoveNext => CurrentIndex < Items.Count - 1;
	public bool CanMovePrevious => CurrentIndex > 0;

	public Iterator<T> MoveNext()
		=> CanMoveNext
			? this with { CurrentIndex = CurrentIndex + 1 }
			: this;

	public Iterator<T> MovePrevious()
		=> CanMovePrevious
			? this with { CurrentIndex = CurrentIndex - 1 }
			: this;
}
