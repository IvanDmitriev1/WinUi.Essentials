namespace WinUi.Essentials.Contracts;

public interface INavigationAware
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
}