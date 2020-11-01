using System;
using System.Threading.Tasks;
using API.Classes;
using API.Utils.Extensions;
using API.Utils.Models;
using API.Utils.Models.ResponseModels;
using API.Utils.Services;
using DB.Data.Queries;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route(("api/users"))]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private IUsersService UsersService { get; }
        private IConfiguration Configuration { get; }
        
        
        public UsersController(IServiceProvider serviceProvider, 
            IUsersService usersService, 
            IConfiguration configuration) : base(serviceProvider)
        {
            UsersService = usersService;
            Configuration = configuration;
        }
        
        /// <summary>
        /// Get the list of all the users registered in the system
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<ListWithTotals<User>>>> GetAllUsersList([FromQuery] SoftDeletableListFilterModel filters)
        {
            var results = await RepProvider.Users
                .Queryable
                .ApplySoftDeletableFilter(filters)
                .AsNoTracking()
                .ToListWithTotalsAsync(filters.PageSize,filters.Page, filters.OrderedBy, filters.OrderReversed);

            return SuccessResponse(results);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetById(Guid id)
        {
            var result = await RepProvider.Users
                .Queryable
                .WithTrashed()
                .GetByIdAsync(id);

            return SuccessResponse(result);
        }
    }
}