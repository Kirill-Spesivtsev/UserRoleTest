using UserRoleTest.Helpers;
using UserRoleTest.Models;

namespace UserRoleTest.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRoles();

        public Task<List<Role>> GetAllRolesFiltered(
            PaginationOptions pagingOptions, 
            RolesFilteringOptions filterOptions,
            RolesSortingHelper sortingOptions);

        Task<Role> GetRoleById(int? postId);
    }
}
