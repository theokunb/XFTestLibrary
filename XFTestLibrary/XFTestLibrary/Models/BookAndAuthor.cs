using SQLite;

namespace XFTestLibrary.Models
{
    public class BookAndAuthor
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int IdBook { get; set; }
        public int IdAuthor { get; set; }
    }
}
