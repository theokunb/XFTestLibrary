using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFTestLibrary.Models;

namespace XFTestLibrary.ViewModels
{
    public class BookDetailViewModel : BaseViewModel
    {
        public BookDetailViewModel(Book book)
        {
            isLoaded = false;
            selectedBook = book;
            Authors = new ObservableCollection<Author>();
            Genres = new ObservableCollection<Genre>();

            CommandAppearing = new Command(param => OnAppearing(param));
            CommandAuthorsManipulation = new Command(param => AuthorsChanged(param));
            CommandGenresManipulation = new Command(param => GenresChanged(param));
            CommandSaveAndClose = new Command(param => SaveAndCloseTapped(param));
            CommandAddCover = new Command(param => AddCoverTapped(param));
        }


        private bool isLoaded;
        private readonly Book selectedBook;
        private string title;
        private bool isRequiredFieldAreEmpty => string.IsNullOrEmpty(Title) || IsAnyFieldEmpty();
        private List<BookAndAuthor> bookAndAuthors;
        private List<BookAndGenre> bookAndGenres;
        private ImageSource coverImage;


        public ObservableCollection<Author> Authors { get; set; }
        public ObservableCollection<Genre> Genres { get; set; }
        public ICommand CommandAppearing { get; }
        public ICommand CommandAuthorsManipulation { get; }
        public ICommand CommandGenresManipulation { get; }
        public ICommand CommandSaveAndClose { get; }
        public ICommand CommandAddCover { get; }
        public string Title
        {
            get => title;
            set
            {
                if (title == value)
                    return;
                title = value;
                OnPropertyChnaged();
            }
        }
        public ImageSource CoverImage
        {
            get => coverImage;
            set
            {
                if (coverImage == value)
                    return;
                coverImage = value;
                OnPropertyChnaged();
            }
        }



        private async void OnAppearing(object param)
        {
            if (isLoaded)
                return;
            Title = selectedBook.Title;

            bookAndAuthors = await App.Database.GetAuthorsOfBookAsync(selectedBook.Id);
            foreach (var element in bookAndAuthors)
                Authors.Add(await App.Database.GetAuthorAsync(element.IdAuthor));

            bookAndGenres = await App.Database.GetGenresOfBookAsync(selectedBook.Id);
            foreach (var element in bookAndGenres)
                Genres.Add(await App.Database.GetGenreAsync(element.IdGenre));

            if (selectedBook.IdCover > 0)
            {
                CoverImage = (await App.Database.GetCoverAsync(selectedBook.IdCover)).FullPath;
            }
            isLoaded = true;
        }
        private void GenresChanged(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if(param.ToString() == "+")
            {
                Genres.Add(new Genre());
            }
            else if(param.ToString() == "-")
            {
                if (Genres.Count > 0)
                    Genres.RemoveAt(Genres.Count - 1);
            }
            IsBusy = false;
        }

        private void AuthorsChanged(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (param.ToString() == "+")
            {
                Authors.Add(new Author());
            }
            else if (param.ToString() == "-")
            {
                if (Authors.Count > 0)
                    Authors.RemoveAt(Authors.Count - 1);
            }
            IsBusy = false;
        }
        private async void SaveAndCloseTapped(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if(isRequiredFieldAreEmpty)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("ошибка", "не все поля заполнены", "ок");
                return;
            }

            selectedBook.Title = Title;

            for(int i = 0; i < Authors.Count; i++)
            {
                var author = await App.Database.GetAuthorByName(Authors[i].FullName);
                if (author == null)
                    Authors[i] = await App.Database.InsertAuthorAsync(Authors[i]);
                else
                    Authors[i] = author;
            }

            for (int i = 0; i < Genres.Count; i++)
            {
                var genre = await App.Database.GetGenreByTitleAsync(Genres[i].Title);
                if (genre == null)
                    Genres[i] = await App.Database.InsertGenreAsync(Genres[i]);
                else
                    Genres[i] = genre;
            }
            await App.Database.RemoveBookAndAuthor(selectedBook.Id);
            await App.Database.RemoveBookAndGenre(selectedBook.Id);

            foreach (var author in Authors)
                await App.Database.InsertBookAndAuthorAsync(new BookAndAuthor()
                {
                    IdBook = selectedBook.Id,
                    IdAuthor = author.Id
                });
            foreach (var genre in Genres)
                await App.Database.InsertBookAndGenreAsync(new BookAndGenre()
                {
                    IdBook = selectedBook.Id,
                    IdGenre = genre.Id
                });


            await App.Database.UpdateBookAsync(selectedBook);

            await Application.Current.MainPage.Navigation.PopModalAsync();
            IsBusy = false;
        }
        private bool IsAnyFieldEmpty()
        {
            if (Authors.Count == 0 || Genres.Count == 0)
                return true;
            foreach (var element in Authors)
                if (string.IsNullOrEmpty(element.FullName))
                    return true;
            foreach (var element in Genres)
                if (string.IsNullOrEmpty(element.Title))
                    return true;
            return false;
        }
        private async void AddCoverTapped(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            var pickedImage = await MediaPicker.PickPhotoAsync();
            if(pickedImage == null)
            {
                IsBusy = false;
                return;
            }
            string fullPath = await App.Storage.PushImageAsync(pickedImage);


            Cover cover = new Cover()
            {
                fileName = pickedImage.FileName,
                FullPath = fullPath
            };
            cover = await App.Database.InsertCoverAsync(cover);
            selectedBook.IdCover = cover.Id;


            await App.Database.UpdateBookAsync(selectedBook);

            CoverImage = ImageSource.FromUri(new Uri(fullPath));

            IsBusy = false;
        }
    }
}
