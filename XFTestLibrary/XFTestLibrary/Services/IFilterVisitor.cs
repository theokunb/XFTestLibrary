using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XFTestLibrary.Models;

namespace XFTestLibrary.Services
{
    public interface IFilterVisitor
    {
        Task<List<Book>> Visit(FilterByTitle filter, string pattern);
        Task<List<Book>> Visit(FilterByAuthor filter, string pattern);
        Task<List<Book>> Visit(FilterByGenre filter, string pattern);
    }
}
