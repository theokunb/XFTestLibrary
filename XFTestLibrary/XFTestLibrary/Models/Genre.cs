using SQLite;

namespace XFTestLibrary.Models
{
    public class Genre
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
