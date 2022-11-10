namespace WinUi.Extensions.Controls;

[TemplatePart(Name = PartDescriptionPresenter, Type = typeof(ContentPresenter))]
public sealed class SettingsGroup : ItemsControl
{
    private const string PartDescriptionPresenter = "DescriptionPresenter";
    private SettingsGroup? _settingsGroup;
    private ContentPresenter _descriptionPresenter = null!;

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(SettingsGroup),
        new PropertyMetadata(string.Empty));


    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(object),
        typeof(SettingsGroup),
        new PropertyMetadata(null, OnDescriptionChanged));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        IsEnabledChanged -= SettingsGroup_IsEnabledChanged;
        _settingsGroup = (SettingsGroup)this;
        _descriptionPresenter = (ContentPresenter)_settingsGroup.GetTemplateChild(PartDescriptionPresenter);
        SetEnabledState();
        IsEnabledChanged += SettingsGroup_IsEnabledChanged;
    }

    private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((SettingsGroup)d).Update();
    }

    private void SettingsGroup_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        SetEnabledState();
    }

    private void SetEnabledState()
    {
        VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
    }

    private void Update()
    {
        if (_settingsGroup == null)
        {
            return;
        }

        _settingsGroup._descriptionPresenter.Visibility =
            _settingsGroup.Description == null ? Visibility.Collapsed : Visibility.Visible;
    }
}
