using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTestLibrary.Helpers;
using XFTestLibrary.Models;
using XFTestLibrary.Services;
using XFTestLibrary.Views;

namespace XFTestLibrary
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var savedToken = Preferences.Get(Strings.Token, string.Empty);
            var token = JsonConvert.DeserializeObject<AuthentificationToken>(savedToken);
            if (token == null || !token.IsValidToken)
                MainPage = new LoginPage();
            else
                MainPage = new MainTabbedPage(token);
        }


        private static IDataBase database;
        private static IStorage storage;


        public static IDataBase Database
        {
            get
            {
                if (database == null)
                    database = new SqliteDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mydb.db"));
                return database;
            }
        }
        public static IStorage Storage
        {
            get
            {
                if (storage == null)
                    storage = new FireBaseStorage("xftestlibrary.appspot.com");
                return storage;
            }
        }


        

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
