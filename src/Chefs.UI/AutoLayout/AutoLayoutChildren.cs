using System.Collections.Generic;

#if IS_WINUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Chefs.UI
{
	public partial class AutoLayoutChildren : List<FrameworkElement>
	{
		public AutoLayoutChildren()
			: base(4)
		{
		}
	}
}
