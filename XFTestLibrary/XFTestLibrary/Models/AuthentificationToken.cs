using System;

namespace XFTestLibrary.Models
{
    public class AuthentificationToken
    {
        public AuthentificationToken(User user)
        {
            this.user = user;
            lifeTime = 10;
        }

        private readonly User user;
        private readonly int lifeTime;

        public bool IsValidToken => user != null && DateTime.Now < user.LastIn.AddSeconds(lifeTime);
        public User User => user;
    }
}
