namespace WinUi.Essentials.Demo.Views;

public sealed partial class HiddenPage
{
    public HiddenPage()
    {
        this.InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Navigation.Current!.NavigateTo(nameof(BlankPage2));
    }
}