using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTestLibrary.Services;
using XFTestLibrary.ViewModels;

namespace XFTestLibrary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage(IHashGenerator hashGenerator)
        {
            InitializeComponent();
            BindingContext = new SignUpViewModel(hashGenerator);
        }
    }
}