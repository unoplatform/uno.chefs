namespace Chefs.Presentation;

public partial record WelcomeModel(INavigator Navigator)
{
	public IState<IntIterator> Pages => State<IntIterator>.Value(this, () => new IntIterator(Enumerable.Range(0, 3).ToImmutableList()));
}
