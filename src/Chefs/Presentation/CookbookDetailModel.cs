namespace Chefs.Presentation;

public partial record CookbookDetailModel
{
	private readonly IMessenger _messenger;

	private readonly Cookbook _cookbook;
	public CookbookDetailModel(Cookbook cookbook, IMessenger messenger)
	{
		_messenger = messenger;
		_cookbook = cookbook ?? new Cookbook();
	}
	
	public IState<Cookbook> Cookbook => State
		.Value(this, () => _cookbook ?? new Cookbook())
		.Observe(_messenger, cb => cb.Id);
}
