using System.Threading.Tasks;
using Advansio.API.Models;

namespace Advansio.API.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser (int id);
        Task<User> GetUserByAccountNo(string accountNumber);
        Task<User> FundAccount(int id, decimal amount);
        Task<User> TransferFund(int id, decimal amount, string accountNumber);
    }
}