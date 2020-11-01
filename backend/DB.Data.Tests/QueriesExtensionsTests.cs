using System;
using System.Linq;
using System.Threading.Tasks;
using DB.Data.Queries;
using Db.Data.Repositories;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Utils;

namespace DB.Data.Tests
{
    public class QueriesExtensionsTests
    {
        private BaseRep<User> _repository;

        private DemoDbContext _context;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<DemoDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            
            _context = new DemoDbContext(options);
            
            _repository = new BaseRep<User>(_context);
            
            var dataToInsert = Enumerable.Range(0, RandomUtils.GetRandomNumber(1, 20))
                .ToList()
                .Select(x => GetRandomUserToInsert())
                .ToList();
            await _repository.AddOrUpdateRangeAsync(dataToInsert);
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _context.DisposeAsync();
            _context = null;
        }

        [Test]
        public async Task GetByIdAsyncShouldReturnTheNeededEntry()
        {
            var firstEntityFromStorage = await _repository.Queryable.FirstOrDefaultAsync();
            
            var person = await _repository.Queryable
                .GetByIdAsync(firstEntityFromStorage.Id);

            Assert.AreEqual(firstEntityFromStorage.Id, person.Id);
        }

        [Test]
        public void GetByIdAsyncShouldThrowErrorIfEntryWithIdIsNotPresented()
        {
            Assert.ThrowsAsync<ApplicationException>(() => _repository.Queryable.GetByIdAsync(Guid.Empty));
        }

        [Test]
        public async Task ToListWithTotalsAsyncShouldReturnCorrectTotalNumber()
        {
            var listWithTotals = await _repository.Queryable
                .WithTrashed()
                .ToListWithTotalsAsync();

            var entriesInTableCount = await _repository.Queryable.CountAsync();

            Assert.AreEqual(entriesInTableCount, listWithTotals.TotalCount);
        }

        private User GetRandomUserToInsert()
        {
            return new User
            {
                Email = RandomUtils.GetRandomString(10) + "@email.com",
                FirstName = RandomUtils.GetRandomString(10),
                LastName = RandomUtils.GetRandomString(10)
            };
        }
    }
}