This article explains how to implement a centralized [.NET MAUI BusyIndicator](https://www.syncfusion.com/maui-controls/maui-busy-indicator) that allows you to show and hide the indicator from the `BusyIndicatorService` without having to replicate the code in every XAML file. Using this approach, the indicator is dynamically added to the current page's layout, making it easier to manage loading states across your app.

**Create the BusyIndicator Service**

The `BusyIndicatorService` class manages the logic for showing and hiding the indicator.

```
using Syncfusion.Maui.Core;
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
```

**XAML**

In the XAML of each page, bind the `BusyIndicatorService` to the ViewModel, and call the `ShowBusyIndicator()` and `HideBusyIndicator()` methods when necessary (e.g., during page load or async operations).

```
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="BusyIndicator.MainPage"
             xmlns:local="clr-namespace:BusyIndicator">

    <ContentPage.BindingContext>
        <local:BusyIndicatorService x:Name="viewModel"/>
    </ContentPage.BindingContext>
    
    <Grid>
        <Button Text="Mail Login" 
                Clicked="OnNavigate"
                HorizontalOptions="Center" VerticalOptions="Center" />
    </Grid>

</ContentPage>
```

**C#**

Call `ShowBusyIndicator()` and `HideBusyIndicator()` from your ViewModel to control the BusyIndicator state.

```
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNavigate(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (viewModel != null)
        {
            viewModel.ShowBusyIndicator();
            await Task.Delay(1500);
            viewModel.HideBusyIndicator();
        }
    }
}
```

- The BusyIndicator is globally available for all views and can be controlled directly from the ViewModel.

- This approach simplifies showing and hiding the busy indicator throughout the app without needing to duplicate UI logic in each view.

**Output**

![BusyIndicator_SharedView.gif](https://support.syncfusion.com/kb/agent/attachment/article/19049/inline?token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjM2NDMzIiwib3JnaWQiOiIzIiwiaXNzIjoic3VwcG9ydC5zeW5jZnVzaW9uLmNvbSJ9.M_unKMkYBD8k2a3MnBUn9Fp84Y1L0DOBJjPqwnZu7og)
