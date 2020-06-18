using System.Threading.Tasks;
using Advansio.API.Interfaces;
using Advansio.API.Models;
using Microsoft.AspNetCore.Http;

namespace Advansio.API.Services 
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService (IUserRepository userRepository) {
            _userRepository = userRepository;

        }

        public Task<User> GetUser (int id) 
        {
            return _userRepository.GetUser(id);
        }

        public Task<User> GetUserByAccountNo(string accountNumber)
        {
            return _userRepository.GetUserByAccountNo(accountNumber);
        }

        public async Task<User> FundAccount(int id, decimal amount) 
        {
            var user = await _userRepository.GetUser(id);
            user.AccountBalance += amount;
            await _userRepository.SaveAsync();
            return user;
        }

        public async Task<User> TransferFund(int id, decimal amount, string accountNumber)
        {
            var currentUser = await _userRepository.GetUser(id);
            currentUser.AccountBalance -= amount;

            var beneficiary = await _userRepository.GetUserByAccountNo(accountNumber);
                            
            beneficiary.AccountBalance += amount;

            await _userRepository.SaveAsync();

            return currentUser;
        }

      
    }
}