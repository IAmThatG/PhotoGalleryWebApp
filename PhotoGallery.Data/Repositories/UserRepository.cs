using PhotoGallery.core.Interfaces.Repositories;
using PhotoGallery.core.Models;
using PhotoGallery.Data.Entities;
using System.Linq;
using System;

namespace PhotoGallery.Data.Repositories
{
    public class UserRepository : IUserRepository<UserModel>
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public int CreateUser(UserModel obj)
        {
            var userEntity = new User
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Password = obj.Password,
            };
            _context.Users.Add(userEntity);
            return _context.SaveChanges();
        }

        public int DeleteUser(UserModel obj)
        {
            var userEntity = new User
            {
                UserID = obj.UserID,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
            };
            _context.Users.Remove(userEntity);
            return _context.SaveChanges();
        }

        public UserModel GetUserByEmail(string email)
        {
            UserModel userModel = null;

            //check if user with that email exist in the DB
            var user = _context.Users.Where(u => u.Email.Equals(email)).SingleOrDefault();
            
            //if user exist in DB, map user entity to model and return the model
            if(user != null)
            {
                userModel = new UserModel
                {
                    UserID = user.UserID,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Password = user.Password
                };
            }
            return userModel;
        }

        public UserModel GetUserByID(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            var userModel = new UserModel
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
            };
            return userModel;
        }

        public int Update(UserModel obj)
        {
            var user = new User
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Password = obj.Password
            };
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges();
        }

        public UserModel ValidateUser(UserModel obj)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == obj.Email && u.Password.Equals(obj.Password));
            var userModel = new UserModel
            {
                UserID = user.UserID,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email
            };
            return userModel;
        }
    }
}
