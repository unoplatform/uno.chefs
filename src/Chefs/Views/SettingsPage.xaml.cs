namespace Chefs.Views;

public sealed partial class SettingsPage : Page
{
	public SettingsPage()
	{
		this.InitializeComponent();
	}

	private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
	{
		var currentText = sender.Text;

		var filteredText = new string(currentText.Where(char.IsDigit).ToArray());

		if (currentText != filteredText)
		{
			sender.Text = filteredText;
			sender.SelectionStart = filteredText.Length;
		}
	}
}
