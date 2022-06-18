using System.Threading.Tasks;

namespace XFTestLibrary.Models
{
    public class LibraryMap
    {
        public LibraryMap()
        {
            rowLenght = 10;
            shelfCount = 4;
            rackCount = 2;
            map = new int[rackCount, shelfCount, rowLenght];

            for (int rack = 0; rack < rackCount; rack++)
                for (int shelf = 0; shelf < shelfCount; shelf++)
                    for (int row = 0; row < rowLenght; row++)
                        map[rack, shelf, row] = 0;
        }

        private int rowLenght;
        private int shelfCount;
        private int rackCount;
        private int[,,] map;



        public async Task<BookPlace> GetFreePlace()
        {
            var books = await App.Database.GetPlacesAsync();
            foreach (var element in books)
                map[element.Rack, element.Shelf, element.Row] = 1;

            for (int rack = 0; rack < rackCount; rack++)
                for (int shelf = 0; shelf < shelfCount; shelf++)
                    for (int row = 0; row < rowLenght; row++)
                        if (map[rack, shelf, row] == 0)
                            return new BookPlace()
                            {
                                Rack = rack,
                                Shelf = shelf,
                                Row = row
                            };
            return null;
        }
    }
}
