using System.Linq;
using System.Threading.Tasks;
using Db.Data.Repositories;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Utils;

namespace DB.Data.Tests
{
    public class BaseRepTests
    {
        private BaseRep<User> _baseRep;
        private DemoDbContext _context;
        
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DemoDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            
            _context = new DemoDbContext(options);
            
            _baseRep = new BaseRep<User>(_context);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _context.DisposeAsync();
            _context = null;
        }
        
        [Test]
        public async Task AddAsyncShouldInsertEntitiesToStorage()
        {
            var initialPersonsCount = await _baseRep.Queryable.CountAsync();
            
            await _baseRep.AddAsync(GetRandomUserToInsert());
            await _context.SaveChangesAsync();
            
            var personsCount = await _baseRep.Queryable.CountAsync();
            
            Assert.Greater(personsCount, initialPersonsCount);
        }
        
        [Test]
        public async Task AddAsyncShouldNotAffectStorageIfSaveNotCalled()
        {
            var initialPersonsCount = await _baseRep.Queryable.CountAsync();
            
            await _baseRep.AddAsync(GetRandomUserToInsert());
            
            var personsCount = await _baseRep.Queryable.CountAsync();
            
            Assert.AreEqual(personsCount, initialPersonsCount);
        }
        
        [Test]
        public async Task AddAsyncShouldReturnTrackingEntity()
        {
            var entity = GetRandomUserToInsert();
            await _baseRep.AddAsync(entity);
            await _context.SaveChangesAsync();

            var initialName = entity.FirstName;

            entity.FirstName = RandomUtils.GetRandomString(10);
            _baseRep.Update(entity);
            await _context.SaveChangesAsync();

            var entityFromStorage = await _baseRep.Queryable.FirstOrDefaultAsync(x => x.Id == entity.Id);
            
            
            Assert.AreNotEqual(initialName, entityFromStorage.FirstName);
        }
        
        [Test]
        public async Task GetAllItemsCountAsyncShouldReturnTheCorrectEntitiesCount()
        {
            var initialPersonsCount = await _baseRep.GetAllItemsCountAsync();

            var dataToInsert = Enumerable.Range(0, RandomUtils.GetRandomNumber(0, 20))
                .ToList()
                .Select(x => GetRandomUserToInsert())
                .ToList();
            
            await _baseRep.AddOrUpdateRangeAsync(dataToInsert);
            await _context.SaveChangesAsync();
            
            var personsCount = await _baseRep.GetAllItemsCountAsync();
            var personsCountFromContext = await _baseRep.Queryable.CountAsync();
            
            Assert.Greater(personsCount, initialPersonsCount);
            Assert.AreEqual(initialPersonsCount + dataToInsert.Count, personsCount);
            Assert.AreEqual(personsCountFromContext, personsCount);
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