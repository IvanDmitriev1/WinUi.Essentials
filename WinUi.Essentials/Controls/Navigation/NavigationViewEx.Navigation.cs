using WinUi.Essentials.Contracts;
using WinUi.Essentials.Enums;
using WinUi.Essentials.Extensions;

namespace WinUi.Essentials.Controls.Navigation;

public sealed partial class NavigationViewEx
{
    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (_interceptNavigationFromUser)
            return;

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

    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (_interceptNavigationFromUser)
        {
            SelectedItem = _previousSelectedItem;
        }
    }

    public bool GoBack()
    {
        IsBackEnabled = Frame.CanGoBack;

        if (!Frame.CanGoBack)
            return false;

        Frame.GoBack();
        SetMainComponents(Frame.SourcePageType, Frame.GetContentAsPage()!);

        return true;
    }

    public bool NavigateTo(string pageTag)
    {
        if (!_tagToPageTypeDictionary.TryGetValue(pageTag, out var navigateToType))
            return false;

        if (_currentNavigationViewItemType == navigateToType)
            return false;

        var vmBeforeNavigation = Frame.GetContentAsPage()?.GetViewModel();
        Frame.Navigate(navigateToType, null);

        if (_interceptNavigationFromUser)
        {
            var backStackItem = Frame.BackStack[^1];
            Frame.BackStack.Remove(backStackItem);
        }

        if (vmBeforeNavigation is INavigationAware navigationAware)
            navigationAware.OnNavigatedFrom();

        SetMainComponents(navigateToType, Frame.GetContentAsPage()!);
        return true;
    }

    private void FrameOnNavigated(object sender, NavigationEventArgs e)
    {
        var page = Frame.GetContentAsPage()!;

        if (page.GetViewModel() is INavigationAware navigationAware)
            navigationAware.OnNavigatedTo();

        SetHeader(page, e);
        Navigated?.Invoke(this, e);
    }

    private void SetMainComponents(Type type, NavigationPage page)
    {
        _interceptNavigationFromUser = page.InterceptNavigationFromUser switch
        {
            false when _interceptNavigationFromUser => false,
            true => true,
            _ => _interceptNavigationFromUser
        };

        _currentNavigationViewItemType = type;
        IsBackEnabled = !_interceptNavigationFromUser && Frame.CanGoBack;

        if (_pageTypeToNavigationViewItemsDictionary.TryGetValue(type, out var navigationViewItem))
        {
            SelectedItem = navigationViewItem;
            _previousSelectedItem = navigationViewItem;
        }
    }

    private void SetHeader(Page? page, NavigationEventArgs e)
    {
        if (page is null)
            return;

        var navigationPage = (NavigationPage)page!;

        if (navigationPage.HeaderMode != NavigationViewExHeaderMode.Always)
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
        else if (navigationPage.CustomHeader is { } customHeader)
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

            var item = _pageTypeToNavigationViewItemsDictionary[e.SourcePageType];
            string itemTag;

            if (item is NavigationViewItemEx itemEx)
                itemTag = itemEx.Tag;
            else
                itemTag = (string)item.Tag;

            _breadcrumbBarItems.Add(new BreadcrumbBarItem(headerString, itemTag));
        }
    }

    private void BreadcrumbBarOnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        if (_interceptNavigationFromUser)
            return;

        var breadcrumbBarItem = (BreadcrumbBarItem)args.Item;
        NavigateTo(breadcrumbBarItem.PageTag);
    }
}