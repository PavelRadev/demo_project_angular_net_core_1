using DB.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DB.Data
{
    public partial class DemoDbContext
    {
        private void DefineData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("914a2250-fe53-4a65-95d3-8edc6e079884"),
                    FirstName = "Root",
                    LastName = "Admin",
                    CompanyName = "TestCompany",
                    Email = "main@mailinator.com",
                    HashedPassword = "AQAAAAEAACcQAAAAEAJKeZ4fD8jinehtYPskwJ6Wuau1g1T3e7xyt+3uUyiASHipi9JY0X5hcy+i7RWmZg==",
                    IsGlobalAdmin = true
                });
        }
    }
}