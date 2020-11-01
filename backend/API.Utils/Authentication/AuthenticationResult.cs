using DB.Models;

namespace API.Utils.Authentication
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
