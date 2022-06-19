using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTestLibrary.Helpers;
using XFTestLibrary.Models;

namespace XFTestLibrary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage(AuthentificationToken token)
        {
            InitializeComponent();
            this.token = token;
            isLoaded = false;
        }

        private bool isLoaded;
        private AuthentificationToken token;

        protected override async void OnAppearing()
        {
            if (isLoaded)
                return;
            Children.Add(new BooksPage());
            Children.Add(new EditBooksPage());

            token.User.LastIn = DateTime.Now;
            await App.Database.UpdateUserAsync(token.User);
            Preferences.Set(Strings.Token, JsonConvert.SerializeObject(token));

            isLoaded = true;
        }
    }
}