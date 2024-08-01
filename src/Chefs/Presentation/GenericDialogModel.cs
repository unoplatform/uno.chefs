namespace Chefs.Presentation;

public partial record GenericDialogModel
{
	public GenericDialogModel(DialogInfo dialogInfo)
	{
		DialogInfo = dialogInfo;
	}

	public DialogInfo DialogInfo { get; }
}
