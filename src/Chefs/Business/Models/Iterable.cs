namespace Chefs.Business.Models;

public record Iterable<T>
{
	private Action? _onChanged;
	
	public Iterable(IList<T> items)
	{
		Items = items ?? throw new ArgumentNullException(nameof(items));
		CurrentIndex = 0;
	}
	
	public int Count => Items.Count;
	public IList<T> Items { get; }
	public T CurrentItem => Items[CurrentIndex];
	public int CurrentIndex { get; private set; }
	
	public event Action? OnChanged
	{
		add { _onChanged += value; }
		remove { _onChanged -= value; }
	}
	public bool CanMoveNext => CurrentIndex < Items.Count - 1;
	public bool CanMovePrevious => CurrentIndex > 0;
	public void MoveNext()
	{
		if (!CanMoveNext)
		{
			return;
		}
		CurrentIndex++;
		_onChanged?.Invoke();
	}
	
	public void MovePrevious()
	{
		if (!CanMovePrevious)
		{
			return;
		}
		CurrentIndex--;
		_onChanged?.Invoke();
	}
}
