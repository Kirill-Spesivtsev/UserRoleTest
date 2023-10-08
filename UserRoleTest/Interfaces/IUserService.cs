using UserRoleTest.Models;

namespace UserRoleTest.Interfaces
{
    public interface IUserService
    {

        Task<List<User>> GetAllUsers();

        Task<User> GetUserById(int? postId);

        Task<int> AddUser(User user);

        Task<int> DeleteUser(int? userId);

        Task UpdateUser(User user);
    
    }
    
}
