using SIMS_Project.Models;

namespace SIMS_Project.Data
{
    public interface IUserRepository
    {
        User Login(string username, string password);
    }
}