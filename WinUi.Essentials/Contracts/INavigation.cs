namespace WinUi.Essentials.Contracts;

public interface INavigation
{
    event NavigatedEventHandler Navigated;

    bool GoBack();
    bool NavigateTo(string pageTag);
}