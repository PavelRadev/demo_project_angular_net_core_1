using System.Threading.Tasks;
using Db.Data.Repositories;
using DB.Models;

namespace API.Utils.Authentication
{
    public interface IUserCredentialsAuthService: IUserAuthService
    {
        Task<AuthenticationResult> AuthenticateUserAsync(IBaseRep<User> usersRep, string email, string password);
    }
}
