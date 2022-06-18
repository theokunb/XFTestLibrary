using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XFTestLibrary.Services;

namespace XFTestLibrary.Models
{
    public abstract class Filter
    {
        protected string title;

        public string Title => title;

        public abstract Task<List<Book>> ApplyFilter(IFilterVisitor filterVisitor, string pattern);
    }

    public class FilterByTitle : Filter
    {
        public FilterByTitle()
        {
            title = "По названию";
        }

        public override Task<List<Book>> ApplyFilter(IFilterVisitor filterVisitor, string pattern)
        {
            return filterVisitor.Visit(this, pattern);
        }
    }
    public class FilterByAuthor : Filter
    {
        public FilterByAuthor()
        {
            title = "По автору";
        }

        public override Task<List<Book>> ApplyFilter(IFilterVisitor filterVisitor, string pattern)
        {
            return filterVisitor.Visit(this, pattern);
        }
    }
    public class FilterByGenre : Filter
    {
        public FilterByGenre()
        {
            title = "По жанру";
        }

        public override Task<List<Book>> ApplyFilter(IFilterVisitor filterVisitor, string pattern)
        {
            return filterVisitor.Visit(this, pattern);
        }
    }
}
