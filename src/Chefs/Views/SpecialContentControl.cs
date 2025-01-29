using System.Diagnostics;

namespace Chefs.Views;

[DebuggerDisplay("SpecialContentControl #{InstanceCount}")]
public class SpecialContentControl : ContentControl
{
	private static int _instanceCount;

	public int InstanceCount { get; } = _instanceCount++;


	public SpecialContentControl()
	{
	}

	protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
	{
		base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
	}
}
