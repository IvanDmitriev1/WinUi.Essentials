namespace WinUi.Essentials.Controls.Navigation;

internal sealed record BreadcrumbBarItem(string Content, string PageTag)
{
    public override string ToString() => Content;
};