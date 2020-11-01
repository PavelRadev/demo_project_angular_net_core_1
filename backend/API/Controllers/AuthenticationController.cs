using System;
using System.Threading.Tasks;
using API.Classes;
using API.Models;
using API.Utils.Authentication;
using API.Utils.Models;
using API.Utils.Models.ResponseModels;
using API.Utils.Services;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : BaseApiController
    {
        protected IUserCredentialsAuthService CredentialsAuthService { get; }
        private IUserRegistrationService UserRegistrationService { get; }
        private IUsersService UsersService { get; }

        public AuthenticationController(IServiceProvider serviceProvider, 
            IUserRegistrationService userRegistrationService,
            IUserCredentialsAuthService credentialsAuthService,
            IUsersService usersService) : base(serviceProvider)
        {
            CredentialsAuthService = credentialsAuthService;
            UserRegistrationService = userRegistrationService;
            UsersService = usersService;
        }

        /// <summary>
        /// Signin using login+pass authentication schema
        /// </summary>
        /// <param name="loginModel">Signin credentials</param>
        /// <response code="200">User model with JWT token</response>
        [HttpPost("signin-with-credentials")]
        public async Task<ActionResult<ApiResponse<AuthenticationResult>>> Signin(LoginWithCredentialsRequest loginModel)
        {
            var signinResult = await CredentialsAuthService.AuthenticateUserAsync(RepProvider.Users,
                loginModel.Login,
                loginModel.Password);

            return SuccessResponse(signinResult);
        }

        /// <summary>
        /// Returns current user if valid token provided
        /// </summary>
        /// <response code="200">Current user model</response>
        /// <response code="401">Token invalid</response>
        [HttpGet("current-user")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<User>>> GetCurrentUser()
        {
            var currentUser = await GetCurrentUserAsync();
            return SuccessResponse(currentUser);
        }


        /// <summary>
        /// Verifies if email is already taken or not
        /// </summary>
        /// <response code="200">Result of is email already taken or not</response>
        [HttpPost("is-email-taken")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<bool>>> GetIsEmailAlreadyTaken(
            [FromBody] SingleValueModel<string> body)
        {
            var result = await UsersService.GetIsEmailAlreadyTaken(body.Value);

            return SuccessResponse(result);
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <response code="200">Newly created user and JWT token</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthenticationResult>>> RegisterUserAsync([FromBody] UserRegistrationModel registrationModel)
        {
            var result = await UserRegistrationService.RegisterUserAsync(registrationModel);

            return SuccessResponse(result);
        }
    }
}
