using SQLite;
using System;

namespace XFTestLibrary.Models
{
    public class User
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastIn { get; set; }
    }
}
