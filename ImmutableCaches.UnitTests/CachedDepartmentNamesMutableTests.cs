using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImmutableCaches.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImmutableCaches.UnitTests
{
    [TestClass]
    public class CachedDepartmentNamesMutableTests
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
                    }
                });

            var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CachedDepartmentNamesMutable(cache, departmentsRepositoryMock.Object);

            var dept = await sut.GetDepartmentNameById(123);
            Assert.IsNotNull(dept);
            Assert.AreEqual("test 123", dept.Name);
            Console.WriteLine(dept.Name);
            dept.Name = "test 123123";      // accidentally updated the name for a department

            dept = await sut.GetDepartmentNameById(123);
            Console.WriteLine(dept.Name);
            Assert.AreEqual("test 123123", dept?.Name); // the value in the cache entry is changed unintentionally
        }
    }
}
