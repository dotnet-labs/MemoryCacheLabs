using System.Threading.Tasks;
using ImmutableCaches.Controllers;
using ImmutableCaches.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImmutableCaches.UnitTests
{
    [TestClass]
    public class DepartmentsControllerTests
    {
        [TestMethod]
        public async Task GetUserNameById_ShouldReturnCorrectResponse()
        {
            var record = new DepartmentNameCacheRecord(123, "Test 123");
            var cachedDepartmentNamesMock = new Mock<ICachedDepartmentNames>();
            cachedDepartmentNamesMock.Setup(x => x.GetDepartmentNameById(123))
                .ReturnsAsync(record);

            var controller = new DepartmentsController(new NullLogger<DepartmentsController>(),
                cachedDepartmentNamesMock.Object);

            var result = await controller.GetUserNameById(123);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var responseObj = (result as OkObjectResult)?.Value;
            Assert.IsInstanceOfType(responseObj, typeof(DepartmentNameCacheRecord));
            Assert.AreEqual(responseObj, record);
        }
    }
}
