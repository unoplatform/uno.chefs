using System;
using System.IO;
using NUnit.Framework;
using Uno.UITest;
using Uno.UITest.Helpers;
using Uno.UITest.Selenium;
using Uno.UITests.Helpers;

namespace Chefs.UITests;

public class TestBase
{
	private IApp _app;
	private DateTime _startTime;
	private readonly string? _screenShotPath = Environment.GetEnvironmentVariable("UNO_UITEST_SCREENSHOT_PATH");

	static TestBase()
	{
		AppInitializer.TestEnvironment.AndroidAppName = Constants.AndroidAppName;
		AppInitializer.TestEnvironment.WebAssemblyDefaultUri = Constants.WebAssemblyDefaultUri;
		AppInitializer.TestEnvironment.iOSAppName = Constants.iOSAppName;
		AppInitializer.TestEnvironment.AndroidAppName = Constants.AndroidAppName;
		AppInitializer.TestEnvironment.iOSDeviceNameOrId = Constants.iOSDeviceNameOrId;
		AppInitializer.TestEnvironment.CurrentPlatform = Constants.CurrentPlatform;

#if DEBUG
		Console.WriteLine("*** WARNING Running Chrome with a head, this will fail when running in CI ***");
		AppInitializer.TestEnvironment.WebAssemblyHeadless = false;
#endif
		AppInitializer.ColdStartApp();
	}

	protected IApp App
	{
		get => _app;
		private set
		{
			_app = value;
			Uno.UITest.Helpers.Queries.Helpers.App = value;
		}
	}

	[SetUp]
	public void SetUpTest()
	{
		_startTime = DateTime.Now;
		App = AppInitializer.AttachToApp();
	}

	[TearDown]
	public void TearDownTest()
	{
		TakeScreenshot("teardown");

		WriteSystemLogs(GetCurrentStepTitle("log"));
	}

	private static string GetCurrentStepTitle(string stepName) 
		=> $"{TestContext.CurrentContext.Test.Name}_{stepName}"
			.Replace(" ", "_")
			.Replace(".", "_")
			.Replace(":", "_")
			.Replace("(", "")
			.Replace(")", "")
			.Replace("\"", "")
			.Replace(",", "_")
			.Replace("__", "_");

	private void WriteSystemLogs(string fileName)
	{
		if (_app != null && AppInitializer.GetLocalPlatform() == Uno.UITest.Helpers.Queries.Platform.Browser)
		{
			var outputPath = string.IsNullOrEmpty(_screenShotPath)
				? Environment.CurrentDirectory
				: _screenShotPath;

			using (var logOutput = new StreamWriter(Path.Combine(outputPath, $"{fileName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss.fff}.txt")))
			{
				foreach (var log in _app.GetSystemLogs())
				{
					logOutput.WriteLine($"{log.Timestamp}/{log.Level}: {log.Message}");
				}
			}
		}
	}

	public FileInfo TakeScreenshot(string stepName)
	{
		var title = GetCurrentStepTitle(stepName);

		var fileInfo = _app.Screenshot(title);

		var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.Name);
		if (fileNameWithoutExt != title && fileInfo.DirectoryName != null)
		{
			var outputPath = string.IsNullOrEmpty(_screenShotPath)
					? fileInfo.DirectoryName
					: _screenShotPath;

			var destFileName = Path
				.Combine(outputPath, title + Path.GetExtension(fileInfo.Name));

			if (File.Exists(destFileName))
			{
				File.Delete(destFileName);
			}

			File.Move(fileInfo.FullName, destFileName);

			TestContext.AddTestAttachment(destFileName, stepName);

			fileInfo = new FileInfo(destFileName);
		}
		else
		{
			TestContext.AddTestAttachment(fileInfo.FullName, stepName);
		}

		return fileInfo;
	}

}
