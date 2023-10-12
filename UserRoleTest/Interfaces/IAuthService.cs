using UserRoleTest.Models;

namespace UserRoleTest.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> ValidateUserCredentials(Account account);

        public Task<bool> CheckAccountExistsence(string userName);

        public Task<int> AddAccount(Account account);
    }
}
