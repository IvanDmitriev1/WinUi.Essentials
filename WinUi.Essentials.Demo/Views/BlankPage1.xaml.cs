namespace WinUi.Essentials.Demo.Views;

public sealed partial class BlankPage1
{
    public BlankPage1()
    {
        this.InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Navigation.Current!.NavigateTo(nameof(HiddenPage));
    }
}