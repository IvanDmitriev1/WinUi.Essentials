using WinUi.Essentials.Contracts;
using WinUi.Essentials.Enums;
using WinUi.Essentials.Extensions;

namespace WinUi.Essentials.Controls.Navigation;

public sealed partial class NavigationViewEx
{
    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            NavigateTo((string)((NavigationViewItem)SelectedItem).Content);
        }
        else
        {
            var navigationViewItem = (NavigationViewItemEx)SelectedItem;
            NavigateTo(navigationViewItem.Tag);
        }
    }

    public bool GoBack()
    {
        IsBackEnabled = Frame!.CanGoBack;

        if (!Frame.CanGoBack)
            return false;

        Frame.GoBack();
        SetMainComponents(Frame.SourcePageType);

        return true;
    }

    public bool NavigateTo(string pageTag)
    {
        if (!_tagToPageTypeDictionary.TryGetValue(pageTag, out var navigateToType))
            return false;

        if (_currentNavigationViewItemType == navigateToType)
            return false;

        var vmBeforeNavigation = Frame.GetContentAsPage()?.GetViewModel();
        var navigated = Frame.Navigate(navigateToType);

        if (navigated && vmBeforeNavigation is INavigationAware navigationAware)
            navigationAware.OnNavigatedFrom();

        SetMainComponents(navigateToType);
        return navigated;
    }

    private void FrameOnNavigated(object sender, NavigationEventArgs e)
    {
        var page = Frame.GetContentAsPage();

        if (page?.GetViewModel() is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo();
        }

        SetHeader(page, e);

        Navigated?.Invoke(this, e);
    }

    private void SetMainComponents(Type type)
    {
        _currentNavigationViewItemType = type;
        IsBackEnabled = Frame.CanGoBack;

        if (_pageTypeToNavigationViewItemsDictionary.TryGetValue(type, out var navigationViewItem))
        {
            SelectedItem = navigationViewItem;
        }
    }

    private void SetHeader(Page? page, NavigationEventArgs e)
    {
        if (page?.GetHeaderMode() != NavigationViewExHeaderMode.Always)
        {
            Header = null;
            return;
        }

        Header ??= _breadcrumbBar;
        var headerString = string.Empty;

        if (_pageTypeToNavigationViewItemsDictionary.TryGetValue(e.SourcePageType, out var navigationViewItem))
        {
            headerString = (string)navigationViewItem.Content;
        }
        else if (page.GetCustomHeader() is { } customHeader)
        {
            headerString = customHeader;
        }

        if (_pageTypeToTagDictionary.TryGetValue(e.SourcePageType, out var tag))
        {
            _breadcrumbBarItems.Add(new BreadcrumbBarItem(headerString, tag));
        }
        else
        {
            _breadcrumbBarItems.Clear();
            _breadcrumbBarItems.Add(new BreadcrumbBarItem(headerString,
                (string)_pageTypeToNavigationViewItemsDictionary[e.SourcePageType].Tag));
        }
    }

    private void BreadcrumbBarOnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        var breadcrumbBarItem = (BreadcrumbBarItem)args.Item;
        NavigateTo(breadcrumbBarItem.PageTag);
    }
}