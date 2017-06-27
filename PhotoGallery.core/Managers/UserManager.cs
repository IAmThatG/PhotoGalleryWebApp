using PhotoGallery.core.Interfaces.Managers;
using PhotoGallery.core.Interfaces.Repositories;
using PhotoGallery.core.Models;

namespace PhotoGallery.core.Managers
{
    public class UserManager : IUserManager<UserModel>
    {
        private IUserRepository<UserModel> _userRepository;

        //Inject UserRepository into UserManager anytime an instance of UserManaer is created
        public UserManager(IUserRepository<UserModel> userRepository)
        {
            _userRepository = userRepository;
        } 

        public bool DeleteUser(UserModel obj)
        {
            return _userRepository.DeleteUser(obj) > 0 ? true : false;
        }

        public bool EditUser(UserModel obj)
        {
            return _userRepository.Update(obj) > 0 ? true : false;
        }

        public UserModel GetUserByID(int id)
        {
            return _userRepository.GetUserByID(id);
        }

        public UserModel RegisterUser(UserModel obj)
        {
            int createdUser = 0;

            //check if user exists
            var existinguser = _userRepository.GetUserByEmail(obj.Email);

            //if user doesn't exist, create user and return the created user
            if (existinguser == null)
                createdUser = _userRepository.CreateUser(obj);
            return createdUser > 0 ? _userRepository.GetUserByEmail(obj.Email) : null;
        }

        public UserModel ValidateUser(UserModel obj)
        {
            return _userRepository.ValidateUser(obj);
        }
    }
}
