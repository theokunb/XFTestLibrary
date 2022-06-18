using SQLite;

namespace XFTestLibrary.Models
{
    public class Author
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}
