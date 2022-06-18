using SQLite;


namespace XFTestLibrary.Models
{
    public class BookAndGenre
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int IdBook { get; set; }
        public int IdGenre { get; set; }
    }
}
