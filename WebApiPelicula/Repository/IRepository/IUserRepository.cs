using System.Collections.Generic;
using WebApiPelicula.Models;

namespace WebApiPelicula.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool ExistUser(string user);
        User Register(User user, string pass);
        User Login(string user, string pass);
        bool Save();
    }
}
