using System.Collections.Generic;
using System.Threading.Tasks;
using ImmutableCaches.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImmutableCaches.UnitTests
{
    [TestClass]
    public class CachedDepartmentNamesRepositoryTests
    {
        [TestMethod]
        public async Task ShouldSetGetCacheEntries()
        {
            var departmentsRepositoryMock = new Mock<IDepartmentsRepository>();
            departmentsRepositoryMock.Setup(x => x.GetDepartmentNameDtos())
                .ReturnsAsync(new List<DepartmentNameDto>
                {
                    new()
                    {
                        Id = 123,
                        Name = "test 123",
                    },
                    new()
                    {
                        Id = 456,
                        Name = "test 456",
                    },
                    new()
                    {
                        Id = 789,
                        Name = "test 789",
                    }
                });

            var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CachedDepartmentNames(cache, departmentsRepositoryMock.Object);

            var allDeptNames = await sut.GetAll();
            var dept = await sut.GetDepartmentNameById(123);

            Assert.AreEqual(3, allDeptNames.Count);
            Assert.AreEqual("test 123", dept?.Name);

            // assert that should have executed one query, means no new query, because all data is already in memoryCache
            departmentsRepositoryMock.Verify(x => x.GetDepartmentNameDtos(), Times.Once);
        }

        [TestMethod]
        public async Task ShouldHaveCorrectLifeCycle_UponSetRemoveCache()
        {
            var departmentsRepositoryMock = new Mock<IDepartmentsRepository>();
            departmentsRepositoryMock.Setup(x => x.GetDepartmentNameDtos())
                .ReturnsAsync(new List<DepartmentNameDto>());

            var cache = new MemoryCache(new MemoryCacheOptions());
            const string cacheKey = CachedDepartmentNames.CacheKey;
            var sut = new CachedDepartmentNames(cache, departmentsRepositoryMock.Object);

            Assert.IsFalse(cache.TryGetValue(cacheKey, out _)); // no cache at the initial stage

            await sut.GetAll(); // get the cached value for the first time

            Assert.IsTrue(cache.TryGetValue(cacheKey, out _)); // stored values into memoryCache due to query

            await sut.GetAll(); //  get the cached value for the second time

            // assert that should have executed one query, means no new query, because all data is already in memoryCache
            departmentsRepositoryMock.Verify(x => x.GetDepartmentNameDtos(), Times.Once);

            sut.RemoveAll();

            Assert.IsFalse(cache.TryGetValue(cacheKey, out _)); // no cache in memory after removal

            await sut.GetAll(); // get the cached value for the first time

            // assert that should have executed two query, means one new query, because the cache entries were cleared once.
            departmentsRepositoryMock.Verify(x => x.GetDepartmentNameDtos(), Times.Exactly(2));
        }
    }
}
