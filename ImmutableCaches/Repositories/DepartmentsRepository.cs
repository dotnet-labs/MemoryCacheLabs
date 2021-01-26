using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImmutableCaches.Repositories
{
    public interface IDepartmentsRepository
    {
        Task<IEnumerable<DepartmentNameDto>> GetDepartmentNameDtos();
    }

    internal class DepartmentsRepository : IDepartmentsRepository
    {
        public async Task<IEnumerable<DepartmentNameDto>> GetDepartmentNameDtos()
        {
            await Task.CompletedTask;
            return new List<DepartmentNameDto>
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
            };
        }
    }

    public class DepartmentNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
