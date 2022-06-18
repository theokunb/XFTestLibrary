using System.Collections;
using System.Collections.Generic;

namespace XFTestLibrary.Models
{
    public class FilterSource : IEnumerable<Filter>
    {
        public FilterSource()
        {
            titles = new List<Filter>()
            {
                new FilterByTitle(),
                new FilterByAuthor(),
                new FilterByGenre()
            };
        }

        private IList<Filter> titles;


        public IEnumerator<Filter> GetEnumerator()
        {
            foreach(var element in titles)
                yield return element;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
