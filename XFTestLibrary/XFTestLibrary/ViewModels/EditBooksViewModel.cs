using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFTestLibrary.Models;

namespace XFTestLibrary.ViewModels
{
    public class EditBooksViewModel : BaseViewModel
    {
        public EditBooksViewModel()
        {
            IsBusy = false;
            isLoaded = false;
            map = new LibraryMap();
            Authors = new ObservableCollection<Author>();
            Genres = new ObservableCollection<Genre>();
            

            CommandAppearing = new Command(param => OnAppearing(param));
            CommandAddBook = new Command(param => AddBookClicked(param));
            CommandAddAuthor = new Command(param => AddAuthorClicked(param));
            CommandRemoveAuthor = new Command(param => RemoveAuthorCLicked(param));
            CommandGenreManipulation = new Command(param => OnGenreChanged(param));
            CommandAddCover = new Command(param => AddCoverClicked(param));
        }


        private Cover cover;
        private ImageSource imageCover;
        private bool isLoaded;
        private readonly LibraryMap map;
        private string bookTitle;
        private bool isRequiredFieldAreEmpty => string.IsNullOrEmpty(BookTitle) || IsAnyFieldEmpty();



        public ObservableCollection<Genre> Genres { get; set; }
        public ObservableCollection<Author> Authors { get; set; }
        public ICommand CommandAppearing { get; }
        public ICommand CommandAddBook { get; }
        public ICommand CommandAddAuthor { get; }
        public ICommand CommandRemoveAuthor { get; }
        public ICommand CommandGenreManipulation { get; }
        public ICommand CommandAddCover { get; }
        public string BookTitle
        {
            get => bookTitle;
            set
            {
                if (bookTitle == value)
                    return;
                bookTitle = value;
                OnPropertyChnaged();
            }
        }
        public ImageSource ImageCover
        {
            get => imageCover;
            set
            {
                if (imageCover == value)
                    return;
                imageCover = value;
                OnPropertyChnaged();
            }
        }



        private void OnAppearing(object param)
        {
            if (isLoaded)
                return;
            isLoaded = true;
        }
        private async void AddBookClicked(object param)
        {
            if (IsBusy)
                return;
            if (isRequiredFieldAreEmpty)
                return;
            IsBusy = true;

            var freePlace = await map.GetFreePlace();
            if(freePlace == null)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("warning", "no places", "ok");
                return;
            }

            for(int i = 0; i < Authors.Count; i++)
            {
                var author = await App.Database.GetAuthorByName(Authors[i].FullName);
                if (author == null)
                    Authors[i] = await App.Database.InsertAuthorAsync(Authors[i]);
                else
                    Authors[i] = author;
            }

            for(int i = 0; i < Genres.Count; i++)
            {
                var genre = await App.Database.GetGenreByTitleAsync(Genres[i].Title);
                if (genre == null)
                    Genres[i] = await App.Database.InsertGenreAsync(Genres[i]);
                else
                    Genres[i] = genre;
            }


            var book = await App.Database.InsertBookAsync(new Book()
            {
                Title = bookTitle
            });

            if(cover != null)
            {
                cover = await App.Database.InsertCoverAsync(cover);
                book.IdCover = cover.Id;
            }


            foreach (var author in Authors)
                await App.Database.InsertBookAndAuthorAsync(new BookAndAuthor()
                {
                    IdBook = book.Id,
                    IdAuthor = author.Id
                });
            foreach(var genre in Genres)
                await App.Database.InsertBookAndGenreAsync(new BookAndGenre()
                {
                    IdBook = book.Id,
                    IdGenre = genre.Id
                });

            freePlace = await App.Database.InsertBookPlace(freePlace);

            book.IdPlace = freePlace.Id;

            await App.Database.UpdateBookAsync(book);

            ClearAllFields();
            IsBusy = false;
        }

        private void ClearAllFields()
        {
            BookTitle = string.Empty;
            ImageCover = null;
            Authors.Clear();
            Genres.Clear();
        }
        private void RemoveAuthorCLicked(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (Authors.Count > 0)
                Authors.RemoveAt(Authors.Count - 1);
            IsBusy = false;
        }

        private void AddAuthorClicked(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            Authors.Add(new Author());
            IsBusy = false;
        }

        private void OnGenreChanged(object param)
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
        private async void AddCoverClicked(object param)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            var pickedImage = await MediaPicker.PickPhotoAsync();
            if (pickedImage == null)
            {
                IsBusy = false;
                return;
            }
            string fullPath = await App.Storage.PushImageAsync(pickedImage);

            ImageCover = ImageSource.FromUri(new Uri(fullPath));

            cover = new Cover()
            {
                fileName = pickedImage.FileName,
                FullPath = fullPath
            };

            IsBusy = false;
        }
    }
}
