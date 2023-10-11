using Microsoft.EntityFrameworkCore;
using UserRoleTest.Data;
using UserRoleTest.Helpers;
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

        public async Task<List<User>> GetAllUsers() =>
            await _context?.Users?.Include(q => q.UserRoles).ThenInclude(q => q.Role).ToListAsync()!;

        public async Task<List<User>> GetAllUsersFiltered(
            PaginationOptions pagingOptions, 
            UsersFilteringOptions filterOptions,
            UsersSortingHelper sortingOptions)
        {
            var pagingFilter = new PaginationOptions(pagingOptions.PageNumber, pagingOptions.PageSize);

            var allUsers = _context?.Users?.Include(q => q.UserRoles).ThenInclude(q => q.Role);

            var filtered = allUsers
                    .Where( i => i.Id.ToString().ToUpper().Contains(filterOptions.Id.ToUpper()))
                    .Where( n => n.Name.ToUpper().Contains(filterOptions.Name.ToUpper()))
                    .Where( a => a.Age.ToString().ToUpper().Contains(filterOptions.Age.ToUpper()))
                    .Where( e => e.Email.ToUpper().Contains(filterOptions.Email.ToUpper()));

            var sorted = sortingOptions.SortResponse(filtered);

            var paged = sorted
                .Skip((pagingFilter.PageNumber - 1) * pagingFilter.PageSize)
                .Take(pagingFilter.PageSize);
            
            return await paged.ToListAsync();
        }
            
        public async Task<User> GetUserById(int? userId)
        {
            if (_context != null && userId != null)
            {
                return await _context.Users
                    .Include(q => q.UserRoles).ThenInclude(q => q.Role).FirstAsync(x => x.Id == userId);
            }
            return null!;

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


        public async Task<int> UpdateUser(int? userId, User user)
        {
            if (_context != null)
            {
                var oldUser = await GetUserById(userId);

                if (oldUser == null)
                {
                    return 0;
                }     

                oldUser.Id = user.Id;
                oldUser.Name = user.Name;
                oldUser.Email = user.Email;
                oldUser.Age = user.Age;

                _context.Users.Update(oldUser);

                await _context.SaveChangesAsync();
            }

            return 1;
        }

        public async Task<int> AddUserToRole(int? userId, int? roleId)
        {
            if (_context != null)
            {
                var record = new UserRole{UserId = userId.Value, RoleId = roleId.Value};
                await _context.UserRoles.AddAsync(record);
                await _context.SaveChangesAsync();
                return record.Id;
            }
            return 0;
        }

    }
}
