using Uno.UI.Runtime.Skia;

namespace Chefs;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var host = SkiaHostBuilder.Create()
            .App(() => new App())
            .UseX11()
            .UseLinuxFrameBuffer()
            .UseMacOS()
#if HAS_SKIA_RENDERER
            .UseWin32()
#else
            .UseWindows()
#endif
            .Build();

        host.Run();
    }
}
