using System.Collections.Generic;
using System.Threading.Tasks;
using XFTestLibrary.Models;

namespace XFTestLibrary.Services
{
    public interface IDataBase
    {
        Task<List<Book>> GetBooksAsync();
        Task<List<BookPlace>> GetPlacesAsync();
        Task<Book> InsertBookAsync(Book book);
        Task<BookAndAuthor> InsertBookAndAuthorAsync(BookAndAuthor bookAndAuthor);
        Task<Author> InsertAuthorAsync(Author author);
        Task<BookAndGenre> InsertBookAndGenreAsync(BookAndGenre bookAndGenre);
        Task<Genre> InsertGenreAsync(Genre genre);
        Task<BookPlace> InsertBookPlaceAsync(BookPlace bookPlace);
        Task<Book> GetBookByTitleAsync(string title);
        Task<Author> GetAuthorByName(string name);
        Task<Genre> GetGenreByTitleAsync(string title);
        Task<int> UpdateBookAsync(Book book);
        Task<BookPlace> InsertBookPlace(BookPlace bookPlace);
        Task<List<BookPlace>> GetBookPlacesAsync();
        Task<List<Book>> GetBooksByFilterAsync(Filter filter, string pattern);
        Task<List<BookAndAuthor>> GetAuthorsOfBookAsync(int idBook);
        Task<Author> GetAuthorAsync(int idAuthor);
        Task<List<BookAndGenre>> GetGenresOfBookAsync(int idBook);
        Task<Genre> GetGenreAsync(int idGenre);
        Task RemoveBookAndAuthor(int idBook);
        Task RemoveBookAndGenre(int idBook);
        Task<Cover> InsertCoverAsync(Cover cover);
        Task<Cover> GetCoverAsync(int idCover);

        Task<int> InsertUserAsync(User user);
        Task<AuthentificationToken> AuthorizeWithLoginPasswordAsync(string login,string password);
        Task<User> GetUserByUserName(string userName);
        Task<int> UpdateUserAsync(User user);
        Task<BookPlace> GetBookPlaceAsync(int idBook);
    }
}
