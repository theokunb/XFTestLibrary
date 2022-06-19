using System.Windows.Input;
using Xamarin.Forms;
using XFTestLibrary.Models;
using XFTestLibrary.Services;

namespace XFTestLibrary.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public SignUpViewModel(IHashGenerator hashGenerator)
        {
            IsBusy = false;
            this.hashGenerator = hashGenerator;

            CommandSignUp = new Command(param => SignUpTapped(param));
        }


        private readonly IHashGenerator hashGenerator;
        private string firstName;
        private string lastName;
        private string userName;
        private string password1;
        private string password2;



        public ICommand CommandSignUp { get; }
        public bool AllFieldsFilled => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password1) && !string.IsNullOrEmpty(Password2);
        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName == value)
                    return;
                firstName = value;
                OnPropertyChnaged();
                OnPropertyChnaged(nameof(AllFieldsFilled));
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                if (lastName == value)
                    return;
                lastName = value;
                OnPropertyChnaged();
                OnPropertyChnaged(nameof(AllFieldsFilled));
            }
        }
        public string UserName
        {
            get => userName;
            set
            {
                if (userName == value)
                    return;
                userName = value;
                OnPropertyChnaged();
                OnPropertyChnaged(nameof(AllFieldsFilled));
            }
        }
        public string Password1
        {
            get => password1;
            set
            {
                if (password1 == value)
                    return;
                password1 = value;
                OnPropertyChnaged();
                OnPropertyChnaged(nameof(AllFieldsFilled));
            }
        }
        public string Password2
        {
            get => password2;
            set
            {
                if (password2 == value)
                    return;
                password2 = value;
                OnPropertyChnaged();
                OnPropertyChnaged(nameof(AllFieldsFilled));
            }
        }




        private async void SignUpTapped(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if(Password1 != Password2)
            {
                await Application.Current.MainPage.DisplayAlert("ошибка", "пароли не совпадают", "ок");
                IsBusy = false;
                return;
            }
            var userFromDB = await App.Database.GetUserByUserName(UserName);
            if(userFromDB !=null)
            {
                await Application.Current.MainPage.DisplayAlert("ошибка", "логин уже занят", "ок");
                IsBusy = false;
                return;
            }

            var user = new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Login = UserName,
                PasswordHash = hashGenerator.ComputeHash(Password1)
            };

            await App.Database.InsertUserAsync(user);

            await Application.Current.MainPage.DisplayAlert("успашно", "добро пожаловать!", "yep");

            IsBusy = false;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
