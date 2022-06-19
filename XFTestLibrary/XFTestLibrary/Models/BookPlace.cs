using SQLite;

namespace XFTestLibrary.Models
{
    public class BookPlace
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int Row { get; set; }
        public int Rack { get; set; }
        public int Shelf { get; set; }


        public int DisplayRow => Row + 1;
        public int DisplayRack => Rack + 1;
        public int DisplayShelf => Shelf + 1;
    }
}
