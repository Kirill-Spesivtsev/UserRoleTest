using Microsoft.EntityFrameworkCore;
using UserRoleTest.Data;
using UserRoleTest.Helpers;
using UserRoleTest.Interfaces;
using UserRoleTest.Models;

namespace UserRoleTest.Services
{
    public class RoleService : IRoleService
    {
        ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRoles() => await _context?.Roles?.ToListAsync()!;

        public async Task<List<Role>> GetAllRolesFiltered(
            PaginationOptions pagingOptions, 
            RolesFilteringOptions filterOptions,
            RolesSortingHelper sortingOptions)
        {
            var allRoles = _context?.Roles;

            var filtered = allRoles?
                    .Where( i => i.Id.ToString().ToUpper().Contains(filterOptions.Id.ToUpper()))
                    .Where( n => n.Name.ToUpper().Contains(filterOptions.Name.ToUpper()));

            var sorted = sortingOptions.SortResponse(filtered);

            int amount = sorted.Count();
            var pagingFilter = new PaginationOptions(pagingOptions.CurrentPage, pagingOptions.PageSize, amount);

            var paged = sorted
                .Skip((pagingFilter.CurrentPage - 1) * pagingFilter.PageSize)
                .Take(pagingFilter.PageSize);
            
            return await paged.ToListAsync();
        }

        public async Task<Role> GetRoleById(int? roleId)
        {
            if (_context != null && roleId != null)
            {
                return await _context.Roles
                    .Include(q => q.UserRoles).ThenInclude(q => q.Role).FirstAsync(x => x.Id == roleId);
            }
            return null!;

        }
    }
}
