using SIMS_Project.Models;

namespace SIMS_Project.Interface
{
    public interface IUserRepository
    {
        User Login(string username, string password);
        void AddUser(User user);
        bool UsernameExists(string username);

        List<User> GetInstructors();
        User GetUserById(int id);
    }
}