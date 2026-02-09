namespace Chefs.Business.Models;

public partial record DialogInfo
{
	[System.Diagnostics.CodeAnalysis.DynamicDependency(nameof(Title))]
	[System.Diagnostics.CodeAnalysis.DynamicDependency(nameof(Content))]
	public DialogInfo(string title, string content)
	{
		Title = title;
		Content = content;
	}

	public string Title { get; init; }
	public string Content { get; init; }
}
