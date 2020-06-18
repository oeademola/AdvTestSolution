using System.Linq;
using System.Threading.Tasks;
using Advansio.API.Data;
using Advansio.API.Interfaces;
using Advansio.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Advansio.API.Repositories 
{
    public class UserRepository : IUserRepository 
    {
        private readonly DataContext _context;
        public UserRepository (DataContext context) 
        {
            _context = context;

        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByAccountNo(string accountNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == accountNumber);
            return user;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        
    }
}