using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using XFTestLibrary.Models;
using XFTestLibrary.Views;

namespace XFTestLibrary.ViewModels
{
    public class BooksViewModel : BaseViewModel
    {
        public BooksViewModel()
        {
            isBusy = false;
            Books = new ObservableCollection<BookWithImage>();
            Filters = new ObservableCollection<Filter>();
            filter = new FilterSource();
            timer = new Timer();
            timer.Elapsed += (sender, e) =>
            {
                IsRefreshingView = true;
            };
            timer.Interval = 1000;
            timer.AutoReset = false;


            CommandAppearing = new Command(param => OnAppearing(param));
            CommandRefreshView = new Command(param => OnRefreshView(param));
            CommandBookTapped = new Command(param => OpenBookDetailPage(param));
        }


        private bool isRefreshingView;
        private readonly FilterSource filter;
        private Filter selectedFilter;
        private string searchPattern;
        private Timer timer;


        public ObservableCollection<Filter> Filters { get; set; }
        public ObservableCollection<BookWithImage> Books { get; set; }
        public ICommand CommandAppearing { get; }
        public ICommand CommandRefreshView { get; }
        public ICommand CommandBookTapped { get; }
        public bool IsRefreshingView
        {
            get => isRefreshingView;
            set
            {
                if (isRefreshingView == value)
                    return;
                isRefreshingView = value;
                OnPropertyChnaged();
            }
        }
        public Filter SelectedFilter
        {
            get => selectedFilter;
            set
            {
                if (selectedFilter == value)
                    return;
                selectedFilter = value;
                OnPropertyChnaged();
            }
        }
        public string SearchPattern
        {
            get => searchPattern;
            set
            {
                if (searchPattern == value)
                    return;
                searchPattern = value;
                if (timer.Enabled)
                {
                    timer.Stop();
                    timer.Start();
                }
                else
                    timer.Start();
                OnPropertyChnaged();
            }
        }


        private void OnAppearing(object param)
        {
            if(Filters.Count == 0)
            {
                foreach (var element in filter)
                    Filters.Add(element);
                SelectedFilter = Filters.First();
            }
            IsRefreshingView = true;
        }
        private async void OnRefreshView(object param)
        {
            Books.Clear();

            var books = await App.Database.GetBooksByFilterAsync(SelectedFilter, SearchPattern);
            foreach (var element in books)
            {
                var coverFullPath = (await App.Database.GetCoverAsync(element.IdCover)).FullPath;
                Books.Add(new BookWithImage(element, coverFullPath));
            }

            

            IsRefreshingView = false;
        }
        private async void OpenBookDetailPage(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            Book selectedBook = (Book)param;
            if (selectedBook == null)
            {
                IsBusy = false;
                return;
            }
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new BookDetailsPage(selectedBook)));
            IsBusy = false;
        }
    }
}
