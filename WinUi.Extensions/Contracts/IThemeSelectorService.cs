namespace WinUi.Extensions.Contracts;

public interface IThemeSelectorService
{
    ElementTheme CurrentTheme { get; }

    void Initialize();
    void SetThemeAsync(ElementTheme theme);
}