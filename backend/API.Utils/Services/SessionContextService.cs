using System;
using Microsoft.AspNetCore.Http;
using Utils;

namespace API.Utils.Services
{
    public class SessionContextService : ISessionContextService
    {
        private IHttpContextAccessor ContextAccessor { get; }
        private IApiControllerWrapper ApiControllerWrapper { get; }
        
        public SessionContextService(IHttpContextAccessor contextAccessor,
            IApiControllerWrapper apiControllerWrapper)
        {
            ContextAccessor = contextAccessor;
            ApiControllerWrapper = apiControllerWrapper;
        }

        public Guid? CurrentUserId =>
            ApiControllerWrapper.GetCurrentUserId(ContextAccessor.HttpContext?.User);
    }
}