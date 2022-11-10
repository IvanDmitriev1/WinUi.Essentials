using WinUi.Extensions.Helpers;

namespace WinUi.Extensions.Services;

public sealed class ThemeSelectorService : IThemeSelectorService
{
    public static IThemeSelectorService Default { get; } = new ThemeSelectorService();

    public ThemeSelectorService()
    {
        var value = _localSettingsService.ReadSetting(SettingsKey);
        CurrentTheme = Enum.TryParse<ElementTheme>(value, out var themeValue) ? themeValue : ElementTheme.Default;
    }

    private const string SettingsKey = "AppRequestedTheme";
    private readonly ILocalSettingsService _localSettingsService = LocalSettingsService.Default;

    public ElementTheme CurrentTheme { get; private set; }

    public void Initialize()
    {
        SetThemeAsync(CurrentTheme);
    }

    public void SetThemeAsync(ElementTheme theme)
    {
        if (ApplicationEx.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = theme;
            TitleBarHelper.UpdateTitleBar(theme);
        }

        if (CurrentTheme != theme)
        {
            CurrentTheme = theme;
            _localSettingsService.SaveSetting(SettingsKey, theme.ToString());   
        }
    }
}