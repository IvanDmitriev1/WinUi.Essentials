using WinUi.Essentials.Services;

namespace WinUi.Essentials.Controls;

[TemplatePart(Name = "ThemeComboBox", Type = typeof(ComboBox))]
public sealed class ThemeSelector : Control
{
    public ThemeSelector()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private ComboBox _comboBox = null!;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _comboBox = (ComboBox)GetTemplateChild("ThemeComboBox");
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _comboBox.SelectedIndex = GetIndexFromElementTheme(ThemeSelectorService.Default.CurrentTheme);
        _comboBox.SelectionChanged += ComboBoxOnSelectionChanged;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;

        _comboBox.SelectionChanged -= ComboBoxOnSelectionChanged;
    }

    private static void ComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;

        var theme = GetElementThemeFromIndex(comboBox.SelectedIndex);
        ThemeSelectorService.Default.SetThemeAsync(theme);
    }

    private static int GetIndexFromElementTheme(ElementTheme theme) =>
        theme switch
        {
            ElementTheme.Default => 2,
            ElementTheme.Light => 0,
            ElementTheme.Dark => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
        };

    private static ElementTheme GetElementThemeFromIndex(int index) =>
        index switch
        {
            0 => ElementTheme.Light,
            1 => ElementTheme.Dark,
            2 => ElementTheme.Default,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
}