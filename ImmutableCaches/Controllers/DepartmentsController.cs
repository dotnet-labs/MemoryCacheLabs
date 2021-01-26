using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ImmutableCaches.Repositories;

namespace ImmutableCaches.Controllers
{
    [ApiController, Produces("application/json")]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly ICachedDepartmentNames _cachedDepartmentNames;

        public DepartmentsController(ILogger<DepartmentsController> logger, ICachedDepartmentNames cachedDepartmentNames)
        {
            _logger = logger;
            _cachedDepartmentNames = cachedDepartmentNames;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUserNameById(int id)
        {
            var dept = await _cachedDepartmentNames.GetDepartmentNameById(id);
            return dept == null ? NoContent() : Ok(dept);
        }
    }
}
