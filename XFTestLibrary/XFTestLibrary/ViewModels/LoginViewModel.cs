using Newtonsoft.Json;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFTestLibrary.Helpers;
using XFTestLibrary.Models;
using XFTestLibrary.Services;
using XFTestLibrary.Views;

namespace XFTestLibrary.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            IsBusy = false;
            hashGenerator = new HashGenerator();


            CommandSignUp = new Command(param => SignUpTapped(param));
            CommandSingIn = new Command(param => SingInTapped(param));
        }


        private string login;
        private string password;
        private readonly IHashGenerator hashGenerator;
        private bool isAnyFieldEmpty => string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password);



        public ICommand CommandSignUp { get; }
        public ICommand CommandSingIn { get; }
        public string Login
        {
            get => login;
            set
            {
                if (login == value)
                    return;
                login = value;
                OnPropertyChnaged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                if (password == value)
                    return;
                password = value;
                OnPropertyChnaged();
            }
        }

        private async void SingInTapped(object param)
        {
            if (IsBusy)
                return;
            if (isAnyFieldEmpty)
            {
                await Application.Current.MainPage.DisplayAlert("ошибка", "не все поля заполнены", "ок");
                return;
            }
            IsBusy = true;

            var hashPassword = hashGenerator.ComputeHash(Password);
            var token = await App.Database.AuthorizeWithLoginPasswordAsync(Login, hashPassword);
            if (!token.IsValidToken)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("ошибка", "доступ запрещен", "ок");
                return;
            }

            var content = JsonConvert.SerializeObject(token);
            Preferences.Set(Strings.Token, content);
            Application.Current.MainPage = new MainTabbedPage(token);

            IsBusy = false;
        }

        private async void SignUpTapped(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new SignUpPage(hashGenerator)));

            IsBusy = false;
        }
    }
}
