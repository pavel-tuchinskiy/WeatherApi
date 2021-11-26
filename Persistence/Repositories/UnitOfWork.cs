using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly WeatherDbContext dbContext;

        public UnitOfWork(WeatherDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> FindCurrentCityTemp(Guid cityId) =>
            await dbContext.Temperatures.Where(t => t.CityId.Equals(cityId)).OrderBy(t => t.DateTime).Select(t => t.CelsiusDegrees).LastAsync();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            dbContext.SaveChangesAsync(cancellationToken);
    }
}
