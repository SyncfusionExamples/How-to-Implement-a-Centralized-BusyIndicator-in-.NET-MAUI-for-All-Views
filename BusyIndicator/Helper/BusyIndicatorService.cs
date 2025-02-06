using Syncfusion.Maui.Core;
using System.Linq;

namespace BusyIndicator;

public partial class BusyIndicatorService
{
    private SfBusyIndicator? _busyIndicator;
    private Layout? _mainLayout;

    public BusyIndicatorService()
    {
        InitializeBusyIndicator();
    }

    private void InitializeBusyIndicator()
    {
        _busyIndicator = new SfBusyIndicator
        {
            AnimationType = AnimationType.DoubleCircle,
            OverlayFill = Color.FromRgba(211, 211, 211, 155),
            IsVisible = false,
        };
    }

    public void ShowBusyIndicator()
    {
        if (_busyIndicator == null) return;

        var application = Application.Current;
        if (application?.Windows.FirstOrDefault()?.Page is not Shell shell) return;

        var currentPage = shell.CurrentPage;
        if (currentPage is not ContentPage contentPage) return;

        var layout = contentPage.Content as Layout;
        if (layout == null) return;

        // Store reference to current layout
        _mainLayout = layout;

        // Remove from previous layout if exists
        if (_busyIndicator.Parent != null)
        {
            (_busyIndicator.Parent as Layout)?.Children.Remove(_busyIndicator);
        }

        // Add to new layout
        if (!layout.Children.Contains(_busyIndicator))
        {
            layout.Children.Add(_busyIndicator);
        }

        layout.InputTransparent = true;
        _busyIndicator.IsVisible = true;
        _busyIndicator.IsRunning = true;
    }

    public void HideBusyIndicator()
    {
        if (_busyIndicator == null || _mainLayout == null) return;

        if (_busyIndicator.Parent != null)
        {
            (_busyIndicator.Parent as Layout)?.Children.Remove(_busyIndicator);
        }

        _mainLayout.InputTransparent = false;
        _busyIndicator.IsRunning = false;
        _busyIndicator.IsVisible = false;
    }
}