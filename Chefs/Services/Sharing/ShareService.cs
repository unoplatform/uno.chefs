using System.Runtime.InteropServices;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using WinRT;
using WinRT.Interop;

namespace Chefs.Services.Sharing;

public class ShareService() : IShareService
{
	private Recipe? _recipe;
	private IImmutableList<Step>? _steps;

#if WINDOWS
	private static readonly Guid _dtm_iid = new Guid("a5caee9b-8708-49d1-8d36-67d25a8da00c");
	static IDataTransferManagerInterop DataTransferManagerInterop => DataTransferManager.As<IDataTransferManagerInterop>();
#endif

	public async Task ShareRecipe(Recipe recipe, IImmutableList<Step> steps, CancellationToken ct)
	{
		_recipe = recipe;
		_steps = steps;

#if WINDOWS
		IntPtr result;
		var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
		result = DataTransferManagerInterop.GetForWindow(hwnd, _dtm_iid);
		DataTransferManager dataTransferManager = MarshalInterface<DataTransferManager>.FromAbi(result);
		dataTransferManager.DataRequested += DataRequested;
		DataTransferManagerInterop.ShowShareUIForWindow(hwnd, null);
#else
		var dataTransferManager = DataTransferManager.GetForCurrentView();
        dataTransferManager.DataRequested += DataRequested;
        DataTransferManager.ShowShareUI();
#endif
	}
	private async void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
	{
		args.Request.Data.Properties.Title = $"Sharing {_recipe?.Name}";
		args.Request.Data.Properties.Description = _recipe?.Details ?? "Chefs Recipe";
		args.Request.Data.SetText(await CreateShareText());
	}

	private async ValueTask<string> CreateShareText()
	{
		var shareTextBuilder = new StringBuilder();

		if (_steps is IImmutableList<Step> steps)
		{
			foreach (var step in steps)
			{
				shareTextBuilder.AppendLine($"Step {step.Number}: {step.Name}")
								.AppendLine($"Ingredients: {string.Join(", ", step.Ingredients ?? ImmutableList<string>.Empty)}")
								.AppendLine($"Description: {step.Description}")
								.AppendLine();
			}
		}

		return shareTextBuilder.ToString();
	}

#if WINDOWS
	[ComImport]
	[Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDataTransferManagerInterop
	{
		IntPtr GetForWindow([In] IntPtr appWindow, [In] ref Guid riid);
		void ShowShareUIForWindow(IntPtr appWindow, ShareUIOptions options);
	}
#endif
}
