using Domain.Entities;
using Domain.Repositories;
using Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class TemperatureRepository : RepositoryBase<Temperature>, ITemperatureRepository
    {
        public TemperatureRepository(WeatherDbContext context) : base(context)
        {

        }

        public void Add(Guid cityId, Temperature temperature)
        {
            temperature.CityId = cityId;
            Create(temperature);
        }

        public async Task<IEnumerable<Temperature>> GetAllTempByCityAsync(Guid cityId, TemperatureParameters requestParameters, bool trackChanges, CancellationToken cancellationToken = default) =>
            await FindByCondition(t => t.CityId.Equals(cityId) && (t.DateTime >= requestParameters.MinDate && t.DateTime <= requestParameters.MaxDate), trackChanges)
            .Search(requestParameters.SearchTerm)
            .OrderBy(ct => ct.DateTime)
            .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
            .Take(requestParameters.PageSize)
            .ToListAsync(cancellationToken);

        public async Task<Temperature> GetTempByDateAsync(Guid cityId, DateTime temperatureDate, bool trackChanges, CancellationToken cancellationToken = default) =>
            await FindByCondition(t => t.CityId.Equals(cityId) && t.DateTime.Equals(temperatureDate), trackChanges).SingleOrDefaultAsync(cancellationToken);

        public async Task<Temperature> GetTempByIdAsync(Guid cityId, Guid temperatureId, bool trackChanges, CancellationToken cancellationToken = default) =>
            await FindByCondition(t => t.CityId.Equals(cityId) && t.Id.Equals(temperatureId), trackChanges).SingleOrDefaultAsync(cancellationToken);

        public void Remove(Temperature temperature)
        {
            Delete(temperature);
        }
    }
}
