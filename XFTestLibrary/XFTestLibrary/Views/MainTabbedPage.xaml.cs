using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFTestLibrary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            isLoaded = false;
        }

        private bool isLoaded;


        protected override void OnAppearing()
        {
            if (isLoaded)
                return;
            Children.Add(new BooksPage());
            Children.Add(new EditBooksPage());

            isLoaded = true;
        }
    }
}