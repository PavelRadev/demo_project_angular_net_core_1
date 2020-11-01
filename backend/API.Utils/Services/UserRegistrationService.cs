using System.Threading.Tasks;
using API.Utils.Authentication;
using API.Utils.Models;
using DB.Data;
using Db.Data.Repositories;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;
using Utils.Hashing;
using System.ComponentModel.DataAnnotations;

namespace API.Utils.Services
{
    public interface IUserRegistrationService
    {
        Task<AuthenticationResult> RegisterUserAsync(UserRegistrationModel registrationModel);
    }

    public class UserRegistrationService : IUserRegistrationService
    {
        private IRepositoryProvider RepositoryProvider { get; }
        private IUserCredentialsAuthService UserCredentialsAuthService { get; } 
        private IUsersService UsersService { get; }
        
        public UserRegistrationService(IRepositoryProvider repositoryProvider, 
            IUserCredentialsAuthService userCredentialsAuthService,
            IUsersService usersService)
        {
            RepositoryProvider = repositoryProvider;
            UserCredentialsAuthService = userCredentialsAuthService;
            UsersService = usersService;
        }

        public async Task<AuthenticationResult> RegisterUserAsync(UserRegistrationModel registrationModel)
        {

            if (!new EmailAddressAttribute().IsValid(registrationModel.Email))
            {
                throw new BadRequestException("Email address is incorrect");
            }

            if (registrationModel.Password != registrationModel.ConfirmPassword)
            {
                throw new BadRequestException("Password confirmation not equals to password");
            }

            await using var transaction = await RepositoryProvider.Db.Database.BeginTransactionAsync();

            try
            {
                var newUser = await UsersService.CreateUser(registrationModel.Email,
                    registrationModel.Password,
                    registrationModel.FirstName,
                    registrationModel.LastName,
                    registrationModel.CompanyName);

                var authData = await UserCredentialsAuthService.AuthenticateUserAsync(RepositoryProvider.Users, newUser.Email,
                    registrationModel.Password);

                await transaction.CommitAsync();

                return authData;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}