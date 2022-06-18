using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTestLibrary.Models;
using XFTestLibrary.ViewModels;

namespace XFTestLibrary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookDetailsPage : ContentPage
    {
        public BookDetailsPage(Book book)
        {
            InitializeComponent();
            Title = book.Title;
            BindingContext = new BookDetailViewModel(book);
        }
    }
}