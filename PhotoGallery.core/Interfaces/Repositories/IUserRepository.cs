using PhotoGallery.core.Models;

namespace PhotoGallery.core.Interfaces.Repositories
{
    public interface IUserRepository<T> where T : UserModel
    {
        //Create User
        int CreateUser(T obj);

        //Get User by ID
        T GetUserByID(int id);

        //Get User by Email
        T GetUserByEmail(string email);

        //Update User
        int Update(T obj);

        //Delete User
        int DeleteUser(T obj);

        //Validate User
        T ValidateUser(T obj);
    }
}
