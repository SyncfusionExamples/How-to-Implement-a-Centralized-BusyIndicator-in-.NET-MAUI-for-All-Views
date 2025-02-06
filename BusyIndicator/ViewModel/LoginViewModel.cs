using Syncfusion.Maui.Core.Carousel;
using System.ComponentModel;
using System.Windows.Input;

namespace BusyIndicator
{
    public class LoginViewModel : BusyIndicatorService, INotifyPropertyChanged
    {
        private string? _email;
        private string? _password;
        private string? _errorMessage;
        private bool _hasError;

        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string? Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        public bool HasError
        {
            get => _hasError;
            set { _hasError = value; OnPropertyChanged(nameof(HasError)); }
        }

        public ICommand? LoginCommand { get; set; }
        public ICommand? NavigateToSignUpCommand { get; set; }
        public ICommand? BackNavigationCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public LoginViewModel()
        {
            LoginCommand = new Command(Login);
            NavigateToSignUpCommand = new Command(NavigateToSignUpPage);
            BackNavigationCommand = new Command(BackNavigation);
        }

        private async void Login()
        {
            var result = VerficationService.Login(Email, Password);
            if (result)
            {
                this.ShowBusyIndicator();
                await Task.Delay(1500);
                this.HideBusyIndicator();
                var currentWindow = Application.Current?.Windows.FirstOrDefault();
                if (currentWindow?.Page is Page currentPage)
                {
                    await currentPage.DisplayAlert("Success", "Logged in!", "OK");
                }
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                HasError = true;
            }
        }
        private async void NavigateToSignUpPage()
        {
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is Page currentPage)
            {
                await currentPage.Navigation.PushAsync(new SignUpPage());
            }
        }
        private async void BackNavigation()
        {
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is Page currentPage)
            {
                await currentPage.Navigation.PushAsync(new MainPage());
            }
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
