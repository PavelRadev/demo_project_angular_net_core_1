using System.Threading.Tasks;
using DB.Models;

namespace API.Utils.Authentication
{
    public interface IUserAuthService
    {
        Task<AuthenticationResult> GetAuthenticationResultByUserAsync(User user);
    }
}