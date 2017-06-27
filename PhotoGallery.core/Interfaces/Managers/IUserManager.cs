using PhotoGallery.core.Models;

namespace PhotoGallery.core.Interfaces.Managers
{
    public interface IUserManager<T> where T : UserModel
    {
        //create user
        T RegisterUser(T obj);

        //get user by id
        T GetUserByID(int id);

        //update user
        bool EditUser(T obj);

        //delete user
        bool DeleteUser(T obj);

        //validate User
        T ValidateUser(T obj);
    }
}
