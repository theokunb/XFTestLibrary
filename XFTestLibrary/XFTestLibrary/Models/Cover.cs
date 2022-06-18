using SQLite;

namespace XFTestLibrary.Models
{
    public class Cover
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string fileName { get; set; }
        public string FullPath { get; set; }
    }
}
