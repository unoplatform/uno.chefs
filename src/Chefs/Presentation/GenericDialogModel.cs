namespace Chefs.Presentation;

public partial class GenericDialogModel
{
	public GenericDialogModel(DialogInfo dialogInfo)
	{
		DialogInfo = dialogInfo;
	}

	public DialogInfo DialogInfo { get; }
}
