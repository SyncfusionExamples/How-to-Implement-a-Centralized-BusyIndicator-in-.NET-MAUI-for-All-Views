using System.ComponentModel;
using System.Windows.Input;

namespace BusyIndicator
{
    public class SignUpViewModel : BusyIndicatorService, INotifyPropertyChanged
    {
        private string? _email;
        private string? _password;
        private string? _confirmPassword;
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

        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
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

        public ICommand SignUpCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SignUpViewModel()
        {
            SignUpCommand = new Command(SignUp);
            NavigateToLoginCommand = new Command(NavigateToLoginPage);
        }

        private async void SignUp()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match!";
                HasError = true;
                return;
            }

            var result = VerficationService.Register(Email, Password);
            if (result)
            {
                var currentWindow = Application.Current?.Windows.FirstOrDefault();
                if (currentWindow?.Page is Page currentPage)
                {
                    this.ShowBusyIndicator();
                    await Task.Delay(1500);
                    this.HideBusyIndicator();
                    await currentPage.DisplayAlert("Success", "Account Created!", "OK");
                }
            }
            else
            {
                ErrorMessage = "Email already registered.";
                HasError = true;
            }
        }

        private async void NavigateToLoginPage()
        {
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is Page currentPage)
            {
                await currentPage.Navigation.PushAsync(new LoginPage());
            }
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
