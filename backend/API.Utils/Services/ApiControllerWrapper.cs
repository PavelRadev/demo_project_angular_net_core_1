using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace API.Utils.Services
{
    public interface IApiControllerWrapper
    {
        Guid? GetCurrentUserId(ClaimsPrincipal user);
    }

    public class ApiControllerWrapper : IApiControllerWrapper
    {
        private delegate bool Parser<T>(string input, out T b);

        public Guid? GetCurrentUserId(ClaimsPrincipal user)
        {
            var value = GetClaimValue(user, "UserId", Guid.Empty, Guid.TryParse);

            if (value == Guid.Empty)
            {
                return null;
            }
            
            return value;
        }
        
        private T GetClaimValue<T>(ClaimsPrincipal user, string claimId, T defaultValue, Parser<T> parser)
        {
            var claim = user?.FindFirst(claimId);
            if (claim != null && parser(claim.Value, out T value))
                return value;
            return defaultValue;
        }
    }
}