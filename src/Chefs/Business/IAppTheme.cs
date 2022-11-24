namespace Chefs.Business;

public interface IAppTheme
{
    bool IsDark { get; }

    Task SetThemeAsync(bool darkMode);
}
