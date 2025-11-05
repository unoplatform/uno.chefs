using Uno.UI.Hosting;

namespace Chefs;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseX11()
            .UseLinuxFrameBuffer()
            .UseMacOS()
#if RUNTIME_IDENTIFIER_WIN
            .UseWin32()
#endif  // RUNTIME_IDENTIFIER_WIN
            .Build();

        host.Run();
    }
}
