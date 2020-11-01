﻿using System;
 using System.Collections.Generic;
 using System.Threading.Tasks;
 using API.Utils.Models;
 using DB.Data;
 using DB.Data.Queries;
 using DB.Models;
 using Microsoft.EntityFrameworkCore;
 using Utils;
 using Utils.Exceptions;
 using Utils.Hashing;

namespace API.Utils.Services
{
    public interface IUsersService
    {
        Task<bool> GetIsEmailAlreadyTaken(string email);
        Task<User> CreateUser(string email, string password, string firstName, string lastName, string companyName); 
    }

    public class UsersService : IUsersService
    {
        private IRepositoryProvider RepositoryProvider { get; }

        public UsersService(IRepositoryProvider repositoryProvider, ISessionContextService sessionContextService)
        {
            RepositoryProvider = repositoryProvider;
        }
        
        
        public async Task<bool> GetIsEmailAlreadyTaken(string email)
        {
            return await RepositoryProvider.Users
                .Queryable
                .AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<User> CreateUser(string email, string password, string firstName, string lastName, string companyName)
        {
            if (await GetIsEmailAlreadyTaken(email))
            {
                throw new BadRequestException("Email is already taken");
            }
            
            var hashedPassword = CryptoHelper.HashPassword(password);
            var newUser = new User
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                HashedPassword = hashedPassword,
                CompanyName = companyName
            };

            await RepositoryProvider.Users.AddAsync(newUser);
            await RepositoryProvider.SaveChangesAsync();

            return newUser;
        }
    }
}