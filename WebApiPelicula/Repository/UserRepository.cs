using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPelicula.Data;
using WebApiPelicula.Models;
using WebApiPelicula.Repository.IRepository;

namespace WebApiPelicula.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool ExistUser(string user)
        {
            if (_db.User.Any(u => u.UserA == user))
            {
                return true;
            }

            return false;
        }
        public User GetUser(int userId)
        {
            return _db.User.FirstOrDefault(u => u.Id == userId);
        }
        public ICollection<User> GetUsers()
        {
            return _db.User.OrderBy(u => u.UserA).ToList();
        }
        public User Login(string user, string pass)
        {
            var userValidation = _db.User.FirstOrDefault(u => u.UserA == user);
            if (userValidation == null)
            {
                return null;
            }

            if (!VerifyPassHash(pass, userValidation.PasswordHash, userValidation.PasswordSalt))
            {
                return null;
            }

            return userValidation;
        }
        public User Register(User user, string pass)
        {
            byte[] passHash, passSalt;

            CreatePassHash(pass, out passHash, out passSalt);

            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;

            _db.User.Add(user);
            Save();
            return user;
        }
        private void CreatePassHash(string pass, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }
        private bool VerifyPassHash(string pass, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                for (int i = 0; i < hashComputed.Length; i++)
                {
                    if (hashComputed[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}

