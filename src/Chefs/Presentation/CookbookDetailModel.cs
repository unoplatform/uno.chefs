using CommunityToolkit.Mvvm.Messaging;
namespace Chefs.Presentation;

public partial record CookbookDetailModel
{
	private readonly IMessenger _messenger;
	public CookbookDetailModel(Cookbook cookbook, IMessenger messenger)
	{
		_messenger = messenger; 
	}
	
	public IState<Cookbook> Cookbook => State
		.Value(this, () => new Cookbook())
		.Observe(_messenger, cb => cb.Id);
}
