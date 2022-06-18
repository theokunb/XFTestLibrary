using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTestLibrary.Models;
using XFTestLibrary.Services;
using XFTestLibrary.Views;

namespace XFTestLibrary
{
    public partial class App : Application
    {
        private static IDataBase database;


        public static IDataBase Database
        {
            get
            {
                if (database == null)
                    database = new SqliteDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mydb.db"));
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new MainTabbedPage();
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
