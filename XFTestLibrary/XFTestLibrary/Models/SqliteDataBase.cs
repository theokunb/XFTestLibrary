using XFTestLibrary.Services;
using SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace XFTestLibrary.Models
{
    public class SqliteDataBase : IDataBase, IFilterVisitor
    {
        public SqliteDataBase(string path)
        {
            database = new SQLiteAsyncConnection(path);
            database.CreateTableAsync<Book>().Wait();
            database.CreateTableAsync<BookAndAuthor>().Wait();
            database.CreateTableAsync<Author>().Wait();
            database.CreateTableAsync<BookAndGenre>().Wait();
            database.CreateTableAsync<Genre>().Wait();
            database.CreateTableAsync<BookPlace>().Wait();
            database.CreateTableAsync<Cover>().Wait();
            database.CreateTableAsync<User>().Wait();
        }

        private readonly SQLiteAsyncConnection database;


        public Task<List<Book>> GetBooksAsync()
        {
            return database.Table<Book>().ToListAsync();
        }


        public Task<List<BookPlace>> GetPlacesAsync()
        {
            return database.Table<BookPlace>().ToListAsync();
        }

        public async Task<Book> InsertBookAsync(Book book)
        {
            await database.InsertAsync(book);
            return (await database.Table<Book>().ToArrayAsync()).Last();
        }

        public async Task<BookAndAuthor> InsertBookAndAuthorAsync(BookAndAuthor bookAndAuthor)
        {
            await database.InsertAsync(bookAndAuthor);
            return (await database.Table<BookAndAuthor>().ToArrayAsync()).Last();
        }

        public async Task<Author> InsertAuthorAsync(Author author)
        {
            await database.InsertAsync(author);
            return (await database.Table<Author>().ToArrayAsync()).Last();
        }

        public async Task<BookAndGenre> InsertBookAndGenreAsync(BookAndGenre bookAndGenre)
        {
            await database.InsertAsync(bookAndGenre);
            return (await database.Table<BookAndGenre>().ToArrayAsync()).Last();
        }

        public async Task<Genre> InsertGenreAsync(Genre genre)
        {
            await database.InsertAsync(genre);
            return (await database.Table<Genre>().ToArrayAsync()).Last();
        }

        public Task<BookPlace> InsertBookPlaceAsync(BookPlace bookPlace)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Author> GetAuthorByName(string name)
        {
            var collection = await database.Table<Author>().Where(author => author.FullName == name).ToListAsync();
            if (collection.Count == 0)
                return null;
            else return collection.First();
        }

        public async Task<Genre> GetGenreByTitleAsync(string title)
        {
            var collection = await database.Table<Genre>().Where(genre => genre.Title == title).ToListAsync();
            if (collection.Count == 0)
                return null;
            else return collection.First();
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            var collection = await database.Table<Book>().Where(book => book.Title == title).ToListAsync();
            if (collection.Count == 0)
                return null;
            else return collection.First();
        }

        public Task<int> UpdateBookAsync(Book book)
        {
            return database.UpdateAsync(book);
        }

        public async Task<BookPlace> InsertBookPlace(BookPlace bookPlace)
        {
            await database.InsertAsync(bookPlace);
            return (await GetBookPlacesAsync()).Last();
        }

        public Task<List<BookPlace>> GetBookPlacesAsync()
        {
            return database.Table<BookPlace>().ToListAsync();
        }


        public Task<List<Book>> GetBooksByFilterAsync(Filter filter, string pattern="")
        {
            return filter.ApplyFilter(this, pattern);
        }

        public async Task<List<Book>> Visit(FilterByTitle filter, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                var books = from element in await database.Table<Book>().ToListAsync()
                            where element != null
                            select element;
                return books.ToList();
            }
            else
            {
                var books = from element in await database.Table<Book>().ToListAsync()
                            where element.Title.Contains(pattern)
                            select element;
                return books.ToList();
            }
        }

        public Task<List<Book>> Visit(FilterByAuthor filter, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return database.Table<Book>().ToListAsync();
            }
            else
            {
                var result = database.QueryAsync<Book>($"select Book.Id, Book.Title, Book.IdPlace, Book.IdCover from Book " +
                    $"join BookAndAuthor on Book.Id = BookAndAuthor.IdBook " +
                    $"join Author on BookAndAuthor.IdAuthor = Author.Id where Author.FullName like '%{pattern}%'");
                return result;
            }
        }

        public Task<List<Book>> Visit(FilterByGenre filter, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return database.Table<Book>().ToListAsync();
            }
            else
            {
                var result = database.QueryAsync<Book>($"select Book.Id, Book.Title, Book.IdPlace, Book.IdCover from Book " +
                    $"join BookAndGenre on Book.Id = BookAndGenre.IdBook " +
                    $"join Genre on BookAndGenre.IdGenre = Genre.Id where Genre.Title like '%{pattern}%'");
                return result;
            }
        }

        public Task<List<BookAndAuthor>> GetAuthorsOfBookAsync(int idBook)
        {
            return database.Table<BookAndAuthor>().Where(book => book.IdBook == idBook).ToListAsync();
        }

        public Task<Author> GetAuthorAsync(int idAuthor)
        {
            return database.Table<Author>().Where(author => author.Id == idAuthor).FirstAsync();
        }

        public Task<List<BookAndGenre>> GetGenresOfBookAsync(int idBook)
        {
            return database.Table<BookAndGenre>().Where(book => book.IdBook == idBook).ToListAsync();
        }

        public Task<Genre> GetGenreAsync(int idGenre)
        {
            return database.Table<Genre>().Where(author => author.Id == idGenre).FirstAsync();
        }

        public Task RemoveBookAndAuthor(int idBook)
        {
            return database.ExecuteAsync($"delete from BookAndAuthor where IdBook = {idBook}");
        }

        public Task RemoveBookAndGenre(int idBook)
        {
            return database.ExecuteAsync($"delete from BookAndGenre where IdBook = {idBook}");
        }

        public async Task<Cover> InsertCoverAsync(Cover cover)
        {
            await database.InsertAsync(cover);
            return (await database.Table<Cover>().ToListAsync()).Last();
        }
        public Task<Cover> GetCoverAsync(int idCover)
        {
            return database.Table<Cover>().Where(cover => cover.Id == idCover).FirstAsync();
        }

        public Task<int> InsertUserAsync(User user)
        {
            return database.InsertAsync(user);
        }

        public async Task<AuthentificationToken> AuthorizeWithLoginPasswordAsync(string login, string password)
        {
            var result = await database.QueryAsync<User>($"select * from User where Login like'{login}' and PasswordHash like '{password}'");
            if (result.Count == 0)
                return new AuthentificationToken(null);
            else
            { 
                var token = new AuthentificationToken(result.First());
                token.User.LastIn = System.DateTime.Now;
                return token;
            }
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var result = await database.QueryAsync<User>($"select * from User where Login like '{userName}'");
            if (result.Count == 0)
                return null;
            else
                return result.First();
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return database.UpdateAsync(user);
        }
    }
}
