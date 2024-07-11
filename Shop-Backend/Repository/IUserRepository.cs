using Shop_Backend.Models;

namespace Shop_Backend.Repository
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User? GetUser(string username, string password);
        User? GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
