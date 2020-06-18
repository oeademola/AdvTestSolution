using System.Threading.Tasks;
using Advansio.API.Models;

namespace Advansio.API.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> SaveAsync();
        Task<User> GetUserByAccountNo(string accountNumber);
        Task<User> GetUser(int id);
    }
}