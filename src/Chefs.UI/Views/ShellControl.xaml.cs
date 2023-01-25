
using Uno.Toolkit.UI;

namespace Chefs.Views;

public sealed partial class ShellControl : UserControl, IContentControlProvider
{
    public ExtendedSplashScreen SplashScreen => Splash;
    public ShellControl()
    {
        this.InitializeComponent();
    }

    public ContentControl ContentControl => Splash;
}
