namespace BusyIndicator
{
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

}
