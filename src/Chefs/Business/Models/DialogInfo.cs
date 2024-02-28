namespace Chefs.Business.Models;

public partial record DialogInfo
{
	public DialogInfo(string title, string content)
	{
		Title = title;
		Content = content;
	}

	public string Title { get; init; }
	public string Content { get; init; }
}
