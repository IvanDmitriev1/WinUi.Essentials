namespace WinUi.Extensions.Controls.Navigation;

public sealed class NavigationViewItemEx : NavigationViewItem
{
    public static readonly DependencyProperty NavigateToTypeProperty = DependencyProperty.Register(
        nameof(NavigateToType),
        typeof(Type),
        typeof(NavigationViewItemEx),
        new PropertyMetadata(null));

    public Type NavigateToType
    {
        get => (Type)GetValue(NavigateToTypeProperty);
        set => SetValue(NavigateToTypeProperty, value);
    }

    public new string Tag { get; set; } = string.Empty;
}