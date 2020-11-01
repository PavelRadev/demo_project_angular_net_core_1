using System;
using System.Net;
using System.Threading.Tasks;
using API.Utils.Interfaces;
using API.Utils.Models.ResponseModels;
using DB.Data;
using DB.Data.Queries;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utils;

namespace API.Classes
{
    /// <summary>
    /// Basic controller. Expands the framework's base controller with additional formatting methods as well as common properties
    /// </summary>
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IApiResponseFormatter ResponseFormatter { get; }
        protected ISessionContextService SessionContextService { get; }
        private readonly DemoDbContext _dbContext;

        private readonly Lazy<Guid?> _currentUserId;
        protected Guid? CurrentUserId => _currentUserId.Value;

        private User _currentUser;

        protected async Task<User> GetCurrentUserAsync()
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            if (CurrentUserId != null && RepProvider != null)
            {
                _currentUser = await RepProvider.Users.Queryable
                    .WithTrashed()
                    .GetByIdAsync(GetCurrentUserId());
            }

            return _currentUser;
        }

        protected IRepositoryProvider RepProvider { get; }

        public BaseApiController(IServiceProvider serviceProvider)
        {
            ResponseFormatter = serviceProvider.GetService<IApiResponseFormatter>();
            RepProvider = serviceProvider.GetService<IRepositoryProvider>();
            SessionContextService = serviceProvider.GetService<ISessionContextService>();
            
            _currentUserId = new Lazy<Guid?>(() => SessionContextService.CurrentUserId, true);
        }
       
        
        protected Guid GetCurrentUserId()
        {
            if (CurrentUserId is null)
            {
                throw new ApplicationException("Current user id isn't set for request");
            }

            return CurrentUserId.Value;
        }

        protected ActionResult<ApiResponse<T>> SuccessResponse<T>(T data)
        {
            return RespondWithResult(HttpStatusCode.OK, ResponseFormatter.GetSuccessResponseBody(data));
        }

        protected ActionResult<ApiResponse<T>> ValidationErrorResponse<T>(T data)
        {
            return RespondWithResult(HttpStatusCode.BadRequest,
                ResponseFormatter.GetValidationErrorResponseBody(data));
        }

        protected ActionResult<ApiResponse<T>> SuccessCreateResponse<T>(T data)
        {
            return RespondWithResult(HttpStatusCode.Created,
                ResponseFormatter.GetSuccessCreateResponseBody(data));
        }

        protected ActionResult<ApiResponse<T>> SuccessUpdateResponse<T>(T data)
        {
            return RespondWithResult(HttpStatusCode.OK, ResponseFormatter.GetSuccessUpdateResponseBody(data));
        }

        protected ActionResult<ApiResponse<T>> SuccessDeleteResponse<T>(T data)
        {
            return RespondWithResult(HttpStatusCode.OK, ResponseFormatter.GetSuccessDeleteResponseBody(data));
        }

        private ActionResult<ApiResponse<T>> RespondWithResult<T>(HttpStatusCode code, ApiResponse<T> body)
        {
            return StatusCode((int) code, body);
        }
    }
}