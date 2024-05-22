namespace Chefs.Business.Models;

public record Iterable<T>
{
	private readonly IList<T> _items;
	
	public Iterable(IList<T> items)
	{
		_items = items ?? throw new ArgumentNullException(nameof(items));
		CurrentIndex = 0;
	}
	
	public int Count => _items.Count;
	public IList<T> Items => _items;
	public bool HasNext => CurrentIndex < _items.Count - 1;
	public bool HasPrevious => CurrentIndex > 0;
	public T CurrentItem => _items[CurrentIndex];
	public int CurrentIndex { get; private set; }
	
	public void Next()
	{
		if (HasNext)
			CurrentIndex++;
	}
	
	public void Previous()
	{
		if (HasPrevious)
			CurrentIndex--;
	}
}
