using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Utils.Services;
using DB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Utils.Authentication
{
    public abstract class BaseAuthenticationService: IUserAuthService
    {
        protected IConfiguration Configuration { get; }

        protected string Issuer => Configuration.GetSection("JwtOptions").GetValue<string>("Issuer");
        protected string Audience => Configuration.GetSection("JwtOptions").GetValue<string>("Audience");
        protected string Key => Configuration.GetSection("JwtOptions").GetValue<string>("Key");
        protected long Lifetime => Configuration.GetSection("JwtOptions").GetValue<long>("LifeTime");
        
        public BaseAuthenticationService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<AuthenticationResult> GetAuthenticationResultByUserAsync(User user)
        {
            var authenticationResult = new AuthenticationResult {User = user, Token = _generateTokenByUser(user)};

            return authenticationResult;
        }
        
        private string _generateTokenByUser(User user)
        {
            var identity = _getIdentity(user);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(Lifetime)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)),
                    SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity _getIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("IsGlobalAdmin", user.IsGlobalAdmin.ToString())
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, "UserRoleId");
            return claimsIdentity;
        }
    }
}