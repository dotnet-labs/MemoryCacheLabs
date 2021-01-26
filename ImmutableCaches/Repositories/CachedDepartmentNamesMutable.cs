using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ImmutableCaches.Repositories
{
    public class CachedDepartmentNamesMutable
    {
        public const string CacheKey = nameof(CachedDepartmentNamesMutable);
        private readonly IMemoryCache _cache;
        private readonly IDepartmentsRepository _departmentsRepository;

        public CachedDepartmentNamesMutable(IMemoryCache cache, IDepartmentsRepository departmentsRepository)
        {
            _cache = cache;
            _departmentsRepository = departmentsRepository;
        }

        /// <summary>
        /// Get a list of department names.
        /// Since the department names won't change often, the system caches all the values in memory to reduce database queries.
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<DepartmentNameDto>> GetAll()
        {
            if (!_cache.TryGetValue(CacheKey, out IReadOnlyList<DepartmentNameDto> result))
            {
                var departments = await _departmentsRepository.GetDepartmentNameDtos();
                result = departments.ToList();
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.Set(CacheKey, result, options);
            }

            return result;
        }

        public async Task<DepartmentNameDto> GetDepartmentNameById(int id)
        {
            var allCachedUserNames = await GetAll();
            return allCachedUserNames.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Remove all cached DepartmentNames
        /// This method should be called when a new department added, a department removed, or a department's name updated
        /// </summary>
        public void RemoveAll()
        {
            _cache.Remove(CacheKey);
        }
    }
}
