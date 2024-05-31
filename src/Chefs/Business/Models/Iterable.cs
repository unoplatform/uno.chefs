namespace Chefs.Business.Models;

public sealed record Iterable<T>(IImmutableList<T> Items)
{
	public int CurrentIndex { get; init; }
	
	public T CurrentItem => Items[CurrentIndex];
	public int Count => Items.Count;
	public bool CurrentIsLast => CurrentIndex == Items.Count - 1;
	public bool CanMoveNext => CurrentIndex < Items.Count - 1;
	public bool CanMovePrevious => CurrentIndex > 0;
	
	public Iterable<T> MoveNext()
		=> CanMoveNext
			? this with { CurrentIndex = CurrentIndex + 1 }
			: this;
	
	public Iterable<T> MovePrevious()
		=> CanMovePrevious
			? this with { CurrentIndex = CurrentIndex - 1 }
			: this;
}
