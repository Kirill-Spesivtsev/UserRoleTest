using Microsoft.EntityFrameworkCore;
using UserRoleTest.Data;
using UserRoleTest.Interfaces;
using UserRoleTest.Models;

namespace UserRoleTest.Services
{
    public class UserService : IUserService
    {
        ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            if (_context != null)
            {
                return await _context.Users.ToListAsync();
            }
            return null;
        }

        public async Task<User> GetUserById(int? userId)
        {
            if (_context != null)
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            }
            return null;

        }

        public async Task<int> AddUser(User user)
        {
            if (_context != null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user.Id;
            }

            return 0;
        }

        public async Task<int> DeleteUser(int? userId)
        {

            if (_context != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user != null)
                {
                    _context.Users.Remove(user);

                    return await _context.SaveChangesAsync();
                }
            }

            return 0;
        }


        public async Task UpdateUser(User user)
        {
            if (_context != null)
            {
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
            }
        }

    }
}
