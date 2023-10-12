using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserRoleTest.Data;
using UserRoleTest.Interfaces;
using UserRoleTest.Models;

namespace UserRoleTest.Services
{
    public class AuthService : IAuthService
    {
        ApplicationDbContext _context;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUserCredentials(Account account)
        {

            var passhash = GetHash(account.Password);
            var existentAcc = await _context.Accounts
                .FirstOrDefaultAsync(x => x.UserName == account.UserName);

            if (existentAcc != null)
            {
                if (existentAcc.UserName == account.UserName && existentAcc.Password == passhash)
                {
                    return true;
                }
            }
            return false;

        }

        public async Task<bool> CheckAccountExistsence(string userName)
        {
            if (userName != null)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == userName);
                return account != null;
            }
            return false;
        }

        public string GetHash(string str)
        {
            StringBuilder sb = new StringBuilder();

            using (var hash = SHA256.Create())            
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(str));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
                    
            }
            return sb.ToString();

        }

        public async Task<int> AddAccount(Account account)
        {
            if (_context != null)
            {
                account.Password = GetHash(account.Password);
                await _context.Accounts.AddAsync(account);
                var code = await _context.SaveChangesAsync();

                return code;
            }

            return 0;
        }
        
    }
}
