// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Chefs.Views.Controls;

public sealed partial class WelcomeView : UserControl
{
	public WelcomeView()
	{
		this.InitializeComponent();
	}

	public string ImageUrl
	{
		get => (string)GetValue(ImageUrlProperty); set => SetValue(ImageUrlProperty, value);
	}

	public static readonly DependencyProperty ImageUrlProperty =
		DependencyProperty.Register("ImageUrl", typeof(string), typeof(WelcomeView), new PropertyMetadata(string.Empty));

	public string Title
	{
		get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value);
	}

	public static readonly DependencyProperty TitleProperty =
		DependencyProperty.Register("Title", typeof(string), typeof(WelcomeView), new PropertyMetadata(string.Empty));

	public string Description
	{
		get => (string)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value);
	}

	public static readonly DependencyProperty DescriptionProperty =
		DependencyProperty.Register("Description", typeof(string), typeof(WelcomeView), new PropertyMetadata(string.Empty));
}
