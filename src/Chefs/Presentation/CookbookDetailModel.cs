namespace Chefs.Presentation;

public partial record CookbookDetailModel
{
	public CookbookDetailModel(Cookbook cookbook, IMessenger messenger)
	{
		Cookbook = State<Cookbook>.Value(this, () => cookbook ?? new Cookbook());

		messenger.Observe(Cookbook, cb => cb.Id);
	}

	public IState<Cookbook> Cookbook { get; }
}
