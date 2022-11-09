namespace WinUi.Extensions.Contracts;

public interface INavigationAware
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
}