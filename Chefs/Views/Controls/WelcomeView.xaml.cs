using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
		get { return (string)GetValue(ImageUrlProperty); }
		set { SetValue(ImageUrlProperty, value); }
	}

	public static readonly DependencyProperty ImageUrlProperty =
		DependencyProperty.Register("ImageUrl", typeof(string), typeof(WelcomeView), new PropertyMetadata(string.Empty));



	public string Title
	{
		get { return (string)GetValue(TitleProperty); }
		set { SetValue(TitleProperty, value); }
	}

	public static readonly DependencyProperty TitleProperty =
		DependencyProperty.Register("Title", typeof(string), typeof(WelcomeView), new PropertyMetadata(string.Empty));




	public string Description
	{
		get { return (string)GetValue(DescriptionProperty); }
		set { SetValue(DescriptionProperty, value); }
	}

	public static readonly DependencyProperty DescriptionProperty =
		DependencyProperty.Register("Description", typeof(string), typeof(WelcomeView), new PropertyMetadata(string.Empty));


}
