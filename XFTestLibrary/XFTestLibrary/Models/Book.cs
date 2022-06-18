using SQLite;
using System.Collections.Generic;

namespace XFTestLibrary.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public int IdPlace { get; set; }
        public int IdCover { get; set; }
    }
}
