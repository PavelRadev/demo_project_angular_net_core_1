using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Utils.Services;
using Db.Data.Repositories;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utils.Exceptions;
using Utils.Hashing;

namespace API.Utils.Authentication
{
    public class UserCredentialsAuthService : BaseAuthenticationService, IUserCredentialsAuthService
    {
        public UserCredentialsAuthService(IConfiguration configuration) : base(configuration)
        {
        }
        
        private async Task<User> GetFirstAvailableUserByEmailAsync(IBaseRep<User> usersRep, string email)
        {
            return await usersRep.Queryable
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<AuthenticationResult> AuthenticateUserAsync(IBaseRep<User> usersRep, string email, string password)
        {
            var userWithLogin = await GetFirstAvailableUserByEmailAsync(usersRep, email);

            if (userWithLogin == null)
            {
                var fieldErrors = new Dictionary<string, string> {{"email", "User not found"}};

                throw new DemoAppException(null, fieldErrors);
            }

            var isSamePassword = CryptoHelper.VerifyHashedPassword(userWithLogin.HashedPassword, password);

            if (!isSamePassword)
            {
                var fieldErrors = new Dictionary<string, string> {{"password", "Incorrect password"}};

                throw new DemoAppException(null, fieldErrors);
            }

            return await GetAuthenticationResultByUserAsync(userWithLogin);
        }
    }
}