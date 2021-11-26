using Domain.Entities;
using Domain.Repositories;
using Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class CityRepository : RepositoryBase<City>, ICityRepository
    {
        private readonly IMemoryCache cache;

        public CityRepository(WeatherDbContext context, IMemoryCache cache) : base(context, cache)
        {
            this.cache = cache;
        }

        public void Add(City city) => Create(city);

        public async Task<IEnumerable<City>> GetAllCitiesAsync(bool includeTemperature, CityParameters requestParameters, bool trackChanges, CancellationToken cancellationToken = default)
        {
            if (includeTemperature)
            {
                return await FindAll(include: c => c.Include(t => t.Temperatures.OrderBy(ct => ct.DateTime)),
                    trackChanges).Search(requestParameters.SearchTerm).OrderBy(c => c.Name).ToListAsync(cancellationToken);
            }
            else
            {
                return await FindAll(trackChanges).Search(requestParameters.SearchTerm).OrderBy(c => c.Name).ToListAsync(cancellationToken);
            }
        }

        public async Task<City> GetCityByIdAsync(Guid cityId, bool includeTemperature, bool trackChanges, CancellationToken cancellationToken = default)
        {
            City city = null;

            if(!cache.TryGetValue(cityId, out city))
            {
                if (includeTemperature)
                {
                    city = await FindByCondition(include: c => c.Include(t => t.Temperatures.OrderBy(ct => ct.DateTime)),
                        expression: c => c.Id.Equals(cityId),
                        trackChanges)
                        .SingleOrDefaultAsync(cancellationToken);
                }
                else
                {
                    city = await FindByCondition(c => c.Id.Equals(cityId), trackChanges).SingleOrDefaultAsync(cancellationToken);
                }

                cache.Set(city.Id, city, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            return city;
        }

        public void Remove(City city) => Delete(city);
    }
}
